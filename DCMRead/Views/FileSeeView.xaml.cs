using DCMRead.Entity;
using DCMRead.Model;
using Dicom;
using Dicom.Imaging;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace DCMRead.Views
{
    /// <summary>
    /// FileSeeView.xaml 的交互逻辑
    /// </summary>
    public partial class FileSeeView : Page
    {
        public FileSeeView()
        {
            InitializeComponent();
            initData();
            /*Magnifier magnifier= new Magnifier();
            magnifier.Scale = 6;
            magnifier.VerticalOffset = -10;
            magnifier.HorizontalOffset = -10;*/
            this.listBox.AddHandler(ListBox.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.itemClient), true);
        }

        int startIndex= 0;
        int selectIndex=0;
        int currentIndex = 0;
        String[] fileList = new string[] { };

        public void loadPageData(object sender, NavigationEventArgs e)
        {
            if (e.ExtraData != null)
            {
                PageData? pageData = e.ExtraData as PageData;
                if (pageData != null)
                {
                    startIndex = pageData.startIndex;
                    selectIndex = pageData.selectIntex;
                    fileList = pageData.dcmFileList;
                    currentIndex = startIndex + selectIndex;
                    fileimage.Source = setModelData((currentIndex), fileList).bitmap;
                }
            }
        }

        private void initData()
        {
            List<FileSeeModel> fileModelList = new List<FileSeeModel>();
            FileSeeModel fileSeeModel1 = new FileSeeModel()
            {
                title = "放大镜",
                path = "../Images/Image/ic_loupe.png"
            };
            FileSeeModel fileSeeModel2 = new FileSeeModel()
            {
                title = "上一张",
                path = "../Images/Image/ic_up.png"
            };
            FileSeeModel fileSeeModel3 = new FileSeeModel()
            {
                title = "下一张",
                path = "../Images/Image/ic_next.png"
            };
            FileSeeModel fileSeeModel4 = new FileSeeModel()
            {
                title = "标注",
                path = "../Images/Image/ic_tagging.png"
            };
            FileSeeModel fileSeeModel5 = new FileSeeModel()
            {
                title = "保存",
                path = "../Images/Image/ic_save.png"
            };
            FileSeeModel fileSeeModel6 = new FileSeeModel()
            {
                title = "返回",
                path = "../Images/Image/ic_back.png"
            };
            fileModelList.Add(fileSeeModel1);
            fileModelList.Add(fileSeeModel2);
            fileModelList.Add(fileSeeModel3);
            fileModelList.Add(fileSeeModel4);
            fileModelList.Add(fileSeeModel5);
            fileModelList.Add(fileSeeModel6);
            listBox.ItemsSource = fileModelList;

        }

        private DcmFileModel setModelData(int i, string[] selectList)
        {
            //实例化文件处理对象并打开文件

            DicomFile dicomFile = DicomFile.Open(selectList[i]);
            //获取dicom图像对象
            DicomImage dicomImage = new DicomImage(dicomFile.Dataset);
            WriteableBitmap bitmap = dicomImage.RenderImage().AsWriteableBitmap();


            DcmFileModel dcmFileModel = new DcmFileModel();
            //截取文件名
            String[] pathItem = selectList[i].Split("\\");
            dcmFileModel.name = pathItem[pathItem.Length - 1];
            dcmFileModel.bitmap = bitmap;
            return dcmFileModel;
        }

        private void itemClient(object sender, MouseButtonEventArgs e)
        {
            int i = listBox.SelectedIndex;
            switch (listBox.SelectedIndex)
            {
                case 0:
                    MessageBox.Show(i + "");
                    break;
                case 1:                  
                    if(currentIndex== 0)
                    {
                        MessageBox.Show("已经是第一张");
                    }
                    else
                    {
                        currentIndex--;
                        fileimage.Source = setModelData((currentIndex), fileList).bitmap;
                    }
                    break;
                case 2:
                    if (currentIndex == fileList.Length-1)
                    {
                        MessageBox.Show("已经是最后一张");
                    }
                    else
                    {
                        currentIndex++;
                        fileimage.Source = setModelData((currentIndex), fileList).bitmap;
                    }
                    break;
                case 3:
                    MessageBox.Show(i + "");
                    break;
                case 4:
                    MessageBox.Show(i + "");
                    break;
                case 5:
                    /*this.NavigationService.GoBack();
                    DcmFileView dcmFileView= new DcmFileView();
                    int tes = dcmFileView.fileLenth;
                    MessageBox.Show(tes + "");*/
                    DcmFileView dcmFileView = new DcmFileView();
                    NavigationService.Navigate(dcmFileView, currentIndex);
                 
                    break;

            }
        }
    }
}
