namespace OnlineDocumentStore.Application.DataTransferObjects.Auth
{
    public class JWTConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public string ExpireInMinutes { get; set; }
    }
}
