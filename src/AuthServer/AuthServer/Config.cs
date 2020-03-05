using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthServer
{
    public class Config
    {
        private const string spaClientUrl = "https://localhost:5200";
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("DSiAccountLoginAPI", "DSi AccountLogin API")
                {
                    Scopes = {new Scope("api.read")}
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client {
                    RequireConsent = false,
                    ClientId = "angular_spa_implicit",
                    ClientName = "Angular SPA",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email", "api.read"},
                    RedirectUris = {"http://localhost:4200/auth-callback"},
                    PostLogoutRedirectUris = {"http://localhost:4200/"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                  },
                new Client {
                    RequireConsent = false,
                    ClientId = "spaCodeClient",
                    ClientName = "SPA Code Client",
                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = 330,// 330 seconds, default 60 minutes
                    IdentityTokenLifetime = 30,

                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:5200/callback",
                        "http://localhost:5200/silent-renew.html"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:5200/unauthorized",
                        "http://localhost:5200"
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:5200"
                    },
                    AllowedScopes = { "openid", "profile", "email", "api.read"}
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
         {
             new TestUser
             {
                 SubjectId = "1",
                 Username = "james",
                 Password = "password",
                 Claims = new List<Claim>
                 {
                     new Claim("name", "James Bond"),
                     new Claim("website", "https://james.com")
                 }
             }
         };
        }
    }
}
