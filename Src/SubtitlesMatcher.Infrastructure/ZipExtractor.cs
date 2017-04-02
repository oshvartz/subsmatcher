using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;

namespace SubtitlesMatcher.Infrastructure
{
    public class ZipExtractor
    {
        public static  List<string>  ExtractFiles(string zipFilePath, string outputPath)
        {
            return ExtractFiles(zipFilePath, null, outputPath);
        }

        public static bool IsZipFile(string filePath)
        {
            return ZipFile.IsZipFile(filePath);
        }

        public static void ExtractGz(string zipFilePath, string outputFile)
        {
            var gzBuffer = File.ReadAllBytes(zipFilePath);

            var content = Compression.DecompressGz(gzBuffer);

            File.WriteAllBytes(outputFile, content);
        }

        public static List<string> ExtractFiles(string zipFilePath, string fileExtension, string outputPath)
        {
            
            List<string> res = new List<string>();
            using (ZipFile zfile = new ZipFile(zipFilePath))
            {

                var zeQuery = from ze in zfile
                              where fileExtension == null || ze.FileName.EndsWith("." + fileExtension)
                              select ze;

                foreach (var ze in zeQuery)
                {
                    res.Add(ze.FileName);
                    ze.Extract(outputPath);
                }

            }
            return res;

        }
    }
}
