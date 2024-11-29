using BitRuisseau.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRuisseau.tools
{
    public static class FileManager
    {
        public static List<FileModel> GetFileInfo(string path)
        {
            List<FileModel> files = new List<FileModel>();

            if (Directory.Exists(path))
            {
                string[] fileEntries = Directory.GetFiles(path);
                foreach (string file in fileEntries)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    files.Add(new FileModel(fileInfo.Name, fileInfo.Length.ToString(), fileInfo.FullName, fileInfo.Extension));
                }

                //Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = Directory.GetDirectories(path);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    files.AddRange(GetFileInfo(subdirectory));
                }
            }
            return files;
        }
    }
}
