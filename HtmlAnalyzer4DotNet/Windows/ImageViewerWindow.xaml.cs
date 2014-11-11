using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace HtmlAnalyzer4DotNet.Windows
{
    /// <summary>
    /// Interaction logic for ImageViewer.xaml
    /// </summary>
    public partial class ImageViewerWindow : Window
    {
        private string imageUrl;

        public ImageViewerWindow(string imageUrl)
        {
            InitializeComponent();
            this.imageUrl = imageUrl;
        }

        private void ImageViewerWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape) this.Close();
        }

        private void ImageViewerWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                this.Title = imageUrl;
                img.Source = new BitmapImage(new Uri(imageUrl));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }
    }
}