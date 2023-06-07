using System.Reflection;
using Autofac;
using Lax.Data.Entities.EntityFrameworkCore;
using Module = Autofac.Module;

namespace Police.Data {

    public class DataModule : Module {

        private readonly Assembly[] _entityAssemblies;
        private readonly string _connectionString;

        public DataModule(
            Assembly[] entityAssemblies,
            string connectionString) {
            _entityAssemblies = entityAssemblies;
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder) {

            builder.RegisterUnitOfWork();

            builder
                .RegisterEntityFrameworkContext<PoliceDbContext>()
                .RegisterEntityFrameworkModelBuilders(_entityAssemblies)
                .RegisterEntityFrameworkDbSetProviders(_entityAssemblies);

            builder
                .Register(context => new ConnectionStringProvider(_connectionString))
                .AsSelf()
                .InstancePerDependency();

        }

    }

}
