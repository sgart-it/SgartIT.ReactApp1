# create-certificate.ps1
# genera un certificato self-signed e lo esporta in formato .cer e .pfx 
# con una password generata casualmente
# se $removeCertificate è true, rimuove il certificato dallo store
# se $removeCertificate è false, stampa il thumbprint del certificato
# il certificato viene creato con scadenza di X anni
# la password generata viene salvata in un file .txt
# Install-Module Microsoft.Graph -Scope CurrentUser -Repository PSGallery -Force

$appName = "SgartSPNet8ReactApp1"
$exportPath = "C:\temp"
$removeCertificate = $false
$expireDate = (Get-Date).AddYears(2)

$dateFile = (Get-Date).ToString("yyyyMMddHHmmss")
$certname = "certificate-$appName-$dateFile"

Import-Module Microsoft.Graph.Applications

function Generate-Password {
    param (
        [Parameter(Mandatory)]
        [int] $length,
        [int] $amountOfNonAlphanumeric = 1
    )

    Add-Type -AssemblyName 'System.Web'

    return [System.Web.Security.Membership]::GeneratePassword($length, $amountOfNonAlphanumeric)
}
Write-Output "AppName: $appName"

Connect-MgGraph -Scopes "Application.Read.All","Application.ReadWrite.All","User.Read.All"

$tenantId = (Get-MgOrganization).Id

$app = New-MgApplication -DisplayName $appName

Write-Output "Id: $($app.Id)"
Write-Output "AppId / ClientId: $($app.AppId)"
Write-Output "SignInAudience: $($app.SignInAudience)"
Write-Output "PublisherDomain: $($app.PublisherDomain)"
Write-Output "TenantId: $tenantId"


 Format-List Id, DisplayName, AppId, SignInAudience, PublisherDomain


Write-Output "Certname: $certname"
Write-Output "Export path: $exportPath"

$pwd = Generate-Password -length 20  

$pwd | Out-File -FilePath "$exportPath\$certname.txt"
write-output "Password generated"

$cert = New-SelfSignedCertificate -Subject "CN=$certname" -CertStoreLocation "Cert:\CurrentUser\My" -KeyExportPolicy Exportable -KeySpec Signature -KeyLength 2048 -KeyAlgorithm RSA -HashAlgorithm SHA256 -NotAfter $expireDate

write-output "Certificate created"

Export-Certificate -Cert $cert -FilePath "$exportPath\$certname.cer" 

write-output "Exported .cer"

$mySecurePwd = ConvertTo-SecureString -String $pwd -Force -AsPlainText  

Export-PfxCertificate -Cert $cert -FilePath "$exportPath\$certname.pfx" -Password $mySecurePwd

write-output "Exported .pfx"

$newCert = Get-ChildItem -Path "Cert:\CurrentUser\My" | Where-Object {$_.Subject -Match $certname}

if( $true -eq $removeCertificate ){
    Remove-Item -Path "Cert:\CurrentUser\My\$($newCert.Thumbprint)" -DeleteKey

    write-output "Certificate removed"
} else{
    write-output "Thumbprint: $($newCert.Thumbprint)"
}


write-output "Upload .cer: $($newCert.Thumbprint)"
$keyCreds = @{
    Type  = "AsymmetricX509Cert";
    Usage = "Verify";
    key   = $cert.RawData
}
Update-MgApplication -ApplicationId $($app.Id) -KeyCredentials $keyCreds

Write-Output "-----------------------------------"
            
Write-Output "TenantId: $tenantId"
Write-Output "AppId / ClientId: $($app.AppId)"
Write-Output "Cer: $exportPath\$certname.cer"
Write-Output "Pfx: $exportPath\$certname.pfx"
Write-Output "Pwd: $exportPath\$certname.txt"

write-output "END"