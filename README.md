# Kayord

Wazzup

## EF Core Migration

```bash
# Create Migration
dotnet ef migrations add Initial --project src/Kayord.Data --startup-project src/Kayord.Api --output-dir Migrations
# Update
dotnet ef database update --project src/Kayord.Data --startup-project src/Kayord.Api
# Run
dotnet run --project src/Kayord.Api


# Test
dotnet test -l "console;verbosity=detailed"

#Scaffold
#dotnet ef dbcontext scaffold "Name=ConnectionStrings:DefaultConnection" Npgsql.EntityFrameworkCore.PostgreSQL
dotnet ef dbcontext list --project src/Kayord.Api

dotnet ef dbcontext scaffold -s src/Kayord.Api -p src/Kayord.Data -o DAL/Kayord "Name=ConnectionStrings:DefaultConnection" Npgsql.EntityFrameworkCore.PostgreSQL --no-pluralize -f --no-onconfiguring --no-build


### Test DB Setup
docker run \
    --add-host host.docker.internal:host-gateway \
    --env PGPASSWORD=bbHRro5Ju2L9cw \
    -v ./temp/world.sql:/app/world.sql \
    -it postgres \
    psql -h host.docker.internal -U kayord kayord -f /app/world.sql
```