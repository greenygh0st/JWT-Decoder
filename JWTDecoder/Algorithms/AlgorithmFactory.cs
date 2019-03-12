using System;
namespace JWTDecoder.Algorithms
{
    public class AlgorithmFactory : IAlgorithmFactory
    {
        /// <inheritdoc />
        public IJwtAlgorithm Create(string algorithmName)
        {
            return Create((HashAlgorithm)Enum.Parse(typeof(HashAlgorithm), algorithmName));
        }

        /// <inheritdoc />
        public virtual IJwtAlgorithm Create(HashAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case HashAlgorithm.HS256:
                    return new HMACSHA256Algorithm();
                case HashAlgorithm.HS384:
                    return new HMACSHA384Algorithm();
                case HashAlgorithm.HS512:
                    return new HMACSHA512Algorithm();
                default:
                    throw new NotSupportedException($"Algorithm {algorithm} is not supported.");
            }
        }
    }
}
