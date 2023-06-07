using System;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Identity.Users;
using Police.Data;

namespace Police.Business.Organization.Officers {

    public class Officer {

        public Guid Id { get; set; }
        
        public string BadgeNumber { get; set; }
        public string EmployeeNumber { get; set; }
        public string Rank { get; set; }
        public string JobTitle { get; set; }
        public string Assignment { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool? IsCurrentlyEmployed { get; set; }
        public bool? IsSwornOfficer { get; set; }

        public Guid? UserId { get; set; }
        public virtual User User { get; set; }

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, Officer> {

            public override void Build(EntityTypeBuilder<Officer> builder) {

                builder.ToTable("Officers", "Org");

                builder.HasKey(_ => _.Id);

                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.BadgeNumber).AsOfficerBadgeNumber();
                    rules.RuleFor(_ => _.EmployeeNumber).AsOfficerEmployeeNumber();
                    rules.RuleFor(_ => _.Rank).AsOfficerRank();
                    rules.RuleFor(_ => _.JobTitle).AsOfficerJobTitle();
                    rules.RuleFor(_ => _.Assignment).AsOfficerAssignment();
                    rules.RuleFor(_ => _.FirstName).AsOfficerFirstName();
                    rules.RuleFor(_ => _.LastName).AsOfficerLastName();
                });

                builder.HasOne(_ => _.User).WithOne().HasForeignKey<Officer>(_ => _.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

            }

        }

    }

}