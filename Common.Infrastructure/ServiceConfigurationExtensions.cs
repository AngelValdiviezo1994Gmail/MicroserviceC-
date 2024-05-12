using Common.Domain.Interfaces;
using Common.Infrastructure.Helpers;
using Common.Infrastructure.Persistence;
using Common.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text;

namespace Common.Infrastructure
{
    public static class ServiceConfigurationExtensions
    {
        private static string superPath = string.Empty;

        public static void AddServiceCommonInfrastructure(this IServiceCollection services, String Cnx, string contentRootPath)
        {

            //services.AddMediatR(Assembly.GetExecutingAssembly()); //Mediator

            services.AddDbContextFactory<ApplicationContext>(options =>
            {
                /*******Produccion Test USAR ENVIROMENT EN APPSettings para definir connectionString******/
                options.UseNpgsql(Cnx);
#if DEBUG
                //  options.EnableSensitiveDataLogging(true);
#endif
            });

            services.AddSingleton<IExecutionOrchestrator, ExecutionOrchestrator>();
            services.AddSingleton<ISesionHelper, SesionHelper>();
            services.AddSingleton<IEncryptHelper, EncryptHelper>();            
            services.AddSingleton<IUtilsHelper, UtilsHelper>();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            
            superPath = contentRootPath;
            
        }

        private static void ProcessException(Exception ex, string message)
        {
            var path = Path.Combine(superPath, @"LogFiles\" + "reports.log");
            // Log exceptions here. For instance:

            using (FileStream fs = System.IO.File.Create(path))
            {
                byte[] content = new UTF8Encoding(true).GetBytes(String.Format("[{0}]: Exception occured. Message: '{1}'. Exception Details:\r\n{2}",
                DateTime.Now, message, ex));

                fs.Write(content, 0, content.Length);
            }

            //System.Diagnostics.Debug.WriteLine("[{0}]: Exception occured. Message: '{1}'. Exception Details:\r\n{2}",
            //    DateTime.Now, message, ex);
        }

    }
}
