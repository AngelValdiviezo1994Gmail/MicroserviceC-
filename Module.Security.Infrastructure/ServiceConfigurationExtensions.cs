using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Common.Domain.Interfaces;
using Common.Infrastructure.Persistence;
using AutoMapper.Extensions.ExpressionMapping;

namespace Module.Security
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddServiceModuleSecurity(this IServiceCollection services)
        {
            //AutoMapper
            services.AddAutoMapper(cfg => { cfg.AddExpressionMapping(); }, Assembly.GetExecutingAssembly()); ///Automapper

            services.AddTransient<IGenericRepository<Domain.Entities.Genero>, GenericRepository<Domain.Entities.Genero, Common.Domain.Entities.Persistence.Genero>>();
            
        }
    }
}
