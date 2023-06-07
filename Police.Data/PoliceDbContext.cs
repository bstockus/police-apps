using System.Collections.Generic;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Police.Data {

    public class PoliceDbContext : DbContext {

        private readonly ConnectionStringProvider _connectionStringProvider;
        private readonly IEnumerable<IEntityFrameworkModelBuilder<PoliceDbContext>> _entityFrameworkModelBuilders;

        public PoliceDbContext(
            ConnectionStringProvider connectionStringProvider,
            IEnumerable<IEntityFrameworkModelBuilder<PoliceDbContext>> entityFrameworkModelBuilders) {

            _connectionStringProvider = connectionStringProvider;
            _entityFrameworkModelBuilders = entityFrameworkModelBuilders;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);


            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(
                _connectionStringProvider.ConnectionString,
                sqlOption => { sqlOption.MigrationsAssembly(typeof(PoliceDbContext).Assembly.FullName); });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            foreach (var entityFrameworkModelBuilder in _entityFrameworkModelBuilders) {
                entityFrameworkModelBuilder.Build(modelBuilder);
            }
        }

    }

}