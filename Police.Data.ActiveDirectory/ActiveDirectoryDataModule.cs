using Autofac;

namespace Police.Data.ActiveDirectory {

    public class ActiveDirectoryDataModule : Module {

        private readonly string _ldapConnectionString;

        public ActiveDirectoryDataModule(string ldapConnectionString) {
            _ldapConnectionString = ldapConnectionString;
        }

        protected override void Load(ContainerBuilder builder) {
            builder.Register(_ => new ActiveDirectoryDataService(_ldapConnectionString)).AsSelf()
                .InstancePerDependency();
        }

    }

}
