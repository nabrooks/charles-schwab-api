using System.Text.Json.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.Models
{
    internal class TDAuthResult
    {
        [JsonPropertyName("redirect_url")]
        public string? RedirectUrl { get; set; }

        [JsonPropertyName("consumer_key")]
        public string? ConsumerKey { get; set; }

        [JsonPropertyName("security_code")]
        public string? SecurityCode { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime? Expiration { get; set; }

        [JsonPropertyName("refresh_token_expiration")]
        public DateTime? RefreshTokenExpiration { get; set; }
        
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
        
        /// <summary>
        /// Evaluates whether the <see cref="TDAuthResult"/> instance needs refreshing
        /// </summary>
        /// <returns>true if in need of a refresh, otherwise false</returns>
        public bool NeedsRefresh()
        {
            if (String.IsNullOrWhiteSpace(AccessToken) || Expiration == null || Expiration < DateTime.Now ||
                String.IsNullOrWhiteSpace(RefreshToken) || RefreshTokenExpiration == null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Evaluates if a refresh can even be performed given current
        /// object state and its property values
        /// </summary>
        /// <returns>True if able to refresh, otherwise False</returns>
        public bool CanRefresh()
        {
            if (String.IsNullOrWhiteSpace(RefreshToken) ||
                RefreshTokenExpiration == null ||
                RefreshTokenExpiration < DateTime.Now - TimeSpan.FromMinutes(3))
                return false;
            else return true;
        }

        /// <summary>
        /// Evaluates if the refresh token is expired and if any necessary properties are null
        /// </summary>
        /// <returns>true if valid, otherwise false</returns>
        public bool IsValid()
        { 
            if (String.IsNullOrWhiteSpace(AccessToken) || String.IsNullOrWhiteSpace(RefreshToken) ||
                Expiration == null || RefreshTokenExpiration == null ||
                RefreshTokenExpiration < DateTime.Now - TimeSpan.FromMinutes(3))
                return false;
            else
                return true;
        }
    }

    internal class TDAuthResultDto
    {
        [JsonPropertyName("redirect_url")]
        public string? RedirectUrl { get; set; }

        [JsonPropertyName("consumer_key")]
        public string? ConsumerKey { get; set; }

        [JsonPropertyName("security_code")]
        public string? SecurityCode { get; set; }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
    }
}
