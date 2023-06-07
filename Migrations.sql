IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    IF SCHEMA_ID(N'Identity') IS NULL EXEC(N'CREATE SCHEMA [Identity];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    IF SCHEMA_ID(N'Org') IS NULL EXEC(N'CREATE SCHEMA [Org];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    IF SCHEMA_ID(N'RestRep') IS NULL EXEC(N'CREATE SCHEMA [RestRep];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [Identity].[Roles] (
        [Id] uniqueidentifier NOT NULL,
        [RoleName] nvarchar(100) NOT NULL,
        [Description] nvarchar(1000) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [Identity].[Users] (
        [Id] uniqueidentifier NOT NULL,
        [UserName] nvarchar(100) NOT NULL,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(200) NOT NULL,
        [EmailAddress] nvarchar(255) NOT NULL,
        [WindowsSid] nvarchar(100) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [AK_Users_WindowsSid] UNIQUE ([WindowsSid])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [Identity].[RolePermissions] (
        [RoleId] uniqueidentifier NOT NULL,
        [PermissionName] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleId], [PermissionName]),
        CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Roles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [Identity].[UserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [Org].[Officers] (
        [Id] uniqueidentifier NOT NULL,
        [BadgeNumber] nvarchar(4) NOT NULL,
        [EmployeeNumber] nvarchar(8) NOT NULL,
        [Rank] nvarchar(50) NOT NULL,
        [JobTitle] nvarchar(200) NOT NULL,
        [Assignment] nvarchar(200) NOT NULL,
        [FirstName] nvarchar(100) NOT NULL,
        [LastName] nvarchar(200) NOT NULL,
        [UserId] uniqueidentifier NULL,
        CONSTRAINT [PK_Officers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Officers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [RestRep].[Incidents] (
        [Id] uniqueidentifier NOT NULL,
        [IncidentDateAndTime] datetime2 NOT NULL,
        [IncidentCaseNumber] nvarchar(10) NOT NULL,
        [ApprovalStatus] int NOT NULL,
        [SubmitterId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Incidents] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Incidents_Users_SubmitterId] FOREIGN KEY ([SubmitterId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [RestRep].[IncidentOfficers] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [WasOfficerInjured] int NOT NULL,
        [DidOfficerRequireMedicalAttention] int NOT NULL,
        [DidOfficerRequireMedicalAttentionDescription] nvarchar(1000) NOT NULL,
        [ApprovalStatus] int NOT NULL,
        [SubmitterId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_IncidentOfficers] PRIMARY KEY ([IncidentId], [OfficerId]),
        CONSTRAINT [FK_IncidentOfficers_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_IncidentOfficers_Officers_OfficerId] FOREIGN KEY ([OfficerId]) REFERENCES [Org].[Officers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_IncidentOfficers_Users_SubmitterId] FOREIGN KEY ([SubmitterId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [RestRep].[Subjects] (
        [IncidentId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [ApprovalStatus] int NOT NULL,
        [SubmitterId] uniqueidentifier NOT NULL,
        [SubjectType] nvarchar(10) NOT NULL,
        [Species] int NULL,
        [FullName] nvarchar(200) NULL,
        [Age] int NULL,
        [Gender] int NULL,
        [Race] int NULL,
        [SuspectedUse] int NULL,
        [WasSubjectInjured] int NULL,
        [DidSubjectRequireMedicalAttention] int NULL,
        [DidSubjectRequireMedicalAttentionDescription] nvarchar(1000) NULL,
        [DateOfBirth] datetime2 NULL,
        CONSTRAINT [PK_Subjects] PRIMARY KEY ([IncidentId], [SubjectId]),
        CONSTRAINT [FK_Subjects_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Subjects_Users_SubmitterId] FOREIGN KEY ([SubmitterId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [RestRep].[Reports] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [ApprovalStatus] int NOT NULL,
        [SubmitterId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Reports] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId]),
        CONSTRAINT [FK_Reports_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Reports_Officers_OfficerId] FOREIGN KEY ([OfficerId]) REFERENCES [Org].[Officers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Reports_Users_SubmitterId] FOREIGN KEY ([SubmitterId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Reports_IncidentOfficers_IncidentId_OfficerId] FOREIGN KEY ([IncidentId], [OfficerId]) REFERENCES [RestRep].[IncidentOfficers] ([IncidentId], [OfficerId]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Reports_Subjects_IncidentId_SubjectId] FOREIGN KEY ([IncidentId], [SubjectId]) REFERENCES [RestRep].[Subjects] ([IncidentId], [SubjectId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [RestRep].[Resistances] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [ResistanceType] int NOT NULL,
        [Description] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_Resistances] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId], [ResistanceType]),
        CONSTRAINT [FK_Resistances_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Resistances_Reports_IncidentId_OfficerId_SubjectId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId]) REFERENCES [RestRep].[Reports] ([IncidentId], [OfficerId], [SubjectId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [RestRep].[Responses] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [Id] uniqueidentifier NOT NULL,
        [ResponseType] int NOT NULL,
        [WasEffective] int NOT NULL,
        CONSTRAINT [PK_Responses] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId], [Id]),
        CONSTRAINT [FK_Responses_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Responses_Reports_IncidentId_OfficerId_SubjectId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId]) REFERENCES [RestRep].[Reports] ([IncidentId], [OfficerId], [SubjectId]) ON DELETE NO ACTION
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [RestRep].[FireArmDeadlyForceAddendums] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [ResponseId] uniqueidentifier NOT NULL,
        [FireArmMake] nvarchar(100) NOT NULL,
        [FireArmModel] nvarchar(100) NOT NULL,
        [FireArmSerialNumber] nvarchar(100) NOT NULL,
        [FireArmAmmoType] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_FireArmDeadlyForceAddendums] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]),
        CONSTRAINT [FK_FireArmDeadlyForceAddendums_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_FireArmDeadlyForceAddendums_Responses_IncidentId_OfficerId_SubjectId_ResponseId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]) REFERENCES [RestRep].[Responses] ([IncidentId], [OfficerId], [SubjectId], [Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [RestRep].[OtherDeadlyForceAddendums] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [ResponseId] uniqueidentifier NOT NULL,
        [OtherDeadlyForceDescription] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_OtherDeadlyForceAddendums] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]),
        CONSTRAINT [FK_OtherDeadlyForceAddendums_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_OtherDeadlyForceAddendums_Responses_IncidentId_OfficerId_SubjectId_ResponseId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]) REFERENCES [RestRep].[Responses] ([IncidentId], [OfficerId], [SubjectId], [Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE TABLE [RestRep].[PitUsageAddendums] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [ResponseId] uniqueidentifier NOT NULL,
        [PitUsageVehicleSpeed] int NOT NULL,
        [WasSuspectVehicleImmobilized] int NOT NULL,
        [WasSecondaryImpactBySuspectVehicleAfterPit] int NOT NULL,
        [SecondaryImpactBySuspectVehicleAfterPitPartsImpacted] nvarchar(1000) NOT NULL,
        CONSTRAINT [PK_PitUsageAddendums] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]),
        CONSTRAINT [FK_PitUsageAddendums_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PitUsageAddendums_Responses_IncidentId_OfficerId_SubjectId_ResponseId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]) REFERENCES [RestRep].[Responses] ([IncidentId], [OfficerId], [SubjectId], [Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE INDEX [IX_UserRoles_RoleId] ON [Identity].[UserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Officers_UserId] ON [Org].[Officers] ([UserId]) WHERE [UserId] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE INDEX [IX_IncidentOfficers_OfficerId] ON [RestRep].[IncidentOfficers] ([OfficerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE INDEX [IX_IncidentOfficers_SubmitterId] ON [RestRep].[IncidentOfficers] ([SubmitterId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE INDEX [IX_Incidents_SubmitterId] ON [RestRep].[Incidents] ([SubmitterId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE INDEX [IX_Reports_OfficerId] ON [RestRep].[Reports] ([OfficerId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE INDEX [IX_Reports_SubmitterId] ON [RestRep].[Reports] ([SubmitterId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE INDEX [IX_Reports_IncidentId_SubjectId] ON [RestRep].[Reports] ([IncidentId], [SubjectId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    CREATE INDEX [IX_Subjects_SubmitterId] ON [RestRep].[Subjects] ([SubmitterId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181022145040_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181022145040_InitialCreate', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181023005144_AddedTaserDisplayUsage')
BEGIN
    CREATE TABLE [RestRep].[TaserDisplayUsageAddendums] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [ResponseId] uniqueidentifier NOT NULL,
        [TaserSerialNumber] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_TaserDisplayUsageAddendums] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]),
        CONSTRAINT [FK_TaserDisplayUsageAddendums_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TaserDisplayUsageAddendums_Responses_IncidentId_OfficerId_SubjectId_ResponseId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]) REFERENCES [RestRep].[Responses] ([IncidentId], [OfficerId], [SubjectId], [Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181023005144_AddedTaserDisplayUsage')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181023005144_AddedTaserDisplayUsage', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027135253_RefactoredTaserUsageAddendum')
BEGIN
    DROP TABLE [RestRep].[TaserDisplayUsageAddendums];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027135253_RefactoredTaserUsageAddendum')
BEGIN
    CREATE TABLE [RestRep].[TaserUsageAddendums] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [ResponseId] uniqueidentifier NOT NULL,
        [TaserSerialNumber] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_TaserUsageAddendums] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]),
        CONSTRAINT [FK_TaserUsageAddendums_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TaserUsageAddendums_Responses_IncidentId_OfficerId_SubjectId_ResponseId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]) REFERENCES [RestRep].[Responses] ([IncidentId], [OfficerId], [SubjectId], [Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027135253_RefactoredTaserUsageAddendum')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181027135253_RefactoredTaserUsageAddendum', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [AdditionalShotsRequired] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [AnySecondaryInjuriesFromTaserUsage] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [CameraUsedToTakePhotos] nvarchar(200) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [CyclesApplied] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [DidProbesContact] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [DidProbesPenetrateSkin] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [DistanceBetweenProbes] decimal(18,2) NOT NULL DEFAULT 0.0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [DistanceWhenLaunched] decimal(18,2) NOT NULL DEFAULT 0.0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [NumberOfPhotosTaken] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [SubjectWearingHeavyClothing] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [TaserCartridgeNumberUsed] nvarchar(20) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [WasArcDisplayUsed] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [WasDriveStunUsed] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [WasLaserDisplayUsed] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [WasMedicalAttentionRequiredForSecondaryInjuries] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [WasProbeDeployUsed] bit NOT NULL DEFAULT CAST(0 AS bit);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [WereProbesRemovedAtScene] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums] ADD [WhoRemovedProbes] nvarchar(1000) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181027141237_AddedRemainingFieldsToTaserUsageAddendum', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [SupervisorApproverId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [SupervisorsComments] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [TrainingApproverId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [TrainingsComments] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Reports] ADD [SupervisorApproverId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Reports] ADD [SupervisorsComments] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Reports] ADD [TrainingApproverId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Reports] ADD [TrainingsComments] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Incidents] ADD [SupervisorApproverId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Incidents] ADD [SupervisorsComments] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Incidents] ADD [TrainingApproverId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Incidents] ADD [TrainingsComments] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[IncidentOfficers] ADD [SupervisorApproverId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[IncidentOfficers] ADD [SupervisorsComments] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[IncidentOfficers] ADD [TrainingApproverId] uniqueidentifier NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[IncidentOfficers] ADD [TrainingsComments] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    CREATE INDEX [IX_Subjects_SupervisorApproverId] ON [RestRep].[Subjects] ([SupervisorApproverId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    CREATE INDEX [IX_Subjects_TrainingApproverId] ON [RestRep].[Subjects] ([TrainingApproverId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    CREATE INDEX [IX_Reports_SupervisorApproverId] ON [RestRep].[Reports] ([SupervisorApproverId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    CREATE INDEX [IX_Reports_TrainingApproverId] ON [RestRep].[Reports] ([TrainingApproverId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    CREATE INDEX [IX_Incidents_SupervisorApproverId] ON [RestRep].[Incidents] ([SupervisorApproverId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    CREATE INDEX [IX_Incidents_TrainingApproverId] ON [RestRep].[Incidents] ([TrainingApproverId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    CREATE INDEX [IX_IncidentOfficers_SupervisorApproverId] ON [RestRep].[IncidentOfficers] ([SupervisorApproverId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    CREATE INDEX [IX_IncidentOfficers_TrainingApproverId] ON [RestRep].[IncidentOfficers] ([TrainingApproverId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[IncidentOfficers] ADD CONSTRAINT [FK_IncidentOfficers_Users_SupervisorApproverId] FOREIGN KEY ([SupervisorApproverId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[IncidentOfficers] ADD CONSTRAINT [FK_IncidentOfficers_Users_TrainingApproverId] FOREIGN KEY ([TrainingApproverId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Incidents] ADD CONSTRAINT [FK_Incidents_Users_SupervisorApproverId] FOREIGN KEY ([SupervisorApproverId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Incidents] ADD CONSTRAINT [FK_Incidents_Users_TrainingApproverId] FOREIGN KEY ([TrainingApproverId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Reports] ADD CONSTRAINT [FK_Reports_Users_SupervisorApproverId] FOREIGN KEY ([SupervisorApproverId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Reports] ADD CONSTRAINT [FK_Reports_Users_TrainingApproverId] FOREIGN KEY ([TrainingApproverId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD CONSTRAINT [FK_Subjects_Users_SupervisorApproverId] FOREIGN KEY ([SupervisorApproverId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD CONSTRAINT [FK_Subjects_Users_TrainingApproverId] FOREIGN KEY ([TrainingApproverId]) REFERENCES [Identity].[Users] ([Id]) ON DELETE NO ACTION;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181101142003_AddedApprovalProcessRelatedFields')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181101142003_AddedApprovalProcessRelatedFields', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181104184325_AddedTaserBodyUsageLocations')
BEGIN
    CREATE TABLE [RestRep].[TaserUsageAddendums.BodyUsageLocations] (
        [IncidentId] uniqueidentifier NOT NULL,
        [OfficerId] uniqueidentifier NOT NULL,
        [SubjectId] uniqueidentifier NOT NULL,
        [ResponseId] uniqueidentifier NOT NULL,
        [BodyUsageType] int NOT NULL,
        [X] int NOT NULL,
        [Y] int NOT NULL,
        CONSTRAINT [PK_TaserUsageAddendums.BodyUsageLocations] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]),
        CONSTRAINT [FK_TaserUsageAddendums.BodyUsageLocations_Incidents_IncidentId] FOREIGN KEY ([IncidentId]) REFERENCES [RestRep].[Incidents] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TaserUsageAddendums.BodyUsageLocations_TaserUsageAddendums_IncidentId_OfficerId_SubjectId_ResponseId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId]) REFERENCES [RestRep].[TaserUsageAddendums] ([IncidentId], [OfficerId], [SubjectId], [ResponseId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181104184325_AddedTaserBodyUsageLocations')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181104184325_AddedTaserBodyUsageLocations', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181104194218_ModifiedTaserBodyUsageLocationsPrimaryKey')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums.BodyUsageLocations] DROP CONSTRAINT [PK_TaserUsageAddendums.BodyUsageLocations];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181104194218_ModifiedTaserBodyUsageLocationsPrimaryKey')
BEGIN
    ALTER TABLE [RestRep].[TaserUsageAddendums.BodyUsageLocations] ADD CONSTRAINT [PK_TaserUsageAddendums.BodyUsageLocations] PRIMARY KEY ([IncidentId], [OfficerId], [SubjectId], [ResponseId], [BodyUsageType], [X], [Y]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181104194218_ModifiedTaserBodyUsageLocationsPrimaryKey')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181104194218_ModifiedTaserBodyUsageLocationsPrimaryKey', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181120143107_ChangedPersonSubjectAgeToNullable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181120143107_ChangedPersonSubjectAgeToNullable', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181120153426_AddedFireArmDeadlyForceDescriptionField')
BEGIN
    ALTER TABLE [RestRep].[FireArmDeadlyForceAddendums] ADD [FireArmDescription] nvarchar(1000) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181120153426_AddedFireArmDeadlyForceDescriptionField')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181120153426_AddedFireArmDeadlyForceDescriptionField', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181120161751_LengthenedTaserCartdridgeAndSerialNumber')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RestRep].[TaserUsageAddendums]') AND [c].[name] = N'TaserSerialNumber');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [RestRep].[TaserUsageAddendums] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [RestRep].[TaserUsageAddendums] ALTER COLUMN [TaserSerialNumber] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181120161751_LengthenedTaserCartdridgeAndSerialNumber')
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[RestRep].[TaserUsageAddendums]') AND [c].[name] = N'TaserCartridgeNumberUsed');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [RestRep].[TaserUsageAddendums] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [RestRep].[TaserUsageAddendums] ALTER COLUMN [TaserCartridgeNumberUsed] nvarchar(50) NOT NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181120161751_LengthenedTaserCartdridgeAndSerialNumber')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181120161751_LengthenedTaserCartdridgeAndSerialNumber', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181203160200_AddedIsSwornAndIsCurrentlyEmployedFieldsToOfficers')
BEGIN
    ALTER TABLE [Org].[Officers] ADD [IsCurrentlyEmployed] bit NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181203160200_AddedIsSwornAndIsCurrentlyEmployedFieldsToOfficers')
BEGIN
    ALTER TABLE [Org].[Officers] ADD [IsSwornOfficer] bit NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20181203160200_AddedIsSwornAndIsCurrentlyEmployedFieldsToOfficers')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20181203160200_AddedIsSwornAndIsCurrentlyEmployedFieldsToOfficers', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    DROP TABLE [SubjectAnimal];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    DROP TABLE [SubjectPerson];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Identity].[Users]') AND [c].[name] = N'FirstName');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Identity].[Users] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [Identity].[Users] DROP COLUMN [FirstName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Identity].[Users]') AND [c].[name] = N'LastName');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Identity].[Users] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [Identity].[Users] DROP COLUMN [LastName];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [Age] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [DateOfBirth] datetime2 NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [DidSubjectRequireMedicalAttention] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [DidSubjectRequireMedicalAttentionDescription] nvarchar(1000) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [FullName] nvarchar(200) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [Gender] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [Race] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [Species] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [SuspectedUse] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    ALTER TABLE [RestRep].[Subjects] ADD [WasSubjectInjured] int NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210325135719_RemovedFirstAndLastNameFromUser')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210325135719_RemovedFirstAndLastNameFromUser', N'5.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210414153208_AlterResistanceAndResponseDeleteCascade')
