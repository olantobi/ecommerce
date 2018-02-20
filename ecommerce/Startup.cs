using IdentityModel;
using IdentityServer3.AccessTokenValidation;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Web.Helpers;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(ecommerce.Startup))]
namespace ecommerce
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            //get or set the claim type from the identity used to uniquely identify the user
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = "unique_user_key";

            AntiForgeryConfig.UniqueClaimTypeIdentifier = JwtClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://sso.test.vggdev.com/identity/",

                ClientId = "ebteller",                
                //ClientSecret = "adetoberu",
                Scope = "openid profile roles",
                RedirectUri = "http://localhost:54803/",
                ResponseType = "code id_token token",
                UseTokenLifetime = true,
                SignInAsAuthenticationType = "Cookies",

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        // we want to keep first name, last name, subject and roles
                        var givenName = id.FindFirst(Constants.ClaimTypes.GivenName);
                        var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName);
                        var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                        var roles = id.FindAll(Constants.ClaimTypes.Role);

                        // create new identity and set name and role claim type
                        var nid = new ClaimsIdentity(
                            id.AuthenticationType,
                            Constants.ClaimTypes.GivenName,
                            Constants.ClaimTypes.Role);

                        nid.AddClaim(givenName);
                        nid.AddClaim(familyName);
                        nid.AddClaim(sub);
                        nid.AddClaims(roles);

                        // add some other app specific claim
                        nid.AddClaim(new Claim("app_specific", "some data"));

                        n.AuthenticationTicket = new AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    }
                }
                //Notifications = new OpenIdConnectAuthenticationNotifications
                //{
                //    SecurityTokenValidated = async n =>
                //    {
                //        var userInfo = await TokenHelper.CallUserInfoEndpoint(n.ProtocolMessage.AccessToken);
                //        var givenNameClaim = new Claim(JwtClaimTypes.GivenName, userInfo.Value<string>(JwtClaimTypes.GivenName));
                //        var familyNameClaim = new Claim(JwtClaimTypes.FamilyName, userInfo.Value<string>(JwtClaimTypes.FamilyName));
                //        var emailClaim = new Claim(JwtClaimTypes.Email, userInfo.Value<string>(JwtClaimTypes.Email));
                //        var phoneClaim = new Claim(JwtClaimTypes.PhoneNumber, userInfo.Value<string>(JwtClaimTypes.PhoneNumber));
                //        var roles = new List<JToken>();
                //        var permissions = new List<JToken>();

                //        //get roles, get permissions
                //        try
                //        {
                //            var singlerole = userInfo.Value<string>("client-id.role");
                //            roles.Add(singlerole);
                //        }
                //        catch (Exception ex)
                //        {
                //            //not single role
                //        }

                //        try
                //        {
                //            var rolesList = userInfo.Value<JArray>("client0id.role").ToList();
                //            roles.AddRange(rolesList);
                //        }
                //        catch (Exception)
                //        {
                //        }
            });        

            ConfigureAuth(app);
        }
    }
}
