using System;
namespace JWTDecoder
{
    public enum HashAlgorithm
    {
        /// <summary>
        /// HMAC using SHA-256
        /// </summary>
        HS256,
        /// <summary>
        /// HMAC using SHA-384
        /// </summary>
        HS384,
        /// <summary>
        /// HMAC using SHA-512
        /// </summary>
        HS512
    }
}
