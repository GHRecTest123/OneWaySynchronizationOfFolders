namespace OneWaySynchronizationOfFolders
{
    public class PathsNotDifferentException : Exception
    {
        /// <summary>
        /// Exception is thrown when at least two of provided paths lead to the same folder.
        /// </summary>
        public PathsNotDifferentException()
        {

        }
    }
}
