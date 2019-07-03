using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice2
{
    class Program
    {
        private static int length;

        public static DirectoryInfo[] GetDirectories() {
            //return new DirectoryInfo(".").GetDirectories();
            return null;
        }
        static void Main(string[] args) {
            //bool exists = Directory.Exists("2019-07-02_03-50");
            //show in console all files

            //Directory.SetCurrentDirectory(@"C:\MyRepos\Practice2\Practice2\Practice2\bin\Debug\2019-07-02_03-50");
            //GetFilesRecursiveInNestedDirs(Directory.GetCurrentDirectory(), 0);
            string baseDir = @"C:\MyRepos\Practice2\2019-07-02_03-50---";
            string purposeDir = @"\lib\net452";

            GetFilesRecursiveInNestedDirs2(baseDir, purposeDir);

            //Console.WriteLine(Directory.GetCurrentDirectory());
            Console.ReadLine();
        }
        public static List<FileInfo> GetFilesRecursiveInNestedDirs(string startSearchDir, string finishParentDir, ref List<FileInfo> filesNames) {
            //string[] newDirs =      Directory.GetDirectories(dir);
            DirectoryInfo dirInfo = new DirectoryInfo(startSearchDir);

            //remove console calls
            //rewrite 2 below - something wrong with collecting an displaying files...

            //if (files.Length != 0) {
            //    string folderName = new DirectoryInfo(baseDir).Name;
            //    for (int i = 0; i < depth; i++) {
            //        Console.Write("|");
            //    }
            //    Console.WriteLine(folderName);
            //}

            FileInfo[] files = dirInfo.GetFiles("*.dll");
            //List<FileInfo> filesNames = new List<FileInfo>();
            foreach (FileInfo file in files) {
                bool shouldAddFile = false;
                if (!string.IsNullOrEmpty(finishParentDir)) {
                    string[] pathTokens = file.FullName.Split('\\');
                    string fileParentDir = pathTokens[pathTokens.Length - 2];
                    if (fileParentDir == finishParentDir)
                        shouldAddFile = true;
                } else {
                    shouldAddFile = true;
                }
                if (shouldAddFile)
                    filesNames.Add(file);
            }

            DirectoryInfo[] newDirs = dirInfo.GetDirectories();
            if (newDirs != null && newDirs.Length != 0) {
                foreach (var newDir in newDirs) {
                    GetFilesRecursiveInNestedDirs(newDir.FullName, finishParentDir, ref filesNames);
                }
            }
            return filesNames;
        }
        public static List<FileInfo> GetFilesRecursiveInNestedDirs2(string baseDir, string purposeDir, int depth = 0) {
            //string[] newDirs =      Directory.GetDirectories(dir);

            //remove console calls
            //rewrite 2 below - something wrong with collecting an displaying files...

            //if (files.Length != 0) {
            //    string folderName = new DirectoryInfo(baseDir).Name;
            //    for (int i = 0; i < depth; i++) {
            //        Console.Write("|");
            //    }
            //    Console.WriteLine(folderName);
            //}

            List<string> folderNNextPath = GetFirstFolderNNextPath(purposeDir);
            DirectoryInfo dirInfo = new DirectoryInfo(baseDir);

            if (folderNNextPath[1] != null && folderNNextPath[1].Length > 1) {
                DirectoryInfo[] newDirs = dirInfo.GetDirectories();
                if (newDirs != null && newDirs.Length != 0) {
                    bool isSameDir = false;
                    foreach (var newDir in newDirs) {
                        Console.WriteLine(newDir.Name);
                        if (newDir.Name == folderNNextPath[0]) {
                            isSameDir = true;

                        }
                        //GetFilesRecursiveInNestedDirs2(newDir.FullName, );
                    }
                }
            }



            FileInfo[] files = dirInfo.GetFiles("*.dll");
            List<FileInfo> filesNames = new List<FileInfo>();
            foreach (FileInfo file in files) {
                filesNames.Add(file);
            }



            return filesNames;
        }
        public static List<string> GetFirstFolderNNextPath(string purpouseDir) {
            List<string> folderNPath = new List<string>();
            int symbolIndexSlash = purpouseDir.Length - 1;
            for (int symbolIndex = 1; symbolIndex < purpouseDir.Length; symbolIndex++) {
                //if (purpouseDir[symbolIndex].ToString() == @"\") {
                if (purpouseDir[symbolIndex] == '\\') {
                    symbolIndexSlash = symbolIndex;
                    break;
                }
            }
            folderNPath.Add(purpouseDir.Remove(symbolIndexSlash));
            folderNPath.Add(purpouseDir.Remove(0, symbolIndexSlash));
            return folderNPath;
        }
    }

    [TestFixture]
    class ProgramTest
    {
        [Test]
        public void PackagesTestFrameworkFull() {
            //arrange
            Directory.SetCurrentDirectory(@"C:\MyRepos\Practice2\2019-07-02_03-50---");
            string startSearchDir = "\\Packages";
            CollectAndAssertFiles(startSearchDir, "net452", @"\1Full", "SomeNested");
            CollectAndAssertFiles(startSearchDir, "netstandard2.0", @"\2Standard", string.Empty);
        }

        void CollectAndAssertFiles(string actualStrartSearchDir, string finishParentDir, string realExistingFilesDirName, string possibleNestedFinishParentDirForRealFiles) {
            //act
            string realExistingFilesFullPath = Directory.GetCurrentDirectory() + realExistingFilesDirName;
            List<FileInfo> realExistingFileInfos = new List<FileInfo>();
            List<string> realExistingFileNames = Program.GetFilesRecursiveInNestedDirs(realExistingFilesFullPath, possibleNestedFinishParentDirForRealFiles, ref realExistingFileInfos).Select(i => i.Name).ToList();
            string actualFilesDir = Path.Combine(Directory.GetCurrentDirectory(), actualStrartSearchDir)s;
            List<FileInfo> actualFileInfosEmpty = new List<FileInfo>();
            List<FileInfo> actualFilesInfosProcessed = Program.GetFilesRecursiveInNestedDirs(actualFilesDir, finishParentDir, ref actualFileInfosEmpty);

            //assert
            AssertFiles(realExistingFileNames, actualFilesInfosProcessed, realExistingFilesDirName);
        }

        void AssertFiles(List<string> filesNamesTest, List<FileInfo> fileWithFullPathInfoActual, string folderName) {
            foreach (string fileNameTest in filesNamesTest) {
                bool isSameFile = false;
                foreach (FileInfo fileNameActual in fileWithFullPathInfoActual) {
                    if (fileNameActual.Name == fileNameTest) {
                        isSameFile = true;
                        break;
                    }
                }
                //Assert.IsTrue(isSameFile, "Not found file " + fileNameTest + " from " + folderName);
                //Assert.IsTrue(isSameFile, string.Format("Not found file {0} from {1}", fileNameTest, folderName));
                Assert.IsTrue(isSameFile, $"Not found file {fileNameTest} from {folderName}");
            }
        }
    }

}

