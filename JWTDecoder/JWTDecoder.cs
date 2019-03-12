using System;
using Newtonsoft.Json;
using JWTDecoder.Helpers;
using JWTDecoder.Algorithms;

namespace JWTDecoder
{
    /// <summary>
    /// Sometimes all you need is a simple decoder.
    /// </summary>
    public static class JWTDecoder
    {
        /// <summary>
        /// Decode the specified token.
        /// </summary>
        /// <returns>A tupal contained the decoded Header and Payload.</returns>
        /// <param name="token">The token you with to decode.</param>
        public static (JwtHeader Header, string Payload, string Verification) DecodeToken(string token)
        {
            string[] split = token.Split('.');
            if (split.Length > 1)
            {
                JwtHeader jsonHeaderData = JsonConvert.DeserializeObject<JwtHeader>(Base64DecodeToString(split[0]));

                string jsonData = Base64DecodeToString(split[1]);

                //byte[] verficationBytes = EncodingHelper.GetBytes(Base64DecodeToString(split[2]));
                string verification = split[2];

                return (jsonHeaderData, jsonData, verification);
            }
            else
            {
                throw new InvalidTokenPartsException("token");
            }
        }

        /// <summary>
        /// Decodes the payload into the provided type.
        /// </summary>
        /// <returns>The payload.</returns>
        /// <param name="token">A properly formatted .</param>
        /// <typeparam name="T">The type you wish to decode into.</typeparam>
        public static T DecodePayload<T>(string token)
        {
            var payloadDecoded = JsonConvert.DeserializeObject<T>(DecodeToken(token).Payload);
            return payloadDecoded;
        }

        private static string Base64DecodeToString(string ToDecode)
        {
            string decodePrepped = ToDecode.Replace("-", "+").Replace("_", "/");

            switch (decodePrepped.Length % 4)
            {
                case 0:
                    break;
                case 2:
                    decodePrepped += "==";
                    break;
                case 3:
                    decodePrepped += "=";
                    break;
                default:
                    throw new Exception("Not a legal base64 string!");
            }

            byte[] data = Convert.FromBase64String(decodePrepped);
            return System.Text.Encoding.UTF8.GetString(data);
        }


        /// <summary>
        /// In case for some some crazed reason you want a client to validate the specified token rather than just letting the creator be authoritative.
        /// </summary>
        /// <returns>Validated?</returns>
        /// <param name="token">The token to be validated.</param>
        public static bool Validate(string token, string secret = null)
        {
            byte[] secretBytes = EncodingHelper.GetBytes(secret);
            AlgorithmFactory algorithmFactory = new AlgorithmFactory();

            var tokenDecoded = DecodeToken(token);

            bool secretValid = string.IsNullOrEmpty(secret);
            bool expirationValid = JsonConvert.DeserializeObject<JwtExpiration>(tokenDecoded.Payload).Expiration == null;

            // actually check the secret
            if (secret != null)
            {
                var alg = algorithmFactory.Create(tokenDecoded.Header.Algorithm);

                var bytesToSign = EncodingHelper.GetBytes(String.Concat(JsonConvert.SerializeObject(tokenDecoded.Header), ".", tokenDecoded.Payload));

                var testSignature = alg.Sign(secretBytes, bytesToSign);
                var decodedTestSignature = Convert.ToBase64String(testSignature);

                secretValid = decodedTestSignature == tokenDecoded.Verification;
            }

            //actually check the expiration
            var expiration = JsonConvert.DeserializeObject<JwtExpiration>(tokenDecoded.Payload).Expiration;
            if (expiration != null)
            {
                expirationValid = DateTimeHelpers.FromUnixTime((long)expiration) < DateTime.Now;
            }

            return secretValid && expirationValid;

        }
    }

    /// <summary>
    /// Exception thrown when when a token does not consist of three parts delimited by dots (".").
    /// </summary>
    public class InvalidTokenPartsException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Creates an instance of <see cref="InvalidTokenPartsException" />
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception</param>
        public InvalidTokenPartsException(string paramName)
            : base(paramName, "Token must consist of 3 delimited by dot parts.")
        {
        }
    }
}
