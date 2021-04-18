using System;
using System.Collections.Generic;
using System.IO;

namespace NwPluginManager
{
    public static class FileUtils
    {
        public static string CreateTempFolder(string prefix)
        {
            string tempPath = Path.GetTempPath();
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(tempPath, "NwPlugins"));
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                try
                {
                    Directory.Delete(dir.FullName, true);
                }
                catch (Exception)
                {
                }
            }
            string str = $"{DateTime.Now:yyyyMMdd_HHmmss_ffff}";
            string path = Path.Combine(directoryInfo.FullName, prefix + str);
            DirectoryInfo directory = new DirectoryInfo(path);
            directory.Create();
            return directory.FullName;
        }
        public static bool FileExistsInFolder(string filePath, string destFolder)
        {
            string path = Path.Combine(destFolder, Path.GetFileName(filePath));
            return File.Exists(path);
        }
        public static void CopyDirectory(string sourceDir, string desDir, List<FileInfo> allCopiedFiles)
        {
            try
            {
                string[] directories = Directory.GetDirectories(sourceDir, "*.*", SearchOption.AllDirectories);
                foreach (string text in directories)
                {
                    string str = text.Replace(sourceDir, "");
                    string path = desDir + str;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }
                string[] files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories);
                foreach (string text2 in files)
                {
                    string str2 = text2.Replace(sourceDir, "");
                    string text3 = desDir + str2;
                    if (!Directory.Exists(Path.GetDirectoryName(text3)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(text3));
                    }
                    if (CopyFile(text2, text3))
                    {
                        allCopiedFiles.Add(new FileInfo(text3));
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public static string CopyFileToFolder(string sourceFilePath, string destFolder, List<FileInfo> allCopiedFiles)
        {
            if (!File.Exists(sourceFilePath))
            {
                return null;
            }
            string directoryName = Path.GetDirectoryName(sourceFilePath);

            CopyDirectory(directoryName, destFolder, allCopiedFiles);

            string text = Path.Combine(destFolder, Path.GetFileName(sourceFilePath));
            if (File.Exists(text))
            {
                return text;
            }
            return null;
        }

        public static bool CopyFile(string sourceFilename, string destinationFilename)
        {
            if (!File.Exists(sourceFilename))
            {
                return false;
            }
            FileAttributes fileAttributes = File.GetAttributes(sourceFilename) & ~FileAttributes.ReadOnly;
            File.SetAttributes(sourceFilename, fileAttributes);
            if (File.Exists(destinationFilename))
            {
                FileAttributes fileAttributes2 = File.GetAttributes(destinationFilename) & ~FileAttributes.ReadOnly;
                File.SetAttributes(destinationFilename, fileAttributes2);
                File.Delete(destinationFilename);
            }
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(destinationFilename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFilename));
                }
                File.Copy(sourceFilename, destinationFilename, true);
            }
            catch (Exception)
            {
                return false;
            }
            return File.Exists(destinationFilename);
        }
    }
}