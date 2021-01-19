using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FindPhotoDuplicates;
using Xunit;

namespace FindPhotoDuplicates.tests
{
    
    public class FindPhotoDuplicatesTests
    {
        [Theory]
        [InlineData(new byte[] { 255, 216, 255 }, new byte[] { 255, 216, 255 }, true)]
        [InlineData(new byte[] { 255, 216 }, new byte[] { 255, 216, 255 }, false)]
        [InlineData(new byte[] { 255, 216, 255 }, new byte[] { 255, 255 }, false)]
        [InlineData(new byte[] { 255, 216, 255,0, 255, 100, 255 }, new byte[] { 255, 216, 255, 0, 255, 100, 255 }, true)]
        [InlineData(new byte[] { }, new byte[] { }, true)]

        public void IsDuplicate_Validate_Bytearrays(byte[] file, byte[] comparisonfile, bool isValid)
        {
            // Actual
            bool actual = FindPhotoDuplicates.IsDuplicate(file, comparisonfile);

            // Assert
            Assert.Equal(isValid, actual);
        }

        [Fact]
        public void SearchListForDuplicate_FileNotFound()
        {
            
            List<FileDetails> Files = new List<FileDetails>();

            FileDetails File = new FileDetails()
            {
                Filelocation = "",
                Duplicates = new List<String>()
            };

            // Assert
            Assert.Throws<FileNotFoundException>(() => FindPhotoDuplicates.SearchListForDuplicate(File, Files));
        }

        [Fact]
        public void SearchListForDuplicate_FirstFileFound()
        {
            string TestFolder = String.Format(@"{0}/TestImages", Environment.CurrentDirectory);

            FileDetails File = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "File1.jpg"),
                Duplicates = new List<String>()
            };
            List<FileDetails> Files = new List<FileDetails>();
            // Actual
            Files = FindPhotoDuplicates.SearchListForDuplicate(File, Files);


            // Assert
            Assert.Equal(Files.Count,1);
        }

        [Fact]
        public void SearchListForDuplicate_AdditionalFileFound()
        {
            string TestFolder = String.Format(@"{0}/TestImages", Environment.CurrentDirectory);

            FileDetails File1 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "File1.jpg"),
                Duplicates = new List<String>()
            };
            FileDetails Identical1 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "Identical1.jpg"),
                Duplicates = new List<String>()
            };

            List<FileDetails> Files = new List<FileDetails>();

            Files.Add(File1);
            

            // Actual
            Files = FindPhotoDuplicates.SearchListForDuplicate(Identical1, Files);


            // Assert
            Assert.Equal(2, Files.Count);
        }

        [Fact]
        public void SearchListForDuplicate_DuplicateFileFound()
        {
            string TestFolder = String.Format(@"{0}/TestImages", Environment.CurrentDirectory);

            FileDetails File1 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "File1.jpg"),
                Duplicates = new List<String>()
            };
            FileDetails Identical1 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "Identical1.jpg"),
                Duplicates = new List<String>()
            };
            FileDetails Identical2 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "Identical2.jpg"),
                Duplicates = new List<String>()
            };
            List<FileDetails> Files = new List<FileDetails>();

            Files.Add(File1);
            Files.Add(Identical1);
            // Actual
            Files = FindPhotoDuplicates.SearchListForDuplicate(Identical2, Files);

            // Assert
            Assert.Equal(2, Files.Count);
        }


        [Fact]
        public void SearchListForDuplicate_DuplicateFileFoundAddedtoList()
        {
            string TestFolder = String.Format(@"{0}/TestImages", Environment.CurrentDirectory);

            FileDetails File1 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "File1.jpg"),
                Duplicates = new List<String>()
            };
            FileDetails Identical1 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "Identical1.jpg"),
                Duplicates = new List<String>()
            };
            FileDetails Identical2 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "Identical2.jpg"),
                Duplicates = new List<String>()
            };
            List<FileDetails> Files = new List<FileDetails>();

            Files.Add(File1);
            Files.Add(Identical1);
            // Actual
            Files = FindPhotoDuplicates.SearchListForDuplicate(Identical2, Files);

            // Assert
            Assert.Single(Files[1].Duplicates);
        }

        [Fact]
        public void SearchListForDuplicate_MultipleDuplicateFileFoundAddedtoList()
        {
            string TestFolder = String.Format(@"{0}/TestImages", Environment.CurrentDirectory);

            FileDetails File1 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "File1.jpg"),
                Duplicates = new List<String>()
            };
            FileDetails Identical1 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "Identical1.jpg"),
                Duplicates = new List<String>()
            };
            FileDetails Identical2 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "Identical2.jpg"),
                Duplicates = new List<String>()
            };
            FileDetails Identical3 = new FileDetails()
            {
                Filelocation = String.Format(@"{0}\\{1}", TestFolder, "Identical3.jpg"),
                Duplicates = new List<String>()
            };
            List<FileDetails> Files = new List<FileDetails>();

            Files.Add(File1);
            Files.Add(Identical1);
            // Actual
            Files = FindPhotoDuplicates.SearchListForDuplicate(Identical2, Files);
            Files = FindPhotoDuplicates.SearchListForDuplicate(Identical3, Files);

            // Assert
            Assert.Equal(2,Files[1].Duplicates.Count);
        }


    }
}
