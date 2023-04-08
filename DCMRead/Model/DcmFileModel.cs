using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DCMRead.Model
{
    public class DcmFileModel 
    {
        /*public DcmFileModel(string name, BitmapImage image)
        {
            Name = name;
            Image = image;
        }*/

        public String name { get; set; }
        public WriteableBitmap bitmap { get; set; }
    }
}
