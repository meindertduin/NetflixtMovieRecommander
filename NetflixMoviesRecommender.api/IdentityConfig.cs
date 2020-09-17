using System.Collections;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace NetflixMoviesRecommender.api
{
    public static class IdentityConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), 
                new IdentityResources.Profile(), 
                new IdentityResource(name: ApiConstants.Claims.Role, userClaims: new []{ ApiConstants.Claims.Role }, displayName: "role"),
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            
            return new List<ApiScope>
            {
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName, new[]
                {
                    JwtClaimTypes.PreferredUserName,
                    ApiConstants.Claims.Role,
                })
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = "web-client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    
                    RedirectUris =
                    {
                        "https://localhost:3000/oidc/sign-in-callback.html",
                        //"https://localhost:3000/auth/silent-sign-in-callback",
                        
                    },

                    PostLogoutRedirectUris = new[] {"https://localhost:3000"},
                    AllowedCorsOrigins = new [] { "https://localhost:3000" },
                    
                    AllowedScopes = new []
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.LocalApi.ScopeName,
                        ApiConstants.Claims.Role,
                    },
                    
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                }
            };
        } 
    }
}