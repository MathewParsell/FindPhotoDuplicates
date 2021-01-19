using System;
using System.Collections.Generic;
using System.IO;

namespace FindPhotoDuplicates
{

    static class FileDirectory
    {
        public static string filePath;
    }

    public class FileDetails
    {
        public string Filelocation { get; set; }
        public List<string> Duplicates { get; set; }
    }


    public class FindPhotoDuplicates
    {

        static void Main()
        {

            FileDirectory.filePath = String.Format(@"{0}\Images", Directory.GetCurrentDirectory());
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }

        }

        private static bool MainMenu()
        {
            
            Console.Clear();
            Console.WriteLine("----------------------------------{0}Welcome to Find File Duplicates! {0}----------------------------------{0}Searching for duplicate files in {1}{0}", Environment.NewLine, FileDirectory.filePath);
            Console.WriteLine("1) Change file path");
            Console.WriteLine("2) Begin Search.");
            Console.WriteLine("3) Exit");
            Console.Write("{0}Select an option: ", Environment.NewLine);

            switch (Console.ReadLine())
            {
                case "1":
                    SetNewFilePath();
                    return true;
                case "2":
                    BeginSearch();
                    return true;
                case "3":
                    return false;
                default:
                    return true;
            }
        }

        private static void ReturnToMenu(string Message)
        {
                Console.WriteLine(Message);
                Console.Write("Press Enter to return to main menu");
                Console.ReadLine();
        }

        private static void SetNewFilePath()
        {
            Console.Clear();
            Console.WriteLine("Enter new file path: ");

            string newPath = Console.ReadLine();


            if(Directory.Exists(newPath))
            {
                FileDirectory.filePath = newPath;
            }
            else
            {
                ReturnToMenu("File path was not found on the disk! Path was not updated.");
            }

        }

        private static void BeginSearch()
        {
            Console.Clear();
            List<string> files = new List<string>();

            files.AddRange(Directory.GetFiles(FileDirectory.filePath, "*", SearchOption.AllDirectories));
            if (files.Count > 0)
            {
                List<FileDetails> detailList = new List<FileDetails>();

                foreach (string file in files)
                {
                    FileDetails filedetail = new FileDetails()
                    {
                        Filelocation = file,
                        Duplicates = new List<string>()
                    };
                    detailList = SearchListForDuplicate(filedetail, detailList);

                }

                foreach (FileDetails detail in detailList)
                {
                    if (detail.Duplicates.Count > 0)
                    {
                        Console.WriteLine("{0}{1} has {2} duplicate(s) in the following places: ", Environment.NewLine, detail.Filelocation, detail.Duplicates.Count.ToString());
                        Console.WriteLine("------------------------------------------");
                        foreach (string filelocation in detail.Duplicates)
                        {
                            Console.WriteLine(filelocation);
                        }
                        Console.WriteLine("------------------------------------------");
                    }
                }

                ReturnToMenu(String.Format("Search Complete! {0} files searched", files.Count.ToString()));
            }
            else
            {
                ReturnToMenu("No files were found in that directory");
            }

        }

        public static List<FileDetails> SearchListForDuplicate(FileDetails file, List<FileDetails> fileList)
        {
            if (File.Exists(file.Filelocation))
            {
                if (fileList.Count == 0)
                {
                    fileList.Add(file);
                }
                else
                {
                    byte[] filebytes = File.ReadAllBytes(file.Filelocation);
                    bool FileIsDuplicate = false;
                    foreach (FileDetails detail in fileList)
                    {
                        byte[] detailFileBytes = File.ReadAllBytes(detail.Filelocation);

                        if (IsDuplicate(filebytes, detailFileBytes))
                        {
                            detail.Duplicates.Add(file.Filelocation);
                            FileIsDuplicate = true;
                            break;
                        }

                    };
                    if (!FileIsDuplicate)
                    {
                        fileList.Add(file);
                    }
                }
                return fileList;
            }
            else
            {
                throw  new System.IO.FileNotFoundException();
            }
        }


        public static bool IsDuplicate(byte[] file, byte[] comparisonfile)
        {
                if (file.Length == comparisonfile.Length)
                {
                    for (int i = 0; i < file.Length; i++)
                    {
                        if (file[i] != comparisonfile[i])
                        {
                            return false;
                        }
                    }
                return true;
                }
            return false;
        }
    }
}
