# SgartIT.ReactApp1.Server.Repositories.Sqlite

Usa il paccketto Nuget Microsoft.EntityFrameworkCore.Sqlite

## Database

Va adeguata la connection string in **SgartIT.ReactApp1.Server/appsettings.json**

```json
{
  "RepositoryDynamic": {
    "AssemblyName": "SgartIT.ReactApp1.Server.Repositories.Sqlite",

    "SqliteConnectionString": "Data Source=|DataDirectory|ReactApp1.db",


```

Per creare un DB da zero pulito mettersi in debug su un metodo, es. FindAsync
e eseguire dbContext.Database.EnsureCreated();
