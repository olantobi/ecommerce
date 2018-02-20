using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using IdentityServer3.AccessTokenValidation;

[assembly: OwinStartup(typeof(ecommerce.Api.Startup))]

namespace ecommerce.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://sso.test.vggdev.com/identity/",
                ClientId = "ebteller",
                ClientSecret = "secret2017",
                RequiredScopes = new[] { "openid", "profile", "roles", "ebteller" }
            });

            ConfigureAuth(app);
        }
    }
}
