using Autofac;
using Police.Web.Common.Navigation;
using Police.Web.Organization.Navigation;

namespace Police.Web.Organization {

    public class OrganizationWebModule : Module {

        protected override void Load(ContainerBuilder builder) {

            builder
                .RegisterType<OrganizationNavigationSectionProvider>()
                .As<INavigationSectionProvider>()
                .InstancePerDependency();

        }

    }

}
