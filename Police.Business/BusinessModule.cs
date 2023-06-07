using Autofac;
using Lax.AutoFac.AutoMapper;
using Lax.AutoFac.FluentValidation;
using Lax.AutoFac.MediatR;
using Lax.AutoFac.NodaTime;
using Lax.Business.Bus.Authorization;
using Lax.Business.Bus.Logging;
using Lax.Business.Bus.UnitOfWork;
using Lax.Business.Bus.Validation;
using Lax.Serialization.Json;
using Police.Business.Common;
using Police.Business.Identity;
using Police.Business.Organization;
using Police.Business.ResistanceResponse;
using Police.Data;
using Police.Data.ActiveDirectory;
using Police.Security;

namespace Police.Business {

    public class BusinessModule : Module {

        private readonly string _policeConnectionString;
        private readonly string _ldapConnectionString;

        public BusinessModule(
            string policeConnectionString,
            string ldapConnectionString) {
            _policeConnectionString = policeConnectionString;
            _ldapConnectionString = ldapConnectionString;
        }

        protected override void Load(ContainerBuilder builder) {

            var businessAssemblies = new[] {
                typeof(CommonBusinessModule).Assembly,
                typeof(IdentityBusinessModule).Assembly,
                typeof(OrganizationBusinessModule).Assembly,
                typeof(ResistanceResponseBusinessModule).Assembly
            };

            builder
                .RegisterMediatRBus()
                .RegisterMediatRHandlers(businessAssemblies)
                .RegisterBusUnitOfWork()
                .RegisterBusLogging()
                .RegisterBusAuthorization()
                .RegisterBusValidation();

            builder.RegisterValidators(businessAssemblies);

            builder.RegisterAutoMapperProfiles(businessAssemblies);

            builder.RegisterSystemClock();

            builder.RegisterJsonNetSerialization();

            builder
                .RegisterModule(new DataModule(businessAssemblies, _policeConnectionString))
                .RegisterModule(new ActiveDirectoryDataModule(_ldapConnectionString))
                .RegisterModule(new SecurityModule(businessAssemblies))
                .RegisterAssemblyModules(businessAssemblies);


        }

    }

}
