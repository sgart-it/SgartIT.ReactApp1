using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PnP.Core.Auth.Services.Builder.Configuration;
using PnP.Core.Services.Builder.Configuration;
using SgartIT.ReactApp1.Server.DTO.Repositories;
using SgartIT.ReactApp1.Server.Repositories.SPOnline.Settings;
using System.Security.Cryptography.X509Certificates;

namespace SgartIT.ReactApp1.Server.Repositories.SPOnline;

public static class Startup
{
    const string AUTH_CERTIFICATE = "x509certificate";
    const string APP_SETTINGS_KEY = "RepositoryDynamic:SPOnline";

    public static void Init(IHostApplicationBuilder builder)
    {
        SPOnlineSettings settings = builder.Configuration.GetSection(APP_SETTINGS_KEY).Get<SPOnlineSettings>() ?? throw new Exception($"Invalid {APP_SETTINGS_KEY}");

        // Add the PnP Core SDK services
        builder.Services.AddPnPCore(options =>
        {
            options.PnPContext.GraphFirst = true;
            //options.PnPContext.GraphCanUseBeta = true;
            //options.PnPContext.GraphAlwaysUseBeta = false;

            options.HttpRequests.UserAgent = settings.UserAgent;

            options.HttpRequests.MicrosoftGraph = new PnPCoreHttpRequestsGraphOptions
            {
                UseRetryAfterHeader = true,
                MaxRetries = 10,
                DelayInSeconds = 3,
                UseIncrementalDelay = true,
            };

            //options.HttpRequests.SharePointRest = new PnPCoreHttpRequestsSharePointRestOptions
            //{
            //    UseRetryAfterHeader = true,
            //    MaxRetries = 10,
            //    DelayInSeconds = 3,
            //    UseIncrementalDelay = true,
            //};

            //options.DefaultAuthenticationProvider = authenticationProvider;

            options.Sites.Add(Constants.KEY_SITE, new PnPCoreSiteOptions
            {
                SiteUrl = settings.SiteUrl
            });
        });

        // PnP Core Authentication
        // To check out more authentication options check out the documentation for more information:
        //  https://pnp.github.io/pnpcore/using-the-sdk/configuring%20authentication.html
        builder.Services.AddPnPCoreAuthentication(options =>
        {
            options.Credentials.Configurations.Add(AUTH_CERTIFICATE,
                new PnPCoreAuthenticationCredentialConfigurationOptions
                {
                    ClientId = settings.ClientId,
                    TenantId = settings.TenantId,
                    X509Certificate = new PnPCoreAuthenticationX509CertificateOptions
                    {
                        //StoreName = StoreName.My,
                        //StoreLocation = StoreLocation.CurrentUser,
                        //Thumbprint = "{certificate_thumbprint}",
                        Certificate = new X509Certificate2(settings.CertificatePath, settings.CertificatePassword)
                    }
                });

            // Configure the default authentication provider
            options.Credentials.DefaultConfiguration = AUTH_CERTIFICATE;

            // Map the site defined in AddPnPCore with the 
            // Authentication Provider configured in this action
            options.Sites.Add(Constants.KEY_SITE, new PnPCoreAuthenticationSiteOptions
            {
                AuthenticationProviderName = AUTH_CERTIFICATE
            });
        });

        builder.Services.AddSingleton(settings);

        builder.Services.AddScoped<ITodoRepository, SPOnlineTodoRepository>();
    }
}
