using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using SevenZip;

namespace TestCompress
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            SevenZipBase.SetLibraryPath(@"C:\Program Files\7-Zip\7z.dll");
            SevenZip.SevenZipExtractor sevenZipExtractor = new SevenZipExtractor(@"D:\qn.zip");
            foreach(string info in GetSubFoldersAndFiles(sevenZipExtractor.ArchiveFileNames, @"Windows\L10_Data\StreamingAssets\assets\res\scenes\randomscenetopic"))
            {
                Console.WriteLine(info);
            }
            

            Console.WriteLine("HelloWorld");

            Console.Read();
        }

        
    }
}
