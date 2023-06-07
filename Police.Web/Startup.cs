using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using Autofac;
using Hangfire;
using Lax.Business.Authorization.HttpContext;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Police.Business;
using Police.Security.Authorization;
using Police.Web.Common;
using Police.Web.Organization;
using Police.Web.ResistanceResponse;

namespace Police.Web {
    public class Startup {

        public static Assembly[] WebAssemblies = {
            typeof(CommonWebModule).Assembly,
            typeof(ResistanceResponseWebModule).Assembly,
            typeof(OrganizationWebModule).Assembly
        };

        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment webHostEnvironment) {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        public void ConfigureServices(IServiceCollection services) {

            var dbConnectionString = Configuration.GetConnectionString("PoliceDb");

            services.AddMemoryCache();

            //services.AddAuthentication(IISDefaults.AuthenticationScheme);
            services
                .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration, "AzureAD")
                .EnableTokenAcquisitionToCallDownstreamApi(new[] { "User.Read", "User.Read.All" })
                .AddMicrosoftGraph(Configuration.GetSection("GraphBeta"))
                .AddInMemoryTokenCaches();

            services.AddAuthorization(options => { options.RegisterAuthorizationPolicies(); });

            services.AddHangfire(_ => _.UseSqlServerStorage(dbConnectionString));

            services.AddMiniProfiler().AddEntityFramework();

            //services.AddAuthorization(options => { options.RegisterAuthorizationPolicies(); });


            //services.AddControllersWithViews()
            //    .AddMicrosoftIdentityUI();
            //var razorPages = services.AddRazorPages(options => {
            //        options.Conventions.AuthorizeFolder("/", AuthorizationPolicies.MustBeActiveUser);
            //    })
            //    .AddRazorRuntimeCompilation();
            //foreach (var webAssembly in WebAssemblies) {
            //    razorPages.AddApplicationPart(webAssembly);
            //}

            //services.AddHttpContextAccessor();

            //services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddControllersWithViews();
            var razorPages =
                services
                    .AddRazorPages(options => {
                        options.Conventions.AuthorizeFolder("/", AuthorizationPolicies.MustBeActiveUser);
                    })
                    .AddMicrosoftIdentityUI();

            foreach (var webAssembly in WebAssemblies) {
                razorPages.AddApplicationPart(webAssembly);
            }

            if (WebHostEnvironment.IsDevelopment()) {
                razorPages.AddRazorRuntimeCompilation();

                //services.Configure<MvcRazorRuntimeCompilationOptions>(options => {
                //    var libraryPartialPath = Path.Combine(WebHostEnvironment.ContentRootPath, "..",
                //        typeof(ClerksLicensingWebModule).Assembly.GetName().Name ?? string.Empty);
                //    var libraryPath = Path.GetFullPath(libraryPartialPath);
                //    options.FileProviders.Add(new PhysicalFileProvider(libraryPath));
                //});

                services.AddMiniProfiler().AddEntityFramework();

                services.AddDatabaseDeveloperPageExceptionFilter();
            }



            services.AddHttpContextAccessor();

        }

        public void ConfigureContainer(ContainerBuilder builder) {
            var policeConnectionString = Configuration.GetConnectionString("PoliceDb");
            var ldapConnectionString = Configuration.GetConnectionString("ActiveDirectory");


            builder
                .RegisterModule(
                    new BusinessModule(
                        policeConnectionString,
                        ldapConnectionString))
                .RegisterModule<WebModule>()
                .RegisterAssemblyModules(WebAssemblies);

            builder.RegisterHttpContextAuthorizationUserProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            if (env.IsDevelopment()) {

                app.UseDeveloperExceptionPage();
                app.UseMiniProfiler();

            } else {

                app.UseExceptionHandler("/Error");
                app.UseHsts();

            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            var impersonateUser = Configuration.GetImpersonateUser();

            if (impersonateUser != null && env.IsDevelopment()) {
                app.Use(async (context, next) => {

                    var claimsIdentity = new ClaimsIdentity(new List<Claim> {
                        new Claim(ClaimTypes.PrimarySid, impersonateUser)
                    }, HttpSysDefaults.AuthenticationScheme);

                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    context.User = claimsPrincipal;

                    await next.Invoke();
                });
            } else {
                app.UseAuthentication();
            }

            app.UseAuthorization();

            if (!env.IsDevelopment()) {
                app.UseHangfireServer();
            }

            app.UseHangfireDashboard("/__admin/hangfire", new DashboardOptions {
                Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

        }
    }

}
