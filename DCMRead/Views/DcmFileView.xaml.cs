using DCMRead.Entity;
using DCMRead.Model;
using Dicom;
using Dicom.Imaging;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Formats.Asn1.AsnWriter;

namespace DCMRead.Views
{
    /// <summary>
    /// DcmFileView.xaml 的交互逻辑
    /// </summary>
    public partial class DcmFileView : Page
    {
        public DcmFileView()
        {
            InitializeComponent();
            this.fileListBox.AddHandler(ListBox.MouseLeftButtonDownEvent, new MouseButtonEventHandler(this.fileItemClient), true);
        }

        string[] selectList;
        public int fileLenth = 0;
        private int currentIndex = 0;
        public bool isLoupe = false;
        public bool isTagging = false;
        public List<DcmFileModel> fileModelList = new List<DcmFileModel>();
        public List<Rectangle> rectangleList = new List<Rectangle>();
        /// <summary>
        /// 加载dcm文件列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadDcmFile()
        {
            selectList = new string[] { };
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "请选择文件";
            fileDialog.Multiselect = true;
            fileDialog.ShowDialog();
            selectList = fileDialog.FileNames;

            fileLenth = selectList.Length;
            fileModelList = new List<DcmFileModel>();

            if (fileLenth > 0)
            {
                titleGrid.Visibility= Visibility.Collapsed;
                fileListBox.Visibility= Visibility.Visible;
                //将图像处理模式设置为全局WPF模式
                ImageManager.SetImplementation(WPFImageManager.Instance);
                fileModelList = getDicomFileModelList(0, fileLenth);
                this.fileListBox.ItemsSource = fileModelList;
                fileListBox.SelectedIndex = 0;
                image.Source = fileModelList[0].bitmap;
                imageMag.Source = fileModelList[0].bitmap;
                textBlock.Text = fileModelList[0].name;
            }
         
        }
        private List<DcmFileModel> getDicomFileModelList(int startIndex, int fileLenth)
        {
            List<DcmFileModel> fileList = new List<DcmFileModel>();
            for (int i = startIndex; i < fileLenth; i++)
            {
                fileList.Add(setModelData(i, selectList));
            }
            return fileList;
        }
        /// <summary>
        /// dcm转bitmap
        /// </summary>
        /// <param name="i"></param>
        /// <param name="selectList"></param>
        /// <returns></returns>
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

        private void fileItemClient(object sender, MouseButtonEventArgs e)
        {
            //标注清空
            taggingCancle();
            int selectIndex = fileListBox.SelectedIndex;
            currentIndex = selectIndex;
            image.Source = fileModelList[selectIndex].bitmap;
            imageMag.Source = fileModelList[selectIndex].bitmap;
            textBlock.Text = fileModelList[selectIndex].name;
        }

        /// <summary>
        /// 选择文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFile(object sender, RoutedEventArgs e)
        {
            LoadDcmFile();
        }

        /// <summary>
        /// 上一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpFile(object sender, RoutedEventArgs e)
        {
            if (fileLenth > 0)
            {
                if (fileListBox.SelectedIndex == 0)
                {
                    MessageBox.Show("已是第一张");
                }
                else
                {
                    //标注清空
                    taggingCancle();
                    currentIndex += 1;
                    int selectIndex = fileListBox.SelectedIndex;
                    fileListBox.SelectedIndex = selectIndex - 1;
                    image.Source = fileModelList[selectIndex - 1].bitmap;
                    imageMag.Source = fileModelList[selectIndex - 1].bitmap;
                    textBlock.Text = fileModelList[selectIndex - 1].name;
                }
            }
        }

