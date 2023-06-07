﻿using System;
using Lax.Data.Entities.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Police.Business.Identity.Roles;
using Police.Data;

namespace Police.Business.Identity.Users {

    public class UserRole {

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        public class EntityModelBuilder : EntityFrameworkModelBuilder<PoliceDbContext, UserRole> {

            public override void Build(EntityTypeBuilder<UserRole> builder) {

                builder.ToTable("UserRoles", "Identity");

                builder.HasKey(_ => new {
                    _.UserId,
                    _.RoleId
                });

                builder.HasOne(_ => _.User).WithMany(_ => _.UserRoles).HasForeignKey(_ => _.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                builder.HasOne(_ => _.Role).WithMany(_ => _.UserRoles).HasForeignKey(_ => _.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

            }

        }

    }

}