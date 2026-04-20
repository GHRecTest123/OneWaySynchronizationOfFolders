namespace OneWaySynchronizationOfFolders
{
    public class LogEntry
    {
        public string pathToLoggingFile;
        public string pathToOriginalFile;
        public string pathToDestinationFile;
        public string actionPerformed;
        public string timestamp;
        public string message;

        /// <summary>
        /// Initiates an object of a log entry.
        /// </summary>
        /// <param name="pathToLoggingFile">Path that points to the file which is used for logging conducted actions on files.</param>
        /// <param name="pathToOriginalFile">Path that points to the file from original folder.</param>
        /// <param name="pathToDestinationFile">Path that points to the file from destination folder.</param>
        public LogEntry(string pathToLoggingFile, string pathToOriginalFile = null, string pathToDestinationFile = null)
        {
            this.pathToLoggingFile = pathToLoggingFile;
            this.pathToOriginalFile = pathToOriginalFile;
            this.pathToDestinationFile = pathToDestinationFile;
        }

        /// <summary>
        /// Sets <c>timestamp</c> to a current value of time.
        /// </summary>
        public void SetCurrentTimestamp()
        {
            this.timestamp = DateTime.Now.ToString();
        }

        /// <summary>
        /// Prints the log in the console.
        /// </summary>
        public void DisplayLogEntry()
        {
            Console.WriteLine(this.message);
        }

        /// <summary>
        /// Deletes the content of the message in the log.
        /// </summary>
        public void ResetMessage()
        {
            this.message = "";
        }

        /// <summary>
        /// Saves the log into a text file. If the file already exists, it gets overwritten with new content. However, if it does not exist, it gets created with a message.
        /// </summary>
        public void SaveLogEntry()
        {
            if (pathToLoggingFile != null)
            {
                File.AppendAllText(pathToLoggingFile, message);
            }
            else
            {
                File.WriteAllText(pathToLoggingFile, message);
            }
        }

        /// <summary>
        /// Creates a log entry.
        /// </summary>
        /// <param name="actionPerformed">What action is performed on the file.</param>
        /// <param name="pathToOriginalFile">Path that points to the file from original folder.</param>
        /// <param name="pathToDestinationFile">Path that points to the file from destination folder.</param>
        public void CreateALog(string actionPerformed, string pathToOriginalFile = null, string pathToDestinationFile = null)
        {
            SetCurrentTimestamp();

            if (actionPerformed == "DELETE")
            {
                this.message += "[" + timestamp + "]\n" +
                    String.Format("Action \'{0}\' was performed on {1}.\n\n", actionPerformed, pathToDestinationFile);
            }
            else if (actionPerformed == "OVERWRITE")
            {
                this.message += "[" + timestamp + "]\n" +
                    String.Format("Action \'{0}\' was performed on {1} with {2}.\n\n", actionPerformed, pathToDestinationFile, pathToOriginalFile);
            }
            else if (actionPerformed == "CREATE")
            {
                this.message += "[" + timestamp + "]\n" +
                    String.Format("Action \'{0}\' was performed on {1} based on {2}.\n\n", actionPerformed, pathToDestinationFile, pathToOriginalFile);
            }

            DisplayLogEntry();
            SaveLogEntry();

            ResetMessage();
        }

        /// <summary>
        /// Informs about the start of the synchronization process in the logs.
        /// </summary>
        public void Start()
        {
            SetCurrentTimestamp();

            this.message += "[" + timestamp + "]\n" +
                "--------------- The synchronization process has just started. ---------------\n\n";

            DisplayLogEntry();
            SaveLogEntry();

            ResetMessage();
        }

        /// <summary>
        /// Informs about the end of the synchronization process in the logs.
        /// </summary>
        public void End()
        {
            SetCurrentTimestamp();

            this.message += "[" + timestamp + "]\n" +
                "--------------- The synchronization process has just ended. ---------------\n\n";

            DisplayLogEntry();
            SaveLogEntry();

            ResetMessage();
        }
    }
}
