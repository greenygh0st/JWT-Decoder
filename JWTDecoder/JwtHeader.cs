using System;
using Newtonsoft.Json;

namespace JWTDecoder
{
    public class JwtHeader
    {
        [JsonProperty("alg")]
        public string Algorithm { get; set; }
        [JsonProperty("typ")]
        public string Type { get; set; }
    }

    public class JwtExpiration
    {
        [JsonProperty("exp")]
        public double? Expiration { get; set; }
    }
}
