using static OneWaySynchronizationOfFolders.SynchronizationMethods;

namespace OneWaySynchronizationOfFolders
{
    public class Program
    {
        static void Main(string[] args)
        {
            string pathToOriginalFolder = args[0];
            string pathToDestinationFolder = args[1];
            string pathToLoggingFolder = args[2];
            int synchronizationInterval = int.Parse(args[3]);

            try
            {
                Synchronize(pathToOriginalFolder, pathToDestinationFolder, pathToLoggingFolder, synchronizationInterval);
            }
            catch (NegativeSynchronizationIntervalException)
            {
                Console.WriteLine("Provided synchronization interval is negative. Please run the program again and provide a positive value.");
            }
            catch (PathsNotDifferentException)
            {
                Console.WriteLine("At least two of provided paths point to the same folder. Before you run the program again, please make sure all provided paths are different. Please start the program again.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Probably, at least one folder of provided paths does not exist. Before you run the program again, please provide new path(s) or create respective folders in accordance with the provided paths. Please start the program again.");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Probably, at least one of provided arguments are null. Before you run the program again, please make sure that all arguments have a non-null value. Please start the program again.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Probably, at least one of provided folder paths is empty. Before you run the program again, please make sure to provide a non-empty value. Please start the program again.");
            }
            catch (Exception)
            {
                Console.WriteLine("Unknown type of exception detected. Before you run the program again, please review provided arguments or your permissions in the system. Please start the program again.");
            }
        }
    }
}
