using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.Services.Identity
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>()
            {
                new IdentityResources.OpenId(), //This initializes open id
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>()
            {
                //This allows users to access all operations
                new ApiScope(name: "mangoAdmin", displayName: "Mango Server"),
                //Customization for different accesses
                new ApiScope(name: "read", displayName: "Read your data"),
                new ApiScope(name: "write", displayName: "Write your data"),
                new ApiScope(name: "delete", displayName: "Delete your data"),
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>()
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"read", "write", "profile"} //Profile is a default scope in IS
                },
                new Client
                {
                    ClientId = "mango",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris={ "https://localhost:7287/signin-oidc" },
                    PostLogoutRedirectUris={"https://localhost:7287/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "mangoAdmin"
                    }
                }
            };
    }
}
