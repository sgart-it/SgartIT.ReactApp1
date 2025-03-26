# SgartIT.ReactApp1.Server.Repositories.SPOnline

Usa il paccketto Nuget PnP.Core.Auth

## SharePoint Online


Per funzionare va creata una lista **ReactApp1** con i seguenti campi (nomi interni):
- Title (text) 
- Completed (boolean)
- Category (text)

 
e poi adeguata la connection string in **SgartIT.ReactApp1.Server/appsettings.json**
```json
{
  "RepositoryDynamic": {
    "AssemblyName": "SgartIT.ReactApp1.Server.Repositories.SPOnline",

    "SPOnline": {
        "SiteUrl": "https://tenantName.sharepoint.com",
        "ListName": "ReactApp1",

        "TenantiD": "tenantId",
        "ClientId": "clientId",
        "CertificatePath": "c:\\xxx\PfxStore\\certificate.pfx",
        "CertificatePassword": "password",
        "UserAgent": "NONISV|Sgart.it|ReactApp1"
    }
```

## App Registration

Aggiungere l'app registration tramite lo script: **scripts/SPOnlineAppRegistration.ps1**

aggiungere all'app la permission Graph / Application / Sites.FullControl.All

**ATTENZIONE: per questioni di sicurezza, in ambiente di produzione è meglio usare la permission Sites.Selected, e selezionare solo il sito di interesse**