BEGIN
    ALTER TABLE [RestRep].[Resistances] DROP CONSTRAINT [FK_Resistances_Reports_IncidentId_OfficerId_SubjectId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210414153208_AlterResistanceAndResponseDeleteCascade')
BEGIN
    ALTER TABLE [RestRep].[Responses] DROP CONSTRAINT [FK_Responses_Reports_IncidentId_OfficerId_SubjectId];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210414153208_AlterResistanceAndResponseDeleteCascade')
BEGIN
    ALTER TABLE [RestRep].[Resistances] ADD CONSTRAINT [FK_Resistances_Reports_IncidentId_OfficerId_SubjectId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId]) REFERENCES [RestRep].[Reports] ([IncidentId], [OfficerId], [SubjectId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210414153208_AlterResistanceAndResponseDeleteCascade')
BEGIN
    ALTER TABLE [RestRep].[Responses] ADD CONSTRAINT [FK_Responses_Reports_IncidentId_OfficerId_SubjectId] FOREIGN KEY ([IncidentId], [OfficerId], [SubjectId]) REFERENCES [RestRep].[Reports] ([IncidentId], [OfficerId], [SubjectId]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210414153208_AlterResistanceAndResponseDeleteCascade')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20210414153208_AlterResistanceAndResponseDeleteCascade', N'5.0.4');
END;
GO

COMMIT;
GO

