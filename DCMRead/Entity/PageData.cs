using DCMRead.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMRead.Entity
{
    public class PageData
    {
        public int startIndex { get; set; }
        public int selectIntex { get; set; }
        public string[] dcmFileList { get; set; }
    }
}
