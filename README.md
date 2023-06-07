# Resistance Response Reports

### Entity Framework Core Migrations

#### Add Migration
_Run from the Police.Web directory:_
```dotnet ef migrations add <MigrationName> -c Police.Data.PoliceDbContext -p ../Police.Data/```

#### Remove Migration
_Run from the Police.Web directory:_
```dotnet ef migrations remove -c Police.Data.PoliceDbContext -p ../Police.Data/```

#### Update Database
_Run from the Police.Web directory:_
```dotnet ef database update -c Police.Data.PoliceDbContext -p ../Police.Data/```

#### Generate Migrations Script
_Run from the Police.Web directory:_
```dotnet ef migrations script -i -c Police.Data.PoliceDbContext -p ../Police.Data/ -o ../Migrations.sql --idempotent```