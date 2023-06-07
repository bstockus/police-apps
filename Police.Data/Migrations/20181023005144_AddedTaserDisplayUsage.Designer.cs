﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Police.Data.Migrations {
    [DbContext(typeof(PoliceDbContext))]
    [Migration("20181023005144_AddedTaserDisplayUsage")]
    partial class AddedTaserDisplayUsage {
        protected override void BuildTargetModel(ModelBuilder modelBuilder) {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Police.Business.Identity.Roles.Role", b => {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description")
                    .IsRequired()
                    .HasMaxLength(1000);

                b.Property<bool>("IsActive");

                b.Property<string>("RoleName")
                    .IsRequired()
                    .HasMaxLength(100);

                b.HasKey("Id");

                b.ToTable("Roles", "Identity");
            });

            modelBuilder.Entity("Police.Business.Identity.Roles.RolePermission", b => {
                b.Property<Guid>("RoleId");

                b.Property<string>("PermissionName");

                b.HasKey("RoleId", "PermissionName");

                b.ToTable("RolePermissions", "Identity");
            });

            modelBuilder.Entity("Police.Business.Identity.Users.User", b => {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("EmailAddress")
                    .IsRequired()
                    .HasMaxLength(255);

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<bool>("IsActive");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<string>("UserName")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<string>("WindowsSid")
                    .IsRequired()
                    .HasMaxLength(100);

                b.HasKey("Id");

                b.HasAlternateKey("WindowsSid");

                b.ToTable("Users", "Identity");
            });

            modelBuilder.Entity("Police.Business.Identity.Users.UserRole", b => {
                b.Property<Guid>("UserId");

                b.Property<Guid>("RoleId");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("UserRoles", "Identity");
            });

            modelBuilder.Entity("Police.Business.Organization.Officers.Officer", b => {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Assignment")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<string>("BadgeNumber")
                    .IsRequired()
                    .HasMaxLength(4);

                b.Property<string>("EmployeeNumber")
                    .IsRequired()
                    .HasMaxLength(8);

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<string>("JobTitle")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<string>("Rank")
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property<Guid?>("UserId");

                b.HasKey("Id");

                b.HasIndex("UserId")
                    .IsUnique()
                    .HasFilter("[UserId] IS NOT NULL");

                b.ToTable("Officers", "Org");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Incident", b => {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<int>("ApprovalStatus");

                b.Property<string>("IncidentCaseNumber")
                    .IsRequired()
                    .HasMaxLength(10);

                b.Property<DateTime>("IncidentDateAndTime");

                b.Property<Guid>("SubmitterId");

                b.HasKey("Id");

                b.HasIndex("SubmitterId");

                b.ToTable("Incidents", "RestRep");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.IncidentOfficers.IncidentOfficer", b => {
                b.Property<Guid>("IncidentId");

                b.Property<Guid>("OfficerId");

                b.Property<int>("ApprovalStatus");

                b.Property<int>("DidOfficerRequireMedicalAttention");

                b.Property<string>("DidOfficerRequireMedicalAttentionDescription")
                    .IsRequired()
                    .HasMaxLength(1000);

                b.Property<Guid>("SubmitterId");

                b.Property<int>("WasOfficerInjured");

                b.HasKey("IncidentId", "OfficerId");

                b.HasIndex("OfficerId");

                b.HasIndex("SubmitterId");

                b.ToTable("IncidentOfficers", "RestRep");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Report", b => {
                b.Property<Guid>("IncidentId");

                b.Property<Guid>("OfficerId");

                b.Property<Guid>("SubjectId");

                b.Property<int>("ApprovalStatus");

                b.Property<Guid>("SubmitterId");

                b.HasKey("IncidentId", "OfficerId", "SubjectId");

                b.HasIndex("OfficerId");

                b.HasIndex("SubmitterId");

                b.HasIndex("IncidentId", "SubjectId");

                b.ToTable("Reports", "RestRep");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Resistances.Resistance", b => {
                b.Property<Guid>("IncidentId");

                b.Property<Guid>("OfficerId");

                b.Property<Guid>("SubjectId");

                b.Property<int>("ResistanceType");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasMaxLength(1000);

                b.HasKey("IncidentId", "OfficerId", "SubjectId", "ResistanceType");

                b.ToTable("Resistances", "RestRep");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.FireArmDeadlyForceAddendum", b => {
                b.Property<Guid>("IncidentId");

                b.Property<Guid>("OfficerId");

                b.Property<Guid>("SubjectId");

                b.Property<Guid>("ResponseId");

                b.Property<string>("FireArmAmmoType")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<string>("FireArmMake")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<string>("FireArmModel")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<string>("FireArmSerialNumber")
                    .IsRequired()
                    .HasMaxLength(100);

                b.HasKey("IncidentId", "OfficerId", "SubjectId", "ResponseId");

                b.ToTable("FireArmDeadlyForceAddendums", "RestRep");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.OtherDeadlyForceAddendum", b => {
                b.Property<Guid>("IncidentId");

                b.Property<Guid>("OfficerId");

                b.Property<Guid>("SubjectId");

                b.Property<Guid>("ResponseId");

                b.Property<string>("OtherDeadlyForceDescription")
                    .IsRequired()
                    .HasMaxLength(1000);

                b.HasKey("IncidentId", "OfficerId", "SubjectId", "ResponseId");

                b.ToTable("OtherDeadlyForceAddendums", "RestRep");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.PitUsageAddendum", b => {
                b.Property<Guid>("IncidentId");

                b.Property<Guid>("OfficerId");

                b.Property<Guid>("SubjectId");

                b.Property<Guid>("ResponseId");

                b.Property<int>("PitUsageVehicleSpeed");

                b.Property<string>("SecondaryImpactBySuspectVehicleAfterPitPartsImpacted")
                    .IsRequired()
                    .HasMaxLength(1000);

                b.Property<int>("WasSecondaryImpactBySuspectVehicleAfterPit");

                b.Property<int>("WasSuspectVehicleImmobilized");

                b.HasKey("IncidentId", "OfficerId", "SubjectId", "ResponseId");

                b.ToTable("PitUsageAddendums", "RestRep");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.Response", b => {
                b.Property<Guid>("IncidentId");

                b.Property<Guid>("OfficerId");

                b.Property<Guid>("SubjectId");

                b.Property<Guid>("Id");

                b.Property<int>("ResponseType");

                b.Property<int>("WasEffective");

                b.HasKey("IncidentId", "OfficerId", "SubjectId", "Id");

                b.ToTable("Responses", "RestRep");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.TaserDisplayUsageAddendum", b => {
                b.Property<Guid>("IncidentId");

                b.Property<Guid>("OfficerId");

                b.Property<Guid>("SubjectId");

                b.Property<Guid>("ResponseId");

                b.Property<string>("TaserSerialNumber")
                    .IsRequired()
                    .HasMaxLength(20);

                b.HasKey("IncidentId", "OfficerId", "SubjectId", "ResponseId");

                b.ToTable("TaserDisplayUsageAddendums", "RestRep");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Subjects.Subject", b => {
                b.Property<Guid>("IncidentId");

                b.Property<Guid>("SubjectId");

                b.Property<int>("ApprovalStatus");

                b.Property<string>("SubjectType")
                    .IsRequired()
                    .HasMaxLength(10);

                b.Property<Guid>("SubmitterId");

                b.HasKey("IncidentId", "SubjectId");

                b.HasIndex("SubmitterId");

                b.ToTable("Subjects", "RestRep");

                b.HasDiscriminator<string>("SubjectType").HasValue("Subject");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Subjects.Animals.SubjectAnimal", b => {
                b.HasBaseType("Police.Business.ResistanceResponse.Incidents.Subjects.Subject");

                b.Property<int>("Species");

                b.ToTable("SubjectAnimal");

                b.HasDiscriminator().HasValue("Animal");
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Subjects.People.SubjectPerson", b => {
                b.HasBaseType("Police.Business.ResistanceResponse.Incidents.Subjects.Subject");

                b.Property<int>("Age");

                b.Property<DateTime?>("DateOfBirth");

                b.Property<int>("DidSubjectRequireMedicalAttention");

                b.Property<string>("DidSubjectRequireMedicalAttentionDescription")
                    .IsRequired()
                    .HasMaxLength(1000);

                b.Property<string>("FullName")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<int>("Gender");

                b.Property<int>("Race");

                b.Property<int>("SuspectedUse");

                b.Property<int>("WasSubjectInjured");

                b.ToTable("SubjectPerson");

                b.HasDiscriminator().HasValue("Person");
            });

            modelBuilder.Entity("Police.Business.Identity.Roles.RolePermission", b => {
                b.HasOne("Police.Business.Identity.Roles.Role", "Role")
                    .WithMany("RolePermissions")
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Police.Business.Identity.Users.UserRole", b => {
                b.HasOne("Police.Business.Identity.Roles.Role", "Role")
                    .WithMany("UserRoles")
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("Police.Business.Identity.Users.User", "User")
                    .WithMany("UserRoles")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Police.Business.Organization.Officers.Officer", b => {
                b.HasOne("Police.Business.Identity.Users.User", "User")
                    .WithOne()
                    .HasForeignKey("Police.Business.Organization.Officers.Officer", "UserId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Incident", b => {
                b.HasOne("Police.Business.Identity.Users.User", "Submitter")
                    .WithMany()
                    .HasForeignKey("SubmitterId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.IncidentOfficers.IncidentOfficer", b => {
                b.HasOne("Police.Business.ResistanceResponse.Incidents.Incident", "Incident")
                    .WithMany("IncidentOfficers")
                    .HasForeignKey("IncidentId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.Organization.Officers.Officer", "Officer")
                    .WithMany()
                    .HasForeignKey("OfficerId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.Identity.Users.User", "Submitter")
                    .WithMany()
                    .HasForeignKey("SubmitterId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Report", b => {
                b.HasOne("Police.Business.ResistanceResponse.Incidents.Incident", "Incident")
                    .WithMany("Reports")
                    .HasForeignKey("IncidentId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.Organization.Officers.Officer", "Officer")
                    .WithMany()
                    .HasForeignKey("OfficerId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.Identity.Users.User", "Submitter")
                    .WithMany()
                    .HasForeignKey("SubmitterId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.ResistanceResponse.Incidents.IncidentOfficers.IncidentOfficer", "IncidentOfficer")
                    .WithMany("Reports")
                    .HasForeignKey("IncidentId", "OfficerId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.ResistanceResponse.Incidents.Subjects.Subject", "Subject")
                    .WithMany("Reports")
                    .HasForeignKey("IncidentId", "SubjectId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Resistances.Resistance", b => {
                b.HasOne("Police.Business.ResistanceResponse.Incidents.Incident", "Incident")
                    .WithMany()
                    .HasForeignKey("IncidentId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.ResistanceResponse.Incidents.Reports.Report", "Report")
                    .WithMany("Resistances")
                    .HasForeignKey("IncidentId", "OfficerId", "SubjectId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.FireArmDeadlyForceAddendum", b => {
                b.HasOne("Police.Business.ResistanceResponse.Incidents.Incident", "Incident")
                    .WithMany()
                    .HasForeignKey("IncidentId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.ResistanceResponse.Incidents.Reports.Responses.Response", "Response")
                    .WithOne("FireArmDeadlyForceAddendum")
                    .HasForeignKey("Police.Business.ResistanceResponse.Incidents.Reports.Responses.FireArmDeadlyForceAddendum", "IncidentId", "OfficerId", "SubjectId", "ResponseId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.OtherDeadlyForceAddendum", b => {
                b.HasOne("Police.Business.ResistanceResponse.Incidents.Incident", "Incident")
                    .WithMany()
                    .HasForeignKey("IncidentId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.ResistanceResponse.Incidents.Reports.Responses.Response", "Response")
                    .WithOne("OtherDeadlyForceAddendum")
                    .HasForeignKey("Police.Business.ResistanceResponse.Incidents.Reports.Responses.OtherDeadlyForceAddendum", "IncidentId", "OfficerId", "SubjectId", "ResponseId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.PitUsageAddendum", b => {
                b.HasOne("Police.Business.ResistanceResponse.Incidents.Incident", "Incident")
                    .WithMany()
                    .HasForeignKey("IncidentId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.ResistanceResponse.Incidents.Reports.Responses.Response", "Response")
                    .WithOne("PitUsageAddendum")
                    .HasForeignKey("Police.Business.ResistanceResponse.Incidents.Reports.Responses.PitUsageAddendum", "IncidentId", "OfficerId", "SubjectId", "ResponseId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.Response", b => {
                b.HasOne("Police.Business.ResistanceResponse.Incidents.Incident", "Incident")
                    .WithMany()
                    .HasForeignKey("IncidentId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.ResistanceResponse.Incidents.Reports.Report", "Report")
                    .WithMany("Responses")
                    .HasForeignKey("IncidentId", "OfficerId", "SubjectId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Reports.Responses.TaserDisplayUsageAddendum", b => {
                b.HasOne("Police.Business.ResistanceResponse.Incidents.Incident", "Incident")
                    .WithMany()
                    .HasForeignKey("IncidentId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.ResistanceResponse.Incidents.Reports.Responses.Response", "Response")
                    .WithOne("TaserDisplayUsageAddendum")
                    .HasForeignKey("Police.Business.ResistanceResponse.Incidents.Reports.Responses.TaserDisplayUsageAddendum", "IncidentId", "OfficerId", "SubjectId", "ResponseId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Police.Business.ResistanceResponse.Incidents.Subjects.Subject", b => {
                b.HasOne("Police.Business.ResistanceResponse.Incidents.Incident", "Incident")
                    .WithMany("Subjects")
                    .HasForeignKey("IncidentId")
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne("Police.Business.Identity.Users.User", "Submitter")
                    .WithMany()
                    .HasForeignKey("SubmitterId")
                    .OnDelete(DeleteBehavior.Restrict);
            });
#pragma warning restore 612, 618
        }
    }
}
