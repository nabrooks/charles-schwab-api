using System.Text.Json;
using System.Text.Json.Serialization;
using NbTrader.Brokers.TDAmeritrade.Models;

namespace NbTrader.Brokers.TDAmeritrade
{
    public class TDToken : IEquatable<TDToken>, IEqualityComparer<TDToken>
    {
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

        public TDToken()
        {
        }

        public TDToken(string json)
        {
            using (JsonDocument jsonDoc = JsonDocument.Parse(json))
            {
                var now = DateTime.Now;
                var jRoot = jsonDoc.RootElement;

                AccessToken = jRoot.GetProperty("access_token").GetString();
                RefreshToken = jRoot.GetProperty("refresh_token").GetString();
                Scope = jRoot.GetProperty("scope").GetString();
                Expiration = now + TimeSpan.FromSeconds(jRoot.GetProperty("expires_in").GetDouble());
                RefreshTokenExpiration = now + TimeSpan.FromSeconds(jRoot.GetProperty("refresh_token_expires_in").GetDouble());
                TokenType = jRoot.GetProperty("token_type").GetString();
            }
        }
        
        /// <summary>
        /// Evaluates whether the <see cref="TDAuthResult"/> instance needs refreshing
        /// </summary>
        /// <returns>true if in need of a refresh, otherwise false</returns>
        public bool NeedsRefresh
        {
            get
            {
                if (String.IsNullOrWhiteSpace(AccessToken) || Expiration == null || Expiration < DateTime.Now ||
                    String.IsNullOrWhiteSpace(RefreshToken) || RefreshTokenExpiration == null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Evaluates if a refresh can even be performed given current
        /// object state and its property values
        /// </summary>
        /// <returns>True if able to refresh, otherwise False</returns>
        public bool CanRefresh
        {
            get
            {
                if (String.IsNullOrWhiteSpace(RefreshToken) ||
                    RefreshTokenExpiration == null ||
                    RefreshTokenExpiration < DateTime.Now - TimeSpan.FromMinutes(3))
                    return false;
                else return true;
            }
        }

        /// <summary>
        /// Evaluates if the refresh token is expired and if any necessary properties are null
        /// </summary>
        /// <returns>true if valid, otherwise false</returns>
        public bool IsValid
        {
            get
            {
                if (String.IsNullOrWhiteSpace(AccessToken) || String.IsNullOrWhiteSpace(RefreshToken) ||
                    Expiration == null || RefreshTokenExpiration == null ||
                    RefreshTokenExpiration < DateTime.Now - TimeSpan.FromMinutes(3))
                    return false;
                else
                    return true;
            }
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public bool Equals(TDToken? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return SecurityCode == other.SecurityCode &&
                   AccessToken == other.AccessToken &&
                   RefreshToken == other.RefreshToken &&
                   Scope == other.Scope &&
                   Expiration == other.Expiration &&
                   RefreshTokenExpiration == other.RefreshTokenExpiration &&
                   TokenType == other.TokenType;
        }

        public bool Equals(TDToken? x, TDToken? y)
        {
            return x?.Equals(y) ?? false;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals(obj as TDToken);
        }

        public override int GetHashCode() => HashCode.Combine(SecurityCode, AccessToken, RefreshToken, Scope, Expiration, RefreshTokenExpiration, TokenType);

        public int GetHashCode(TDToken obj) => obj.GetHashCode();

        public static bool operator ==(TDToken? left, TDToken? right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
            if (ReferenceEquals(null, left) && !ReferenceEquals(null, right)) return true;
            if (!ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;

            return left!.Equals(right);
        }

        public static bool operator !=(TDToken? left, TDToken? right)
        {
            return !(left == right);
        }
    }
}