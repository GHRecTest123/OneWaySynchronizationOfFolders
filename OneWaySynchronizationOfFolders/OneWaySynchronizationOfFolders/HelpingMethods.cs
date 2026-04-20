using System.Security.Cryptography;

namespace OneWaySynchronizationOfFolders
{
    public static class HelpingMethods
    {
        /// <summary>
        /// Extracts names of files that exist within the folder of provided path.
        /// </summary>
        /// <param name="path">Path to the folder.</param>
        /// <returns>An array of names of files, including files' extensions.</returns>
        public static string[] GetNamesOfFiles(string path)
        {
            var filesInPath = Directory.GetFiles(path);
            var namesOfFilesInPath = new string[filesInPath.Count()];

            for (int i = 0; i < filesInPath.Length; i++)
            {
                var file = new FileInfo(filesInPath[i]);
                namesOfFilesInPath[i] = file.Name;
            }

            return namesOfFilesInPath;
        }

        /// <summary>
        /// Computes the hash value using SHA-256 algorithm (as a byte[] array) for a file of provided directory.
        /// </summary>
        /// <param name="directoryOfFile">Path to the file.</param>
        /// <returns>An array consisting of bytes of calculated hash value.</returns>
        public static byte[] GetTheSHA256Hash(string directoryOfFile)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(directoryOfFile))
                {
                    var hash = sha256.ComputeHash(stream);
                    return hash;
                }
            }
        }

        /// <summary>
        /// Checks if two files are the same. Verification is based on comparing hash values already calculated for these files.
        /// </summary>
        /// <param name="hashOfOriginalFile">Hash value of the first file (from original folder).</param>
        /// <param name="hashOfDestinationFile">Hash value of the second file (from destination folder).</param>
        /// <returns><c>True</c> if hash values are equal. <c>False</c> if hash values are not equal.</returns>
        public static bool AreFilesTheSame(byte[] hashOfOriginalFile, byte[] hashOfDestinationFile)
        {
            bool areTheSame = false;
            bool[] resultsOfByteComparison = new bool[hashOfOriginalFile.Length];

            for (int i = 0; i < hashOfOriginalFile.Length; i++)
            {
                if (hashOfOriginalFile[i] != hashOfDestinationFile[i])
                {
                    resultsOfByteComparison[i] = false;
                }
                else
                {
                    resultsOfByteComparison[i] = true;
                }
            }

            if (!resultsOfByteComparison.Contains(false))
            {
                areTheSame = true;
            }

            return areTheSame;
        }
    }
}
