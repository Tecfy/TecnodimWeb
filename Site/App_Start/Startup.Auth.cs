using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Model;
using Owin;
using Repository;
using Site.Providers;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Configuration;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.Owin;
using Sustainsys.Saml2.WebSso;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Configuration;
using System.Web.Hosting;

namespace Site
{
    public partial class Startup
    {
        #region Public /Protected Properties.

        /// <summary>
        /// OAUTH options property.
        /// </summary>
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        /// <summary>
        /// Public client ID property.
        /// </summary>
        public static string PublicClientId { get; private set; }

        #endregion

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/LogOff"),
                ExpireTimeSpan = TimeSpan.FromHours(12),
            });

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                AllowInsecureHttp = true //Don't do this in production ONLY FOR DEVELOPING: ALLOW INSECURE HTTP!
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseSaml2Authentication(CreateSaml2Options());
        }

        private static Saml2AuthenticationOptions CreateSaml2Options()
        {
            var spOptions = CreateSPOptions();
            var Saml2Options = new Saml2AuthenticationOptions(false)
            {
                SPOptions = spOptions
            };

            //Alterei Com Informações do ADFS
            var idp = new IdentityProvider(new EntityId(WebConfigurationManager.AppSettings["ADFS.Trust"]), spOptions)
            {
                AllowUnsolicitedAuthnResponse = true,
                Binding = Saml2BindingType.HttpRedirect,
                SingleSignOnServiceUrl = new Uri(WebConfigurationManager.AppSettings["ADFS.LS"]),
                SingleLogoutServiceUrl = new Uri(WebConfigurationManager.AppSettings["ADFS.Logout"]),
                MetadataLocation = WebConfigurationManager.AppSettings["ADFS.FederationMetadata"],
                LoadMetadata = true
            };

            //Alterei Com Informações do ADFS
            idp.SigningKeys.AddConfiguredKey(new X509Certificate2(HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["ADFS.Path.SigningKey"]), "", X509KeyStorageFlags.MachineKeySet));

            Saml2Options.IdentityProviders.Add(idp);

            // It's enough to just create the federation and associate it
            // with the options. The federation will load the metadata and
            // update the options with any identity providers found.
            new Federation(WebConfigurationManager.AppSettings["ADFS.FederationMetadata"], true, Saml2Options);

            return Saml2Options;
        }

        private static SPOptions CreateSPOptions()
        {
            var portuguese = "pt-br";

            var organization = new Organization();
            organization.Names.Add(new LocalizedName(WebConfigurationManager.AppSettings["ADFS.Organization.Name"], portuguese));
            organization.DisplayNames.Add(new LocalizedName(WebConfigurationManager.AppSettings["ADFS.Organization.Name"], portuguese));
            organization.Urls.Add(new LocalizedUri(new Uri(WebConfigurationManager.AppSettings["ADFS.Organization.Url"]), portuguese));

            //Alterei Com Informações do Projeto
            var spOptions = new SPOptions
            {
                EntityId = new EntityId(WebConfigurationManager.AppSettings["ADFS.EntityId"]),
                ReturnUrl = new Uri(WebConfigurationManager.AppSettings["ADFS.ReturnUrl"]),
                //DiscoveryServiceUrl = new Uri("http://localhost:52071/DiscoveryService"),
                ModulePath = WebConfigurationManager.AppSettings["ADFS.Path.Module"],
                Organization = organization
            };

            var techContact = new ContactPerson
            {
                Type = ContactType.Technical
            };
            techContact.EmailAddresses.Add(WebConfigurationManager.AppSettings["ADFS.Organization.Email.Technical"]);
            spOptions.Contacts.Add(techContact);

            var supportContact = new ContactPerson
            {
                Type = ContactType.Support
            };
            supportContact.EmailAddresses.Add(WebConfigurationManager.AppSettings["ADFS.Organization.Email.Support"]);
            spOptions.Contacts.Add(supportContact);

            var attributeConsumingService = new AttributeConsumingService
            {
                IsDefault = true,
                ServiceNames = { new LocalizedName("Saml2", "en") }
            };

            attributeConsumingService.RequestedAttributes.Add(
                new RequestedAttribute("urn:someName")
                {
                    FriendlyName = "Some Name",
                    IsRequired = true,
                    NameFormat = RequestedAttribute.AttributeNameFormatUri
                });

            attributeConsumingService.RequestedAttributes.Add(new RequestedAttribute("Minimal"));

            spOptions.AttributeConsumingServices.Add(attributeConsumingService);

            //Alterei Com Informações do Projeto            
            spOptions.ServiceCertificates.Add(new X509Certificate2(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + WebConfigurationManager.AppSettings["ADFS.Path.ServiceCertificate"], "", X509KeyStorageFlags.MachineKeySet));

            return spOptions;
        }

    }
}