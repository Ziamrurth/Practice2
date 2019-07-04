using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Practice2
{
    public static class MyDllFileSearcher
    {
        const string SearchFilesExtension = "dll";
        static string GetSearchPattern() {
            return $"*.{SearchFilesExtension}";
        }
        static IEnumerable<FileInfo> GetDllFiles(string startSearchDir, SearchOption howToSearch) {
            return new DirectoryInfo(startSearchDir)
                .EnumerateFiles(GetSearchPattern(), howToSearch);
        }
        public static List<string> GetDllFilesInTopDirectory(string topSearcDir) {
            return GetDllFiles(topSearcDir, SearchOption.TopDirectoryOnly)
                .Select(f => f.Name)
                .ToList();
        }
        static string GetFileParentDir(FileInfo fileInfo) {
            string[] pathtokens = fileInfo.FullName.Split('\\');
            return pathtokens[pathtokens.Length - 2];
        }
        public static List<FileInfo> GetDllFilesRecursive(string startSearchDir, string finishParentDir) {
            var newFilesInfosAll = GetDllFiles(startSearchDir, SearchOption.AllDirectories);
            List<FileInfo> result = new List<FileInfo>();
            foreach (FileInfo newFileInfo in newFilesInfosAll) {
                if (GetFileParentDir(newFileInfo) == finishParentDir) {
                    result.Add(newFileInfo);
                }
            }
            return result;
        }
    }
    class Program
    {

        static void Main(string[] args) {
            //TODO:
            //Create Enum
            //Supply Name
            //Add Some Values

            //Iterate ForEach

            //Write All Values To Console
            //Its Test Representation + Int Code

            Console.WriteLine("This Is Test App. Run Using Test Runner");
            Console.ReadLine();
        }
    }

    [TestFixture]
    class ProgramTest
    {
        [Test]
        public void PackagesTestFrameworkFull() {
            Directory.SetCurrentDirectory(@"C:\MyRepos\Practice2\2019-07-02_03-50---");
            string startSearchDir = "Packages";
            CollectAndAssertFiles("1Full", startSearchDir, "net452");
            CollectAndAssertFiles("2Standard", startSearchDir, "netstandard2.0");
        }
        void CollectAndAssertFiles(string realExistingFilesDirName, string actualStrartSearchDir, string finishParentDir) {
            string realExistingFilesFullPath = Path.Combine(Directory.GetCurrentDirectory(), realExistingFilesDirName);
            List<string> realExistingFileNames = MyDllFileSearcher.GetDllFilesInTopDirectory(realExistingFilesFullPath);

            string actualFilesDir = Path.Combine(Directory.GetCurrentDirectory(), actualStrartSearchDir);
            List<FileInfo> actualFilesInfosProcessed = MyDllFileSearcher.GetDllFilesRecursive(actualFilesDir, finishParentDir);

            //assert
            AssertFiles(realExistingFilesDirName, realExistingFileNames, actualFilesInfosProcessed);
        }

        void AssertFiles(string folderName, List<string> filesNamesTest, List<FileInfo> fileWithFullPathInfoActual) {
            Assert.IsTrue(filesNamesTest.Count != 0, $"Nothing To Check In {folderName}");
            foreach (string fileNameTest in filesNamesTest) {
                bool isSameFile = false;
                foreach (FileInfo fileNameActual in fileWithFullPathInfoActual) {
                    if (fileNameActual.Name == fileNameTest) {
                        isSameFile = true;
                        break;
                    }
                }
                Assert.IsTrue(isSameFile, $"Not found file {fileNameTest} from {folderName}");
            }
        }
    }

}

