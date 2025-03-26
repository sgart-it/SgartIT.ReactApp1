namespace SgartIT.ReactApp1.Server.Repositories.SPOnline.Settings;

public class SPOnlineSettings
{
    /// <summary>
    /// es.: https://contoso.sharepoint.com/sites/demo
    /// </summary>
    public string SiteUrl { get; set; } = string.Empty;

    /// <summary>
    /// es.: ReactApp1
    /// </summary>
    public string ListName { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string CertificatePath { get; set; } = string.Empty;
    public string CertificatePassword { get; set; } = string.Empty;

    /// <summary>
    /// es.: NONISV|Sgart|ReactApp1
    /// </summary>
    public string UserAgent { get; set; } = string.Empty;
}