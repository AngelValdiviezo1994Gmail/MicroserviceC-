using Common.Domain.Entities;
using Common.Domain.Enums;
using Common.Domain.Interfaces;
using Common.Domain.Wrappers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Common.Domain.Extensiones;
using Microsoft.EntityFrameworkCore.Query;
using AutoMapper.Execution;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Common.Infrastructure.Persistence
{

    public class GenericRepository<TDomain, TEntity> : IGenericRepository<TDomain> where TDomain : class where TEntity : class
    {
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;

        private readonly IExecutionOrchestrator _executor;

        public GenericRepository(IExecutionOrchestrator executor, IDbContextFactory<ApplicationContext> contextFactory)
        {
            _executor = executor;
            _contextFactory = contextFactory;
        }

        #region TRANSACTIONS

        public async Task<TDomain> Insert(TDomain entity)
        {
            var entityDB = _executor.Mapper.Map<TEntity>(entity);
            using (var _context = _contextFactory.CreateDbContext())
            {
                _context.Set<TEntity>().Add(entityDB);
                await _context.SaveChangesAsync();
            }
            return _executor.Mapper.Map<TDomain>(entityDB);
        }

        public async Task<IEnumerable<TDomain>> InsertRange(IEnumerable<TDomain> entity)
        {
            if (!entity.Any())
            {
                return entity;
            }
            var entityDB = _executor.Mapper.Map<IEnumerable<TEntity>>(entity);
            using (var _context = _contextFactory.CreateDbContext())
            {
                _context.Set<TEntity>().AddRange(entityDB);
                await _context.SaveChangesAsync();
            }
            return _executor.Mapper.Map<IEnumerable<TDomain>>(entityDB);
        }

        public async Task<TDomain> Update(TDomain entity)
        {

            var entityDB = _executor.Mapper.Map<TEntity>(entity);
            using (var _context = _contextFactory.CreateDbContext())
            {
                _context.Update(entityDB);
                await _context.SaveChangesAsync();
            }
            return _executor.Mapper.Map<TDomain>(entityDB);
        }
        public async Task<IEnumerable<TDomain>> Update(List<TDomain> entities)
        {
            var entitiesDB = _executor.Mapper.Map<IEnumerable<TEntity>>(entities);
            using (var _context = _contextFactory.CreateDbContext())
            {
                _context.UpdateRange(entitiesDB);
                await _context.SaveChangesAsync();
            }
            return _executor.Mapper.Map<List<TDomain>>(entitiesDB);
        }

        public async Task<bool> Delete(TDomain entityToDelete)
        {
            var entitiesDB = _executor.Mapper.Map<TEntity>(entityToDelete);
            using (var _context = _contextFactory.CreateDbContext())
            {
                _context.Set<TEntity>().Remove(entitiesDB);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
        }

        public async Task<bool> Delete(List<TDomain> entities)
        {
            var entitiesDB = _executor.Mapper.Map<IEnumerable<TEntity>>(entities);
            using (var _context = _contextFactory.CreateDbContext())
            {
                _context.Set<TEntity>().RemoveRange(entitiesDB);
                int result = await _context.SaveChangesAsync();
                return result > 0;
            }
        }

        public async Task<bool> Exists(string propertyName, string propertyValue)
        {
            var entityType = typeof(TEntity);

            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, propertyName);
            var propertyType = property.Type;
            var value = Convert.ChangeType(propertyValue, propertyType);


            var predicate = Expression.Lambda<Func<TEntity, bool>>(
            Expression.Equal(property, Expression.Constant(value, propertyType)),
            parameter);


            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = _context.Set<TEntity>().AsNoTracking().Where(convertedExpression).FirstOrDefault();
                return res != null;
            }
        }

        #endregion

        #region SELECTABLES PAGED RESPONSE

        public async Task<ResponsePaged<List<TDomain>>> SelectByPaged(Expression<Func<TDomain, bool>> predicate, int pagina, int cantidad)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            bool paginacion = true;
            int total = await this.SelectCountBy(predicate);
            if (cantidad == -1)
            {
                cantidad = total;
                paginacion = false;
            }
            decimal totalPaginas = Math.Ceiling((decimal)total / (decimal)cantidad);
            if (total < cantidad) totalPaginas = 1;
            if (pagina > totalPaginas)
            {
                pagina = Convert.ToInt32(totalPaginas);
            }

            using (var _context = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();
                query = query.Where(convertedExpression);

                if (paginacion)
                {
                    if (pagina <= 1)
                    {
                        query = query.Take(cantidad);
                    }
                    else
                    {
                        query = query.Skip((pagina - 1) * cantidad).Take(cantidad);
                    }
                }

                var datosResponse = _executor.Mapper.Map<List<TDomain>>(query.ToList());

                return new ResponsePaged<List<TDomain>>()
                {
                    Data = datosResponse,
                    Cantidad = cantidad,
                    Pagina = pagina,
                    TotalElementos = total,
                    TotalPaginas = Convert.ToInt32(totalPaginas),
                    Filtros = null,
                    Orden = null
                };

            }
        }

        public async Task<ResponsePaged<List<TDomain>>> SelectByPaged(Expression<Func<TDomain, bool>> predicate, int pagina, int cantidad, OrdenFiltro? orden)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);

            bool paginacion = true;
            int total = await this.SelectCountBy(predicate);
            if (cantidad == -1)
            {
                cantidad = total;
                paginacion = false;
            }


            using (var _context = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();

                query = query.Where(convertedExpression);
                if (query.Count() != total)
                {
                    total = query.Count();
                }

                if (pagina <= 1)
                {
                    query = query.Take(cantidad);
                }
                else
                {
                    query = query.Skip((pagina - 1) * cantidad).Take(cantidad);
                }

                if (orden != null)
                {
                    //var propertyInfo = typeof(TEntity).GetProperty(orden.PropiedadBusqueda, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    switch (orden.Orden)
                    {
                        case OrdenFiltroEnum.ASC:
                            query = query.OrderBy(orden.PropiedadBusqueda!);
                            //query = query.OrderBy( o =>  propertyInfo.GetValue(o,null) );
                            break;
                        case OrdenFiltroEnum.DESC:
                            query = query.OrderByDescending(orden.PropiedadBusqueda!);
                            //query = query.OrderByDescending( o => propertyInfo.Attributes );
                            break;
                    }
                }

                decimal totalPaginas = Math.Ceiling((decimal)total / (decimal)cantidad);
                if (total < cantidad) totalPaginas = 1;
                if (pagina > totalPaginas)
                {
                    pagina = Convert.ToInt32(totalPaginas);
                }


                var datosResponse = _executor.Mapper.Map<List<TDomain>>(query.ToList());

                return new ResponsePaged<List<TDomain>>()
                {
                    Data = datosResponse,
                    Cantidad = cantidad,
                    Pagina = pagina,
                    TotalElementos = total,
                    TotalPaginas = Convert.ToInt32(totalPaginas),
                    Filtros = null,
                    Orden = orden
                };

            }
        }

        public async Task<ResponsePaged<List<TDomain>>> SelectByPaged(Expression<Func<TDomain, bool>> predicate, int pagina, int cantidad, OrdenFiltro? orden, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);

            bool paginacion = true;
            int total = await this.SelectCountBy(predicate);
            if (cantidad == -1)
            {
                cantidad = total;
                paginacion = false;
            }


            using (var _context = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();

                if (includeProperties != null)
                {
                    Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>>(includeProperties);

                    var expre = includes.Compile();

                    query = expre(query);
                }

                query = query.Where(convertedExpression);
                if (query.Count() != total)
                {
                    total = query.Count();
                }


                if (pagina <= 1)
                {
                    query = query.Take(cantidad);
                }
                else
                {
                    query = query.Skip((pagina - 1) * cantidad).Take(cantidad);
                }

                if (orden != null)
                {
                    //var propertyInfo = typeof(TEntity).GetProperty(orden.PropiedadBusqueda, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    switch (orden.Orden)
                    {
                        case OrdenFiltroEnum.ASC:
                            query = query.OrderBy(orden.PropiedadBusqueda!);
                            //query = query.OrderBy( o =>  propertyInfo.GetValue(o,null) );
                            break;
                        case OrdenFiltroEnum.DESC:
                            query = query.OrderByDescending(orden.PropiedadBusqueda!);
                            //query = query.OrderByDescending( o => propertyInfo.Attributes );
                            break;
                    }
                }

                decimal totalPaginas = Math.Ceiling((decimal)total / (decimal)cantidad);
                if (total < cantidad) totalPaginas = 1;
                if (pagina > totalPaginas)
                {
                    pagina = Convert.ToInt32(totalPaginas);
                }

                var datosResponse = _executor.Mapper.Map<List<TDomain>>(query.ToList());

                return new ResponsePaged<List<TDomain>>()
                {
                    Data = datosResponse,
                    Cantidad = cantidad,
                    Pagina = pagina,
                    TotalElementos = total,
                    TotalPaginas = Convert.ToInt32(totalPaginas),
                    Filtros = null,
                    Orden = orden
                };

            }
        }

        public async Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>>> OrderBy, OrdenFiltro? orden)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);

            bool paginacion = true;
            int total = await this.SelectCountBy(predicate);
            if (cantidad == -1)
            {
                cantidad = total;
                paginacion = false;
            }


            using (var _context = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();

                query = query.Where(convertedExpression);
                if (query.Count() != total)
                {
                    total = query.Count();
                }

                if (pagina <= 1)
                {
                    query = query.Take(cantidad);
                }
                else
                {
                    query = query.Skip((pagina - 1) * cantidad).Take(cantidad);
                }

                if (orden != null)
                {
                    //var propertyInfo = typeof(TEntity).GetProperty(orden.PropiedadBusqueda, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    switch (orden.Orden)
                    {
                        case OrdenFiltroEnum.ASC:
                            query = query.OrderBy(orden.PropiedadBusqueda!);
                            //query = query.OrderBy( o =>  propertyInfo.GetValue(o,null) );
                            break;
                        case OrdenFiltroEnum.DESC:
                            query = query.OrderByDescending(orden.PropiedadBusqueda!);
                            //query = query.OrderByDescending( o => propertyInfo.Attributes );
                            break;
                    }
                }
                else
                {
                    Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> convertedOrder = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>>(OrderBy);
                    var orderes = convertedOrder.Compile();
                    query = orderes(query);
                }

                decimal totalPaginas = Math.Ceiling((decimal)total / (decimal)cantidad);
                if (total < cantidad) totalPaginas = 1;
                if (pagina > totalPaginas)
                {
                    pagina = Convert.ToInt32(totalPaginas);
                }

                var datosResponse = _executor.Mapper.Map<List<TDomain>>(query.ToList());

                return new ResponsePaged<List<TDomain>>()
                {
                    Data = datosResponse,
                    Cantidad = cantidad,
                    Pagina = pagina,
                    TotalElementos = total,
                    TotalPaginas = Convert.ToInt32(totalPaginas),
                    Filtros = null,
                    Orden = orden
                };

            }
        }

        public async Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>>> OrderBy, OrdenFiltro? orden, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);

            bool paginacion = true;
            int total = await this.SelectAllCount();
            if (predicate != null)
            {
                total = await this.SelectCountBy(predicate);
            }

            if (cantidad == -1)
            {
                cantidad = total;
                paginacion = false;
            }


            using (var _context = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();

                if (includeProperties != null)
                {
                    Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>>(includeProperties);

                    var expre = includes.Compile();

                    query = expre(query);
                }

                if (predicate != null)
                {
                    query = query.Where(convertedExpression);
                }

                if (query.Count() != total)
                {
                    total = query.Count();
                }


                if (pagina <= 1)
                {
                    query = query.Take(cantidad);
                }
                else
                {
                    query = query.Skip((pagina - 1) * cantidad).Take(cantidad);
                }

                if (orden != null)
                {
                    //var propertyInfo = typeof(TEntity).GetProperty(orden.PropiedadBusqueda, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    switch (orden.Orden)
                    {
                        case OrdenFiltroEnum.ASC:
                            query = query.OrderBy(orden.PropiedadBusqueda!);
                            //query = query.OrderBy( o =>  propertyInfo.GetValue(o,null) );
                            break;
                        case OrdenFiltroEnum.DESC:
                            query = query.OrderByDescending(orden.PropiedadBusqueda!);
                            //query = query.OrderByDescending( o => propertyInfo.Attributes );
                            break;
                    }
                }
                else
                {
                    Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> convertedOrder = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>>(OrderBy);
                    var orderes = convertedOrder.Compile();
                    query = orderes(query);
                }

                decimal totalPaginas = Math.Ceiling((decimal)total / (decimal)cantidad);
                if (total < cantidad) totalPaginas = 1;
                if (pagina > totalPaginas)
                {
                    pagina = Convert.ToInt32(totalPaginas);
                }

                var datosResponse = _executor.Mapper.Map<List<TDomain>>(query.ToList());

                return new ResponsePaged<List<TDomain>>()
                {
                    Data = datosResponse,
                    Cantidad = cantidad,
                    Pagina = pagina,
                    TotalElementos = total,
                    TotalPaginas = Convert.ToInt32(totalPaginas),
                    Filtros = null,
                    Orden = orden
                };

            }
        }

        #endregion

        #region SELECTABLES INUMERABLES 
        public async Task<IEnumerable<TDomain>> SelectBy(Expression<Func<TDomain, bool>> predicate)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = _context.Set<TEntity>().AsNoTracking().Where(convertedExpression).ToList();
                return _executor.Mapper.Map<List<TDomain>>(res);
            }
        }

        public async Task<IEnumerable<TDomain>> SelectBy(Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.Set<TEntity>().AsNoTracking();
                if (includeProperties != null)
                {
                    Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>>(includeProperties);

                    var expre = includes.Compile();

                    query = expre(query);
                }
                query = query.Where(convertedExpression);

                return _executor.Mapper.Map<List<TDomain>>(query.ToList());
            }
        }

        public async Task<IEnumerable<TDomain>> SelectBy(Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties, Expression<Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>>> OrderBy)
        {

            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.Set<TEntity>().AsNoTracking();
                if (includeProperties != null)
                {
                    Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>>(includeProperties);

                    var expre = includes.Compile();

                    query = expre(query);
                }

                if (predicate != null)
                {
                    Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
                    query = query.Where(convertedExpression);
                }


                if (OrderBy != null)
                {
                    Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> convertedOrder = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>>(OrderBy);
                    var orderes = convertedOrder.Compile();
                    query = orderes(query);
                }

                return _executor.Mapper.Map<List<TDomain>>(query.ToList());
            }
        }

        public async Task<List<TDomain>> SelectListBy(Expression<Func<TDomain, bool>> predicate)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = _context.Set<TEntity>().AsNoTracking().Where(convertedExpression).ToList();
                return _executor.Mapper.Map<List<TDomain>>(res);
            }
        }
        #endregion

        #region QUERYS SIMPLES
        public DbSet<TDomain> GetAll()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                return _context.Set<TDomain>();
            }
        }

        public async Task<TDomain> SelectFisrOrDefault(Expression<Func<TDomain, bool>> predicate)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = _context.Set<TEntity>().AsNoTracking().Where(convertedExpression).FirstOrDefault();
                return _executor.Mapper.Map<TDomain>(res);
            }
        }

        public async Task<TDomain> SelectFisrOrDefault(Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.Set<TEntity>().AsNoTracking();
                if (includeProperties != null)
                {
                    Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>>(includeProperties);
                    var expre = includes.Compile();

                    query = expre(query);
                }
                var res = query.Where(convertedExpression).FirstOrDefault();
                return _executor.Mapper.Map<TDomain>(res);
            }
        }

        public async Task<int> SelectCountBy(Expression<Func<TDomain, bool>> predicate)
        {
            Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
            using (var _context = _contextFactory.CreateDbContext())
            {
                int res = _context.Set<TEntity>().AsNoTracking().Where(convertedExpression).Count();
                return res;
            }
        }

        public async Task<List<TDomain>> SelectAll()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = _context.Set<TEntity>().AsNoTracking().ToList();
                return _executor.Mapper.Map<List<TDomain>>(res);
            }
        }

        public async Task<List<TDomain>> SelectAll(Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var res = _context.Set<TEntity>().AsNoTracking();
                if (includeProperties != null)
                {
                    Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>>(includeProperties);

                    var expre = includes.Compile();

                    res = expre(res);
                }
                return _executor.Mapper.Map<List<TDomain>>(res.ToList());
            }
        }

        public async Task<List<TDomain>> SelectAll(Expression<Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>>> OrderBy)
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                var query = _context.Set<TEntity>().AsNoTracking();

                Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> convertedOrder = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>>>(OrderBy);
                var orderes = convertedOrder.Compile();
                query = orderes(query);

                return _executor.Mapper.Map<List<TDomain>>(query.ToList());
            }

        }

        public async Task<int> SelectAllCount()
        {
            using (var _context = _contextFactory.CreateDbContext())
            {
                int res = _context.Set<TEntity>().AsNoTracking().Count();
                return res;
            }
        }

        #endregion

        #region DEJADOS POR COMPATIBILIDAD
        public async Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, List<Filtro>? filtros, OrdenFiltro? orden)
        {
            bool paginacion = true;
            int total = await this.SelectAllCount();
            if (cantidad == -1)
            {
                cantidad = total;
                paginacion = false;
            }


            using (var _context = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();
                if (orden != null)
                {
                    //var propertyInfo = typeof(TEntity).GetProperty(orden.PropiedadBusqueda, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    switch (orden.Orden)
                    {
                        case OrdenFiltroEnum.ASC:
                            query = query.OrderBy(orden.PropiedadBusqueda!);
                            //query = query.OrderBy( o =>  propertyInfo.GetValue(o,null) );
                            break;
                        case OrdenFiltroEnum.DESC:
                            query = query.OrderByDescending(orden.PropiedadBusqueda!);
                            //query = query.OrderByDescending( o => propertyInfo.Attributes );
                            break;
                    }
                }

                if (filtros != null && filtros.Count > 0)
                {
                    foreach (var filtro in filtros)
                    {
                        if (filtro != null)
                        {
                            switch (filtro.Operacion)
                            {
                                case "==":
                                    query = query.Where(t => t.GetType().GetProperty(filtro.PropiedadBusqueda).GetValue(t) == filtro.TerminoBusqueda);
                                    break;
                                case "!=":
                                    query = query.Where(t => t.GetType().GetProperty(filtro.PropiedadBusqueda).GetValue(t) != filtro.TerminoBusqueda);
                                    break;
                                case "LIKE":
                                    query = query.Where(t => t.GetType().GetProperty(filtro.PropiedadBusqueda).GetValue(t).ToString().Contains(filtro.TerminoBusqueda.ToString()));

                                    break;
                            }
                        }

                    }

                    if (query.Count() != total)
                    {
                        total = query.Count();
                    }
                }



                if (paginacion)
                {
                    if (pagina <= 1)
                    {
                        query = query.Take(cantidad);
                    }
                    else
                    {
                        query = query.Skip((pagina - 1) * cantidad).Take(cantidad);
                    }
                }

                decimal totalPaginas = Math.Ceiling((decimal)total / (decimal)cantidad);
                if (total < cantidad) totalPaginas = 1;
                if (pagina > totalPaginas)
                {
                    pagina = Convert.ToInt32(totalPaginas);
                }

                var datosResponse = _executor.Mapper.Map<List<TDomain>>(query.ToList());

                return new ResponsePaged<List<TDomain>>()
                {
                    Data = datosResponse,
                    Cantidad = cantidad,
                    Pagina = pagina,
                    TotalElementos = total,
                    TotalPaginas = Convert.ToInt32(totalPaginas),
                    Filtros = filtros,
                    Orden = orden
                };
            }
        }

        public async Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, Expression<Func<TDomain, bool>> predicate, OrdenFiltro? orden)
        {
            bool paginacion = true;

            int total = await this.SelectAllCount();

            if (cantidad == -1)
            {
                cantidad = total;
                paginacion = false;
            }

            using (var _context = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();

                if (predicate != null)
                {
                    Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
                    query = query.Where(convertedExpression);

                    if (query.Count() != total)
                    {
                        total = query.Count();
                    }
                }

                if (orden != null)
                {
                    //var propertyInfo = typeof(TEntity).GetProperty(orden.PropiedadBusqueda, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    switch (orden.Orden)
                    {
                        case OrdenFiltroEnum.ASC:
                            query = query.OrderBy(orden.PropiedadBusqueda!);
                            //query = query.OrderBy( o =>  propertyInfo.GetValue(o,null) );
                            break;
                        case OrdenFiltroEnum.DESC:
                            query = query.OrderByDescending(orden.PropiedadBusqueda!);
                            //query = query.OrderByDescending( o => propertyInfo.Attributes );
                            break;
                    }
                }

                if (paginacion)
                {
                    if (pagina <= 1)
                    {
                        query = query.Take(cantidad);
                    }
                    else
                    {
                        query = query.Skip((pagina - 1) * cantidad).Take(cantidad);
                    }
                }

                decimal totalPaginas = Math.Ceiling((decimal)total / (decimal)cantidad);
                if (total < cantidad) totalPaginas = 1;
                if (pagina > totalPaginas)
                {
                    pagina = Convert.ToInt32(totalPaginas);
                }

                var datosResponse = _executor.Mapper.Map<List<TDomain>>(query.ToList());

                return new ResponsePaged<List<TDomain>>()
                {
                    Data = datosResponse,
                    Cantidad = cantidad,
                    Pagina = pagina,
                    TotalElementos = total,
                    TotalPaginas = Convert.ToInt32(totalPaginas),
                    Filtros = null,
                    Orden = orden
                };
            }
        }

        public async Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, Expression<Func<TDomain, bool>> predicate, OrdenFiltro? orden, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties)
        {
            bool paginacion = true;
            int total = await this.SelectAllCount();
            if (cantidad == -1)
            {
                cantidad = total;
                paginacion = false;
            }

            using (var _context = _contextFactory.CreateDbContext())
            {
                IQueryable<TEntity> query = _context.Set<TEntity>().AsNoTracking();

                if (includeProperties != null)
                {
                    Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = _executor.Mapper.Map<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>>(includeProperties);

                    var expre = includes.Compile();

                    query = expre(query);
                }

                if (predicate != null)
                {
                    Expression<Func<TEntity, bool>> convertedExpression = _executor.Mapper.Map<Expression<Func<TEntity, bool>>>(predicate);
                    query = query.Where(convertedExpression);

                    if (query.Count() != total)
                    {
                        total = query.Count();
                    }
                }

                if (orden != null)
                {
                    //var propertyInfo = typeof(TEntity).GetProperty(orden.PropiedadBusqueda, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    switch (orden.Orden)
                    {
                        case OrdenFiltroEnum.ASC:
                            query = query.OrderBy(orden.PropiedadBusqueda!);
                            //query = query.OrderBy( o =>  propertyInfo.GetValue(o,null) );
                            break;
                        case OrdenFiltroEnum.DESC:
                            query = query.OrderByDescending(orden.PropiedadBusqueda!);
                            //query = query.OrderByDescending( o => propertyInfo.Attributes );
                            break;
                    }
                }

                if (paginacion)
                {
                    if (pagina <= 1)
                    {
                        query = query.Take(cantidad);
                    }
                    else
                    {
                        query = query.Skip((pagina - 1) * cantidad).Take(cantidad);
                    }
                }

                decimal totalPaginas = Math.Ceiling((decimal)total / (decimal)cantidad);
                if (total < cantidad) totalPaginas = 1;
                if (pagina > totalPaginas)
                {
                    pagina = Convert.ToInt32(totalPaginas);
                }

                var datosResponse = _executor.Mapper.Map<List<TDomain>>(query.ToList());

                return new ResponsePaged<List<TDomain>>()
                {
                    Data = datosResponse,
                    Cantidad = cantidad,
                    Pagina = pagina,
                    TotalElementos = total,
                    TotalPaginas = Convert.ToInt32(totalPaginas),
                    Filtros = null,
                    Orden = orden
                };
            }
        }

        #endregion

    }
}

public static class LinqExtensions
{
    public static IQueryable<TSource> WhereEqual<TSource, TProperty>(this IQueryable<TSource> query, Expression<Func<TSource, TProperty>> propertySelector, TProperty value)
    {
        var body2 = Expression.Equal(propertySelector.Body, Expression.Constant(value));
        var lambda = Expression.Lambda<Func<TSource, bool>>(body2, propertySelector.Parameters);
        return query.Where(lambda);
    }
    public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> query, string property, object value)
    {
        IQueryable<TSource> values =
            query.Where(t => t.GetType().GetProperty(property).GetValue(t, null) == value);
        return values;
    }
}
