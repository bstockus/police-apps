using System;
using System.Collections.Generic;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Data;

namespace Police.Business.Identity.Users {

    public class User {

        public Guid Id { get; set; }

        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public string WindowsSid { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, User> {

            public override void Build(EntityTypeBuilder<User> builder) {

                builder.ToTable("Users", "Identity");

                builder.HasKey(_ => _.Id);

                builder.HasAlternateKey(_ => _.WindowsSid);

                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.UserName).AsUserUserName();
                    rules.RuleFor(_ => _.EmailAddress).AsUserEmailAddress();
                    rules.RuleFor(_ => _.WindowsSid).AsUserWindowsSid();
                });

            }

        }

    }

}