using static OneWaySynchronizationOfFolders.HelpingMethods;

namespace OneWaySynchronizationOfFolders
{
    public static class SynchronizationMethods
    {
        /// <summary>
        /// Creates copies of files from an original folder in a destination folder if those files do not exist in a destination folder.
        /// </summary>
        /// <param name="originalFolderPath">Path that points to the original folder.</param>
        /// <param name="destinationFolderPath">Path that points to the destination folder.</param>
        /// <param name="logEntry">Object used for tracking actions conducted on the files.</param>
        public static void CopyFiles(string originalFolderPath, string destinationFolderPath, LogEntry logEntry)
        {
            string[] namesOfFilesInOriginalPath = GetNamesOfFiles(originalFolderPath);
            string[] namesOfFilesInDestinationPath = GetNamesOfFiles(destinationFolderPath);

            for (int i = 0; i < namesOfFilesInOriginalPath.Length; i++)
            {
                bool[] doesFileExistInDestinationFolder = new bool[namesOfFilesInDestinationPath.Length];

                for (int j = 0; j < namesOfFilesInDestinationPath.Length; j++)
                {
                    if (namesOfFilesInOriginalPath[i] == namesOfFilesInDestinationPath[j])
                    {
                        doesFileExistInDestinationFolder[j] = true;
                    }
                    else if (namesOfFilesInOriginalPath[i] != namesOfFilesInDestinationPath[j])
                    {
                        doesFileExistInDestinationFolder[j] = false;
                    }
                }

                if (!doesFileExistInDestinationFolder.Contains(true))
                {
                    string pathToFileToCopy = originalFolderPath + String.Format("\\{0}", namesOfFilesInOriginalPath[i]);
                    string pathToFileInDestinationFolder = destinationFolderPath + String.Format("\\{0}", namesOfFilesInOriginalPath[i]);

                    File.Copy(pathToFileToCopy, pathToFileInDestinationFolder);

                    logEntry.CreateALog("CREATE", pathToFileToCopy, pathToFileInDestinationFolder);
                }

                Array.Clear(doesFileExistInDestinationFolder);
            }
        }

