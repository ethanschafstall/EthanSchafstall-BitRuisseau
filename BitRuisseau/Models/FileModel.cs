using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRuisseau.Models
{
    public class FileModel
    {
        public string Name { get; set; }
        public string SizeInKb { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public FileModel(string name, string size, string path, string extension)
        {
            Name = name;
            SizeInKb = size;
            Path = path;
            Extension = extension;
        }

    }
}
