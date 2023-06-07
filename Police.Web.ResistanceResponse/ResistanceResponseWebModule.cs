using Autofac;
using Police.Web.Common.Navigation;
using Police.Web.ResistanceResponse.Navigation;

namespace Police.Web.ResistanceResponse {

    public class ResistanceResponseWebModule : Module {

        protected override void Load(ContainerBuilder builder) {

            builder
                .RegisterType<ResistanceResponseNavigationSectionProvider>()
                .As<INavigationSectionProvider>()
                .InstancePerDependency();

        }

    }

}