        /// <summary>
        /// Overwrites files in a destination folder if a file from an original folder and a file from a destination folder both have the same name, but their hash values are not equal.
        /// </summary>
        /// <param name="originalFolderPath">Path that points to the original folder.</param>
        /// <param name="destinationFolderPath">Path that points to the destination folder.</param>
        /// <param name="logEntry">Object used for tracking actions conducted on the files.</param>
        public static void OverwriteFiles(string originalFolderPath, string destinationFolderPath, LogEntry logEntry)
        {
            string[] namesOfFilesInOriginalPath = GetNamesOfFiles(originalFolderPath);
            string[] namesOfFilesInDestinationPath = GetNamesOfFiles(destinationFolderPath);

            for (int i = 0; i < namesOfFilesInOriginalPath.Length; i++)
            {
                for (int j = 0; j < namesOfFilesInDestinationPath.Length; j++)
                {
                    string pathToOriginalFile = originalFolderPath + String.Format("\\{0}", namesOfFilesInOriginalPath[i]);
                    string pathToDestinationFile = destinationFolderPath + String.Format("\\{0}", namesOfFilesInDestinationPath[j]);

                    if (namesOfFilesInOriginalPath[i] == namesOfFilesInDestinationPath[j])
                    {
                        if (AreFilesTheSame(GetTheSHA256Hash(pathToOriginalFile), GetTheSHA256Hash(pathToDestinationFile)) == true)
                        {
                            continue;
                        }
                        else if (AreFilesTheSame(GetTheSHA256Hash(pathToOriginalFile), GetTheSHA256Hash(pathToDestinationFile)) == false)
                        {
                            File.Copy(pathToOriginalFile, pathToDestinationFile, true);

                            logEntry.CreateALog("OVERWRITE", pathToOriginalFile, pathToDestinationFile);

                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes files from a destination folder if they cannot be found in an original folder.
        /// </summary>
        /// <param name="originalFolderPath">Path that points to the original folder.</param>
        /// <param name="destinationFolderPath">Path that points to the destination folder.</param>
        /// <param name="logEntry">Object used for tracking actions conducted on the files.</param>
        public static void DeleteFiles(string originalFolderPath, string destinationFolderPath, LogEntry logEntry)
        {
            string[] namesOfFilesInOriginalPath = GetNamesOfFiles(originalFolderPath);
            string[] namesOfFilesInDestinationPath = GetNamesOfFiles(destinationFolderPath);

            if (namesOfFilesInDestinationPath.Length >= 1)
            {
                for (int i = 0; i < namesOfFilesInDestinationPath.Length; i++)
                {
                    bool[] areFileNamesTheSame = new bool[namesOfFilesInOriginalPath.Length];

                    for (int j = 0; j < namesOfFilesInOriginalPath.Length; j++)
                    {
                        if (namesOfFilesInDestinationPath[i] != namesOfFilesInOriginalPath[j])
                        {
                            areFileNamesTheSame[j] = false;
                        }
                        else if (namesOfFilesInDestinationPath[i] == namesOfFilesInOriginalPath[j])
                        {
                            areFileNamesTheSame[j] = true;
                        }
                    }

                    if (!areFileNamesTheSame.Contains(true))
                    {
                        string pathToFileToDelete = destinationFolderPath + String.Format("\\{0}", namesOfFilesInDestinationPath[i]);

                        File.Delete(pathToFileToDelete);

                        logEntry.CreateALog("DELETE", null, pathToFileToDelete);
                    }

                    Array.Clear(areFileNamesTheSame);
                }
            }
        }

        /// <summary>
        /// Conducts a synchronization between two folders of provided paths (<c>pathToOriginalFolder</c> and <c>pathToDestinationFolder</c>) periodically (frequency is expressed with <c>synchronizationInterval</c>). Actions conducted on the files within these folders are logged in a folder under <c>pathToLoggingFolder</c>.
        /// </summary>
        /// <param name="pathToOriginalFolder">Path that points to the original folder.</param>
        /// <param name="pathToDestinationFolder">Path that points to the destination folder.</param>
        /// <param name="pathToLoggingFolder">Path that points to the folder where logs are kept.</param>
        /// <param name="synchronizationInterval">Frequency with which synchronization is conducted. Expressed in milliseconds.</param>
        /// <exception cref="PathsNotDifferentException"></exception>
        public static void Synchronize(string pathToOriginalFolder, string pathToDestinationFolder, string pathToLoggingFolder, int synchronizationInterval)
        {
            if ((pathToOriginalFolder == pathToDestinationFolder) || (pathToOriginalFolder == pathToLoggingFolder) || (pathToDestinationFolder == pathToLoggingFolder))
            {
                throw new PathsNotDifferentException();
            }
            else if (synchronizationInterval < 0)
            {
                throw new NegativeSynchronizationIntervalException();
            }
            else if ((pathToOriginalFolder != pathToDestinationFolder) && (pathToOriginalFolder != pathToLoggingFolder) && (pathToDestinationFolder != pathToLoggingFolder))
            {
                while (true)
                {
                    LogEntry logEntry = new LogEntry(pathToLoggingFolder + "\\LogEntry_" + DateTime.Now.Date.ToString("dd.MM.yyyy") + ".txt");

                    logEntry.Start();

                    DeleteFiles(pathToOriginalFolder, pathToDestinationFolder, logEntry);
                    OverwriteFiles(pathToOriginalFolder, pathToDestinationFolder, logEntry);
                    CopyFiles(pathToOriginalFolder, pathToDestinationFolder, logEntry);

                    logEntry.End();

                    Thread.Sleep(synchronizationInterval);
                }
            }
        }
    }
}
