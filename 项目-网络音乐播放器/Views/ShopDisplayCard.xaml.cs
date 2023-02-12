using System.Windows.Controls;

namespace 项目_网络音乐播放器.Views
{
    /// <summary>
    /// ShopDisplayCard.xaml 的交互逻辑
    /// </summary>
    public partial class ShopDisplayCard : UserControl
    {
        public ShopDisplayCard()
        {
            InitializeComponent();
            //DataContext = new ShopDiaplayCardViewModel(Id,Title,Artist,Album,MyImage);
        }

        //public int Id { get; set; }
        //public string Title { get; set; }
        //public string Artist { get; set; }
        //public string Album { get; set; }
        //public byte[] ImageBytes { get; set; }




        //private BitmapImage myImage;

        //public BitmapImage MyImage
        //{
        //    get {
        //        if (myImage == null)
        //        {
        //            myImage = ReturnPhoto(ImageBytes);
        //        }
        //        return myImage; 
        //    }
        //    set { myImage = value; }
        //}


        //private BitmapImage ReturnPhoto(byte[] streamByte)
        //{
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(streamByte);
        //    BitmapImage img = new BitmapImage();
        //    img.BeginInit();
        //    img.StreamSource = ms;
        //    img.EndInit();
        //    return img;
        //}
    }
}
