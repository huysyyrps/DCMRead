using DCMRead.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace DCMRead.ViewModels
{
    public class FileSeeViewModel:BindableBase
    {
        private List<FileSeeModel> _buttonList = new List<FileSeeModel>();

        public List<FileSeeModel> ButtonList
        {
            get { return _buttonList; }
            set { SetProperty(ref _buttonList, value); }
        }
        private FileSeeViewModel()
        {
            ButtonList = GetButtonList();
        }

        private List<FileSeeModel> GetButtonList()
        {
            List<FileSeeModel> fileModelList = new List<FileSeeModel>();
            FileSeeModel fileSeeModel1 = new FileSeeModel()
            {
                title = "放大镜",
                path = "../Images/Image/ic_loupe.png"
            };
            FileSeeModel fileSeeModel2 = new FileSeeModel()
            {
                title = "放大镜",
                path = "../Images/Image/ic_loupe.png"
            };
            FileSeeModel fileSeeModel3 = new FileSeeModel()
            {
                title = "放大镜",
                path = "../Images/Image/ic_loupe.png"
            };
            FileSeeModel fileSeeModel4 = new FileSeeModel()
            {
                title = "放大镜",
                path = "../Images/Image/ic_loupe.png"
            };
            FileSeeModel fileSeeModel5 = new FileSeeModel()
            {
                title = "放大镜",
                path = "../Images/Image/ic_loupe.png"
            };
            FileSeeModel fileSeeModel6 = new FileSeeModel()
            {
                title = "放大镜",
                path = "../Images/Image/ic_loupe.png"
            };
            fileModelList.Add(fileSeeModel1);
            fileModelList.Add(fileSeeModel2);
            fileModelList.Add(fileSeeModel3);
            fileModelList.Add(fileSeeModel4);
            fileModelList.Add(fileSeeModel5);
            fileModelList.Add(fileSeeModel6);
            return fileModelList;
        }
    }
}
