using Autofac;
using Police.Business.ResistanceResponse.Incidents;

namespace Police.Business.ResistanceResponse {

    public class ResistanceResponseBusinessModule : Module {

        protected override void Load(ContainerBuilder builder) {

            builder.RegisterType<EmailNotificationsManager>().As<IEmailNotificationsManager>().InstancePerDependency();

        }

    }

}
