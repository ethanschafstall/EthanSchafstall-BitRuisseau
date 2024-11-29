using BitRuisseau.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitRuisseau
{
    public class Catalog
    {
        public List<FileModel> fileModels { get; set; } = new List<FileModel>();
        public Catalog(){ }
    }
}
