using DCMRead.Views;
using HandyControl.Tools.Extension;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;

namespace DCMRead.ViewModels
{
    public class WelcomeViewModel : BindableBase
    {
        private List<String> _bannerList;
        public List<String> BannerList
        {
            get { return _bannerList; }
            set { 
                SetProperty(ref _bannerList, value); 
            }
        }

        //按钮文字
        private String _title = "进入程序";

        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /*private DelegateCommand _dcmFileCommand;
        public DelegateCommand DcmFileCommand =>
            _dcmFileCommand ?? (_dcmFileCommand = new DelegateCommand(DcmReadFun));

        void DcmReadFun()
        {
            NavigationService.GetNavigationService(new WelcomeView()).Navigate(new DcmFileView());
        }*/


        public WelcomeViewModel() {
            BannerList = GetBnnnerList();
        }

        private List<String> GetBnnnerList()
        {
            return new List<String>
            {
                "../Images/Banner/banner1.jpg",
                "../Images/Banner/banner2.jpg",
                "../Images/Banner/banner3.jpg",
                "../Images/Banner/banner4.jpg",
                "../Images/Banner/banner5.jpg",
            };
        }
    }
   
}
