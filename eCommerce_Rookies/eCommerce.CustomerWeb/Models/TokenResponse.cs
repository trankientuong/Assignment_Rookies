namespace eCommerce.CustomerWeb.Models
{
    public class TokenResponse
    {
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }

    }
}
