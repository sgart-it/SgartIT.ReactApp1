# SgartIT.ReactApp1.Server.Repositories.MsSqlDapper

Usa il paccketto Nuget Microsoft.Data.SqlClient

## Database

Per funzionare va creato il database Microsoft SQL Server con lo script: **SgartIT.ReactApp1.Server.Repositories.MsSql/Scripts/ReactApp1.sql**

e poi adeguata la connection string in **SgartIT.ReactApp1.Server/appsettings.json**
```json
{
  "RepositoryDynamic": {
    "AssemblyName": "SgartIT.ReactApp1.Server.Repositories.MsSql",

    "MsSqlConnectionString": "Server=NomeServer;Database=ReactApp1;MultipleActiveResultSets=true;Trusted_Connection=True;Encrypt=true;TrustServerCertificate=true",

```

## Errori

Se compare:

**"La connessione con il server è stata stabilita correttamente, ma poi si è verificato un errore durante la procedura di login. (provider: Provider memoria condivisa, error: 0 - Nessun altro processo all'altra estremità della pipe.)"**

verificare il server MS SQL Server abbia abilitato il protocollo TCP.

Se compare:

**"La connessione con il server è stata stabilita correttamente, ma poi si è verificato un errore durante la procedura di login. (provider: Provider SSL, error: 0 - Catena di certificati emessa da una Autorità di certificazione non disponibile nell'elenco locale.)"**

verificare la catena dei certificati.

Se è in DEV si può aggiungere alla connection string il parametro: **TrustServerCertificate=true**