        /// <summary>
        /// 下一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextFile(object sender, RoutedEventArgs e)
        {
            if (fileLenth > 0)
            {
                if (fileListBox.SelectedIndex >= fileLenth - 1)
                {
                    MessageBox.Show("已是最后一张");
                }
                else
                {
                    //标注清空
                    taggingCancle();
                    currentIndex -= 1;
                    int selectIndex = fileListBox.SelectedIndex;
                    fileListBox.SelectedIndex = selectIndex + 1;
                    image.Source = fileModelList[selectIndex + 1].bitmap;
                    imageMag.Source = fileModelList[selectIndex + 1].bitmap;
                    textBlock.Text = fileModelList[selectIndex + 1].name;
                }
            }
        }
        /// <summary>
        /// 是否使用放大镜
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsLoupe(object sender, RoutedEventArgs e)
        {
            isLoupe = !isLoupe;
            if (isLoupe)
            {
                image.Visibility = Visibility.Collapsed;
                imageMag.Visibility = Visibility.Visible;
                btnLoupe.Visibility = Visibility.Collapsed;
                btnLoupeUse.Visibility = Visibility.Visible;
            }
            else
            {
                image.Visibility = Visibility.Visible;
                imageMag.Visibility = Visibility.Collapsed;
                btnLoupe.Visibility = Visibility.Visible;
                btnLoupeUse.Visibility = Visibility.Collapsed;
            }
            
        }
        /// <summary>
        /// 标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsTagging(object sender, RoutedEventArgs e)
        {
            isTagging = !isTagging;
            if (isTagging)
            {
                btnTagging.Visibility = Visibility.Collapsed;
                btnTaggingUse.Visibility = Visibility.Visible;
            }
            else
            {
                btnTagging.Visibility = Visibility.Visible;
                btnTaggingUse.Visibility = Visibility.Collapsed;
            }

        }

        /// <summary>
        /// 撤销标注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle(object sender, RoutedEventArgs e)
        {
            if(rectangleList.Count>0) {
                cnvImage.Children.Remove(rectangleList[rectangleList.Count-1]);
                rectangleList.RemoveAt(rectangleList.Count - 1);
                if (rectangleList.Count==0)
                {
                    btnCancel.Visibility = Visibility.Collapsed;
                }
                cnvImage.UpdateLayout();
            }
        }

        private void saveBitmap(object sender, RoutedEventArgs e)
        {
            if (fileModelList.Count == 0)
            {
                MessageBox.Show("请先选择文件");
            }else
            {
                System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                //folderBrowserDialog.RootFolder = "d:\\";    //设置初始目录
                folderBrowserDialog.ShowDialog();        //这个方法可以显示文件夹选择对话框
                string directoryPath = folderBrowserDialog.SelectedPath;    //获取选择的文件夹的全路径名

                RenderTargetBitmap bmp = new RenderTargetBitmap(650, 550, 90, 96, PixelFormats.Pbgra32);
                bmp.Render(cnvImage);

                var bitmap = new TransformedBitmap(bmp,
                    new ScaleTransform(
                   2, 2));
                String fileName = fileModelList[currentIndex].name.Trim();
                fileName = fileName.Substring(0, fileName.Length - 5) +".png";
                string file = directoryPath+"\\"+ fileName; 
                string Extension = System.IO.Path.GetExtension(file).ToLower();
                BitmapEncoder encoder = new PngBitmapEncoder(); 
                encoder.Frames.Add(BitmapFrame.Create(bitmap)); 
                using (Stream stm = File.Create(file)) 
                {
                    encoder.Save(stm); 
                }
            }
        }

        /// <summary>
        /// 上一张、下一张时删除标注
        /// </summary>
        private void taggingCancle()
        {
            if (rectangleList.Count>0) 
            {
                for (int i = 0; i < rectangleList.Count; i++)
                {
                    cnvImage.Children.Remove(rectangleList[i]);
                }
                rectangleList.Clear();
                btnCancel.Visibility = Visibility.Collapsed;
                cnvImage.UpdateLayout();
            }
        }

        bool isDown = false;
        private Point startPoint;
        private Rectangle rectSelectArea;
        private void mouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isTagging)
            {
                isDown = true;
                startPoint = e.GetPosition(image);
                rectSelectArea = new Rectangle
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 2
                };

                Canvas.SetLeft(rectSelectArea, startPoint.X);
                Canvas.SetTop(rectSelectArea, startPoint.X);
                cnvImage.Children.Add(rectSelectArea);
                rectangleList.Add(rectSelectArea);
                btnCancel.Visibility = Visibility.Visible;
            } 
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (isDown&&isTagging)
            {
                if (e.LeftButton == MouseButtonState.Released || rectSelectArea == null)
                    return;

                var pos = e.GetPosition(cnvImage);

                // Set the position of rectangle
                var x = Math.Min(pos.X, startPoint.X);
                var y = Math.Min(pos.Y, startPoint.Y);

                // Set the dimenssion of the rectangle
                var w = Math.Max(pos.X, startPoint.X) - x;
                var h = Math.Max(pos.Y, startPoint.Y) - y;

                rectSelectArea.Width = w;
                rectSelectArea.Height = h;

                Canvas.SetLeft(rectSelectArea, x);
                Canvas.SetTop(rectSelectArea, y);
            }
        }

        private void mouseUp(object sender, MouseButtonEventArgs e)
        {
            //rectSelectArea = null;
        }
    }
}

