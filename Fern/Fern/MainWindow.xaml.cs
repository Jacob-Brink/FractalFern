using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace FernNamespace
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //canvas is used for faster draw times
        private Bitmap _canvas;

        public MainWindow()
        {
            InitializeComponent();
            _canvas = new Bitmap((int) this.Width, (int) this.Height);
        }
        
        private void drawToImage()
        {
            using (Graphics g = Graphics.FromImage(_canvas))
            {
                g.Clear(System.Drawing.Color.White);
                Fern f = new Fern(fallOffSlider.Value, (int)reduxSlider.Value, turnSlider.Value, g, (int) this.Width, (int) this.Height);

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                MemoryStream pngStream = new MemoryStream();
                _canvas.Save(pngStream, ImageFormat.Png);
                pngStream.Position = 0;
                bitmapImage.StreamSource = pngStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                CanvasImage.Source = bitmapImage;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            drawToImage();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            drawToImage();
        }
    }

}
