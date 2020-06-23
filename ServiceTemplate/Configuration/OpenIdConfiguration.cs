namespace ServiceTemplate.Configuration
{
    public class OpenIdConfiguration : IOpenIdConfiguration
    {
        public string IdentityAdminRedirectUri { get; set; }

        public string IdentityServerBaseUrl { get; set; }

        public string IdentityAdminBaseUrl { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string OidcResponseType { get; set; }

        public string[] Scopes { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}