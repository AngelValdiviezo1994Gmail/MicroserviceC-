using AutoMapper.Extensions.ExpressionMapping;
using Common.Domain.Interfaces;
using Common.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Module.Cliente.Application.CQRS.CommandHandler;
using Module.Cliente.Application.CQRS.QueryHandler;
using Module.Cliente.Domain.CQRS.Query;
using Module.Cliente.Infrastructura.Client;
using Module.Cliente.Infrastructura.Interfaces.Client;
using System.Reflection;
using Modulo = Module.Cliente.Domain.Entities;
using Persistencia = Common.Domain.Entities.Persistence;

namespace Module.Cliente
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddServiceModuleAcademico(this IServiceCollection services)
        {
            //AutoMapper
            services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); }, Assembly.GetExecutingAssembly()); ///Automapper

            #region Categoría Género

            services.AddMediatR(typeof(GeneroCommandHandler));
            services.AddMediatR(typeof(GeneroQueryHandler));

            //Interface-Implementer Dependency Injection
            services.AddTransient<IGeneroClient, GeneroClient>();

            //Repositories
            services.AddTransient<IGenericRepository<Modulo.Genero>, GenericRepository<Modulo.Genero, Persistencia.Genero>>();

            #endregion

            #region Comunes            
            
            services.AddTransient<IGenericRepository<Persistencia.Genero>, GenericRepository<Persistencia.Genero, Persistencia.Genero>>();
            
            #endregion

        }
    }
}

