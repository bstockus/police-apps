using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Police.Web {

    public class Program {
        
        public static void Main(string[] args) {

            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog((context, services, configuration) =>
                    configuration.ReadFrom.Configuration(context.Configuration))
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureAppConfiguration((context, builder) => {
                            var env = context.HostingEnvironment;
                            builder
                                .AddYamlFile("appsettings.yml", optional: false)
                                .AddYamlFile($"appsettings.{env.EnvironmentName}.yml", optional: true)
                                .AddEnvironmentVariables()
                                .AddCommandLine(args);
                        });
                }).Build().Run();
        }

    }

}
