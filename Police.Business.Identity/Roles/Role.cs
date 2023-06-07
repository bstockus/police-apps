using System;
using System.Collections.Generic;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Identity.Users;
using Police.Data;

namespace Police.Business.Identity.Roles {

    public class Role {

        public Guid Id { get; set; }

        public string RoleName { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, Role> {

            public override void Build(EntityTypeBuilder<Role> builder) {

                builder.ToTable("Roles", "Identity");

                builder.HasKey(_ => _.Id);

                builder.FromValidator(rules => {
                    rules.RuleFor(_ => _.RoleName).AsRoleRoleName();
                    rules.RuleFor(_ => _.Description).AsRoleDescription();
                });

            }

        }

    }

}
