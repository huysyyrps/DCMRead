using DCMRead.Model;
using Dicom;
using Dicom.Imaging;
using HandyControl.Controls;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DCMRead.ViewModels
{
    internal class DcmFileViewModel : BindableBase
    {

        private String _imagePath = "../Images/Image/nofile.png";

        public String ImagePath 
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }


        private String _btnText = "请选择文件";

        public String BtnText
        {
            get { return _btnText = "请选择文件"; }
            set { SetProperty(ref _btnText, value); }
        }

        //底部分页数
        private int _maxPageCount;

        public int MaxPageCount
        {
            get { return _maxPageCount; }
            set { SetProperty(ref _maxPageCount, value); }
        }

        private ObservableCollection<DcmFileModel> _fileList = new ObservableCollection<DcmFileModel>();

        public ObservableCollection<DcmFileModel> FileList
        {
            get { return _fileList; }
            set { SetProperty(ref _fileList, value); }
        }


        public DelegateCommand _loadDcoFile;
        public DelegateCommand LoadDcoFile =>
            _loadDcoFile ?? (_loadDcoFile = new DelegateCommand(DcmReadFun));

        void DcmReadFun()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "请选择文件";
            fileDialog.Multiselect = true;
            fileDialog.ShowDialog();
            string[] fileList = fileDialog.FileNames;

            int fileLenth = fileList.Length;
            if (fileLenth % 16 > 0)
            {
                this.MaxPageCount = fileLenth / 16+1;
            }
            else
            {
                this.MaxPageCount = fileLenth / 16;
            }

            if(fileList.Length > 0 )
            {
                FileList.Clear();
                //将图像处理模式设置为全局WPF模式
                ImageManager.SetImplementation(WPFImageManager.Instance);
                for (int i = 0; i < fileList.Length; i++)
                {
                    //实例化文件处理对象并打开文件
                    DicomFile dicomFile = DicomFile.Open(@fileList[i]);
                    //获取dicom图像对象
                    DicomImage dicomImage = new DicomImage(dicomFile.Dataset);                
                    WriteableBitmap bitmap = dicomImage.RenderImage().AsWriteableBitmap();


                    DcmFileModel dcmFileModel = new DcmFileModel();
                    //截取文件名
                    String[] pathItem = fileList[i].Split("\\");
                    dcmFileModel.name = pathItem[pathItem.Length - 1];
                    dcmFileModel.bitmap = bitmap;
                    FileList.Add(dcmFileModel);
                }
            }
           
        }
    }
}
