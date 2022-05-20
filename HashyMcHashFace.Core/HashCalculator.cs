using System.Security.Cryptography;

namespace HashyMcHashFace.Core
{
    public enum HashingAlgorithm
    {
        SHA256,
        SHA384,
        SHA512,
        MD5,
        SHA1
    }

    public class HashCalculator
    {
        private readonly HashAlgorithm _hashAlgorithm;

        public HashCalculator(HashingAlgorithm hashingAlgorithm)
        {
            _hashAlgorithm = GetHashAlgorithm(hashingAlgorithm);
        }

        public string Calculate(FileInfo file)
        {
            using var stream = File.OpenRead(file.FullName);

            var hash = _hashAlgorithm.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "");
        }

        public ICollection<string> Calculate(IEnumerable<FileInfo> files)
        {
            var hashes = new List<string>(files.Count());

            foreach (var file in files)
            {
                hashes.Add(Calculate(file));
            }

            return hashes;
        }

        private HashAlgorithm GetHashAlgorithm(HashingAlgorithm hashingAlgorithm)
        {
            var algorithm = hashingAlgorithm switch
            {
                HashingAlgorithm.SHA256 => HashAlgorithm.Create("SHA256"),
                HashingAlgorithm.SHA384 => HashAlgorithm.Create("SHA384"),
                HashingAlgorithm.SHA512 => HashAlgorithm.Create("SHA512"),
                HashingAlgorithm.MD5 => HashAlgorithm.Create("MD5"),
                HashingAlgorithm.SHA1 => HashAlgorithm.Create("SHA1"),
                _ => throw new ArgumentOutOfRangeException(nameof(hashingAlgorithm),
                                                           "Cannot determine which hashing alorithm to use."),
            };

            ArgumentNullException.ThrowIfNull(algorithm);
            return algorithm;
        }
    }
}