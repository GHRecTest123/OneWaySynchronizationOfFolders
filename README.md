# One-Way Synchronization Of Folders
I would like to present my C# solution to one-way synchronization of folders.

## :books: About
The solution conducts a one-way synchronization between two folders - an **original folder** and a **destination folder** - of user's choice. The destination folder holds the latest copy of the original folder files-wise within itself. The synchronization is performed periodically which means that once the first one is completed, after a certain amount of time, another synchronization starts again and so on until a user terminates the code. The frequency is controlled by the user.

Every action performed on each file is logged into a special file, but also gets displayed in a console. For the sake of this solution, we should distinguish three types of actions:
1. CREATE - this action creates a copy of a file from original folder and pastes it in a destination folder, as a new file.
2. OVERWRITE - this action overwrites a file in a destination folder with its counterpart file from an original folder, but only if some differences are detected.
3. DELETE - this action deletes excessive files from a destination folder that do not exist in an original folder.

An exemplary log looks like this:
<img width="717" height="374" alt="log" src="https://github.com/user-attachments/assets/11a70fa5-05ac-4c7f-8f06-e79681e1a168" />

If there is literally no action conducted on any file during one of the synchronization sessions, then it means that the state of the destination folder is identical to the original one. Thus, nothing should be logged:
<img width="626" height="121" alt="emptylog" src="https://github.com/user-attachments/assets/e3edd123-1db6-4328-a35c-0b1b18305eca" />


>[!NOTE]
>The synchronization process in conducted only on the files. If there are some additional subfolders in the original folder, they won't be copied onto the destination folder. Same applies to the content of those subfolders.
>
>**Example:**
>
>This is how the original folder looks like: <img width="519" height="258" alt="stateoforiginalfolder" src="https://github.com/user-attachments/assets/be39d5af-b8a1-43dc-829e-bd98389b103d" />
>
>This is how the destination folder looks like after the synchronization was conducted: <img width="415" height="177" alt="stateofdestinationfolder" src="https://github.com/user-attachments/assets/0f8457db-8429-4bf9-a7da-7bf45b80bb42" />
>
>These are the logs triggered by the synchronization process: <img width="866" height="310" alt="log2" src="https://github.com/user-attachments/assets/144a6e01-ceec-4445-bcff-ed93b9e31d03" />

## :mag_right: Overview of the code
The solution consists of 6 classes:
1. `HelpingMethods.cs` - contains some small, yet significant methods. Here, you can find a method that calculates hash values (using SHA-256 algorithm) for the files and a method that compares files based on hash values, for example.
2. `LogEntry.cs` - introduces a new type of object representing entries in the logs.
3. `SynchronizationMethods.cs` - the most important class for whole synchronization process; it contains static methods for copying, deleting and overwrite files. It also contains a synchronization method called `Synchronize(string pathToOriginalFolder, string pathToDestinationFolder, string pathToLoggingFolder, int synchronizationInterval)` that combines `DeleteFiles(string originalFolderPath, string destinationFolderPath, LogEntry logEntry)`, `OverwriteFiles(string originalFolderPath, string destinationFolderPath, LogEntry logEntry)` and `CopyFiles(string originalFolderPath, string destinationFolderPath, LogEntry logEntry)`.
4. `PathsNotDifferentException.cs` - introduces a new type of exception, crafted just for this solution. It is thrown whenever at least 2 of provided paths point to the same folder.
5. `NegativeSynchronizationIntervalException.cs` - introduces a new type of exception, crafted just for this solution. It is thrown whenever the value provided in `synchronizationInterval` is negative.
6. `Program.cs` - the class where the solution is run.

Aside from that, each method created for the sake of this solution is properly documented in the code itself.

## 💾 Installation

First, you need to have Git Bash installed on your machine. Then, create an empty folder and launch Git Bash in it. Once Git Bash opens, run the following line in it:
```
git clone https://github.com/GHRecTest123/OneWaySynchronizationOfFolders.git
```

Once you're done, the folder you cloned this repository into should look like this: <img width="800" height="382" alt="installation" src="https://github.com/user-attachments/assets/50bf85a3-2d05-4e91-996a-a74a92ebe025" />




## 🚀 How to run it

1. Open the folder you cloned this repository into.
2. Go straight to `path_to_the_folder_with_cloned_repo\OneWaySynchronizationOfFolders\OneWaySynchronizationOfFolders\OneWaySynchronizationOfFolders`. Once you do it, you should be seeing these files: <img width="736" height="400" alt="howtorunit1" src="https://github.com/user-attachments/assets/14f1433d-fcf0-4e69-892f-e6db256362b9" />



3. Launch CMD in this folder.
4. Once you're in CMD, run the following command:
   ```
   dotnet run "path_to_original_folder" "path_to_destination_folder" "path_to_logging_folder" synchronizationFrequency
   ```
where:

a) `path_to_original_folder` - path that leads directly to the folder where the files are kept. Please provide the existing path, otherwise a proper exception will be thrown.

b) `path_to_destination_folder` - path that leads directly to the folder which is going to store the copies of files from the original folder. Please provide the existing path, otherwise a proper exception will be thrown.

c) `path_to_logging_folder` - path that leads directly to the folder in which log files are going to be kept. Please provide the existing path, otherwise a proper exception will be thrown.

d) `synchronizationFrequency` - frequency with which the synchronization is conducted. It's expressed in milliseconds. When one synchronization is over, another one gets started when the amount of time expressed in this variable passes, and so on until the code is terminated. Please provide a non-negative value, otherwise a proper exception will be thrown. If `0` is provided, then the synchronization goes on indefinitely without a break.

>[!WARNING]
>If `dotnet run` is not recognized by your CMD, you probably need to install .NET SDK.

5. Since the code runs indefinitely, when you're all done with it, just close CMD or use `CTRL+C` within CMD.

## :wrench: Brief summary of how the code works

First of all, paths provided in `path_to_original_folder`, `path_to_destination_folder` and `path_to_logging_folder` are verified. The code checks if they point to the same folder. If they do, `PathsNotDifferentException` is thrown. 

Then, the verification of the value provided as `synchronizationInterval` is conducted. If it's negative, `NegativeSynchronizationIntervalException` is thrown. 

If aforementioned exceptions are not thrown, an object of `LogEntry` type is created to initialize the logging process. `Start()` method is invoked on it in order to add the message indicating the start of synchronization process.

Then, the synchronization begins with files deletion. If there are any excessive files in the destination folder that do not exist in the original folder at the same time, they get deleted. The comparison is conducted based on filenames and their file extensions. Each deletion is logged properly - both saved into the text file (existing in the path expressed as `path_to_logging_folder`) and displayed in a console.

Once the code is all done with deleting, it checks if there exist any file in the destination folder that has the same name (including their file extensions) as a file in the original folder. If that's true, then hash values are calculated for both files using SHA-256 algorithm. If those hash values are equal, then it means that the file in the destination folder is the latest version of that file in the original folder, thus nothing is performed. However, if hash values are different, the file in the destination folder gets overwrtitten with its counterpart file (of the same name and file extension) from the original folder. Each overwriting is logged just like each deletion.

At the end, if there are files in the original folder that do not exit in the destination one (the verification is - again - conducted based on filenames and their file extensions), then copies of these files are created and pasted into the destination folder. Each file creation is logged just like each deletion.

`End()` method gets invoked on `LogEntry` object to properly indicate the end of the synchronization process in the logs.

Once the synchronization process is over, the code waits for the amount of time expressed in `synchronizationFrequency`. Then the new synchronization session starts. And so on, indefinitely, until the user terminates the code.


