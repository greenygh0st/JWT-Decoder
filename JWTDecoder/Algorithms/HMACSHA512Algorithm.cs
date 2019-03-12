using System;
using System.Security.Cryptography;

namespace JWTDecoder.Algorithms
{
    /// <summary>
    /// HMAC using SHA-512
    /// </summary>
    public sealed class HMACSHA512Algorithm : IJwtAlgorithm
    {
        /// <inheritdoc />
        public byte[] Sign(byte[] key, byte[] bytesToSign)
        {
            using (var sha = new HMACSHA512(key))
            {
                return sha.ComputeHash(bytesToSign);
            }
        }

        /// <inheritdoc />
        public string Name => HashAlgorithm.HS512.ToString();

        /// <inheritdoc />
        public bool IsAsymmetric { get; } = false;
    }
}
