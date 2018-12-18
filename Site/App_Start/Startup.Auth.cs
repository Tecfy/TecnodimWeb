using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.WsFederation;
using Model;
using Owin;
using Repository;
using Site.Providers;
using System;
using System.Configuration;

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

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        private static string realm = ConfigurationManager.AppSettings["ida:Wtrealm"];
        private static string adfsMetadata = ConfigurationManager.AppSettings["ida:ADFSMetadata"];
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
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    LogoutPath = new PathString("/Account/LogOff"),
            //    ExpireTimeSpan = TimeSpan.FromHours(12),
            //});
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Api/AccountAd/SignIn"),
                LogoutPath = new PathString("/Api/AccountAd/SignOut"),
                ExpireTimeSpan = TimeSpan.FromHours(12),
            });
           


            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    Wtrealm = realm,
                    MetadataAddress = adfsMetadata
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

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }    
}