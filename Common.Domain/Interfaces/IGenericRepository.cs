using Common.Domain.Entities;
using Common.Domain.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Common.Domain.Interfaces
{
    public interface IGenericRepository<TDomain> where TDomain : class
    {

        Task<List<TDomain>> SelectAll();

        Task<List<TDomain>> SelectAll(Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties);

        Task<List<TDomain>> SelectAll(Expression<Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>>> OrderBy);

        DbSet<TDomain> GetAll();

        Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, List<Filtro>? filtros, OrdenFiltro? orden);
        Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, Expression<Func<TDomain, bool>> predicate, OrdenFiltro? orden);
        Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>>> OrderBy, OrdenFiltro? orden);
        Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>>> OrderBy, OrdenFiltro? orden, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties);
        Task<ResponsePaged<List<TDomain>>> SelectAllPaged(int pagina, int cantidad, Expression<Func<TDomain, bool>> predicate, OrdenFiltro? orden, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties);
        Task<ResponsePaged<List<TDomain>>> SelectByPaged(Expression<Func<TDomain, bool>> predicate, int pagina, int cantidad);
        Task<ResponsePaged<List<TDomain>>> SelectByPaged(Expression<Func<TDomain, bool>> predicate, int pagina, int cantidad, OrdenFiltro? orden);
        Task<ResponsePaged<List<TDomain>>> SelectByPaged(Expression<Func<TDomain, bool>> predicate, int pagina, int cantidad, OrdenFiltro? orden, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties);
        Task<IEnumerable<TDomain>> SelectBy(Expression<Func<TDomain, bool>> predicate);
        Task<IEnumerable<TDomain>> SelectBy(Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties);
        Task<IEnumerable<TDomain>> SelectBy(Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties, Expression<Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>>> OrderBy);
        Task<int> SelectCountBy(Expression<Func<TDomain, bool>> predicate);
        Task<int> SelectAllCount();

        Task<TDomain> SelectFisrOrDefault(Expression<Func<TDomain, bool>> predicate);
        Task<TDomain> SelectFisrOrDefault(Expression<Func<TDomain, bool>> predicate, Expression<Func<IQueryable<TDomain>, IIncludableQueryable<TDomain, object>>> includeProperties);

        Task<TDomain> Insert(TDomain entity);
        Task<IEnumerable<TDomain>> InsertRange(IEnumerable<TDomain> entity);

        Task<TDomain> Update(TDomain entity);
        Task<IEnumerable<TDomain>> Update(List<TDomain> entity);
        Task<bool> Delete(TDomain entityToDelete);
        Task<bool> Delete(List<TDomain> entities);
        Task<bool> Exists(string propertyName, string propertyValue);
    }
}
