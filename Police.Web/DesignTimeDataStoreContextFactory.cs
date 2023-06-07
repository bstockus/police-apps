using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Police.Business;
using Police.Data;
using Serilog;

namespace Police.Web {

    public class DesignTimeDataStoreContextFactory : IDesignTimeDbContextFactory<PoliceDbContext> {

        public PoliceDbContext CreateDbContext(string[] args) {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.LiterateConsole()
                .WriteTo.Debug()
                .CreateLogger();
            
            var builder = new ContainerBuilder();
            builder.Populate(new ServiceCollection());

            builder.RegisterModule(
                new BusinessModule(
                    @"Server=lax-sql1;Database=Police;User Id=app_PoliceApps;Password=IpW7nLMXItqTZmiVaiuU;MultipleActiveResultSets=True;",
                    ""));
            
            var container = builder.Build();

            var scope = container.BeginLifetimeScope();

            return scope.Resolve<PoliceDbContext>();
        }

    }

}