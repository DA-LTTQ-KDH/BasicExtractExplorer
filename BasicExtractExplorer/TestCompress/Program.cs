﻿using System;
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
            SevenZipBase.SetLibraryPath(@"7z.dll");
            SevenZip.SevenZipExtractor sevenZipExtractor = new SevenZipExtractor(@"D:\qn.zip");
            foreach(string info in sevenZipExtractor.ArchiveFileNames)
            {
                Console.WriteLine(info);
            }
            

            Console.WriteLine("---End");

            Console.Read();
        }

        
    }
}
