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
        static void Main(string[] args) {
            //bool exists = Directory.Exists("2019-07-02_03-50");
            //show in console all files
            Directory.SetCurrentDirectory(@"C:\MyRepos\Practice2\Practice2\Practice2\bin\Debug\2019-07-02_03-50");
            allDirs2(Directory.GetCurrentDirectory(), 0);

            void allDirs2(string dir, int numOfSpaces) {
                //string[] newDirs =      Directory.GetDirectories(dir);
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                FileInfo[] files = dirInfo.GetFiles("*.*");
                if (files.Length != 0) {
                    string folderName = new DirectoryInfo(dir).Name;
                    for (int i = 0; i < numOfSpaces; i++) {
                        Console.Write("|");
                    }
                    Console.WriteLine(folderName);
                }
                foreach (FileInfo file in files) {
                    for (int i = 0; i < numOfSpaces+1; i++) {
                        Console.Write("|");
                    }
                    Console.WriteLine(file.Name);
                }
                DirectoryInfo[] newDirs = dirInfo.GetDirectories();
                if (newDirs.Length != 0) {
                    foreach (var newDir in newDirs) {
                        allDirs2(newDir.FullName, numOfSpaces + 1);
                    }
                }

            }
            
            //Console.WriteLine(Directory.GetCurrentDirectory());
            Console.ReadLine();
        }
    }
    //
}

