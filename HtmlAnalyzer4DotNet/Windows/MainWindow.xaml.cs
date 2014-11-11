using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExCSS;
using NSoup;
using NSoup.Nodes;

namespace HtmlAnalyzer4DotNet.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HtmlData parsedData = new HtmlData();

        public MainWindow()
        {
            InitializeComponent();

            txtUrl.Focus();
            txtUrl.SelectionStart = txtUrl.Text.Length;
        }

        private void Fetch()
        {
            try
            {
                this.Cursor = Cursors.Wait;
                treeParseData.Items.Clear();

                // Connecting & Fetching ...
                IConnection connection = NSoupClient.Connect(txtUrl.Text);
                connection.Timeout(30000);
                Document document = connection.Get();

                // Parsing ...
                parsedData.Clear();
                parsedData.Url = document.BaseUri;
                parsedData.RawHtml = document.Html();
                parsedData.Title = document.Title;
                foreach (Element meta in document.Select("meta")) parsedData.MetadataList.Add(meta.OuterHtml());
                foreach (Element image in document.Select("img")) parsedData.ImageList.Add(image.Attr("src"));
                foreach (Element anchor in document.Select("a")) parsedData.AnchorList.Add(anchor.Attr("href"));

                Parser parser = new Parser();
                foreach (Element style in document.Select("style"))
                {
                    foreach (StyleRule rule in parser.Parse(style.Html()).StyleRules)
                    {
                        if (rule.Selector is SimpleSelector && rule.Value.StartsWith("."))
                            parsedData.CssClassList.Add(rule.ToString());
                        else
                            parsedData.CssRuleList.Add(rule.ToString());
                    }
                }

                // Filling UI ...
                txtRawHtml.Text = parsedData.RawHtml;
                treeParseData.Items.Add("URL: " + parsedData.Url);
                treeParseData.Items.Add("Title: " + parsedData.Title);
                treeParseData.Items.Add(new TreeViewItem { Header = "Metadata List", ItemsSource = parsedData.MetadataList });
                treeParseData.Items.Add(new TreeViewItem { Header = "Anchor List", ItemsSource = parsedData.AnchorList });
                treeParseData.Items.Add(new TreeViewItem { Header = "CSS Class List", ItemsSource = parsedData.CssClassList });
                treeParseData.Items.Add(new TreeViewItem { Header = "CSS Rule List", ItemsSource = parsedData.CssRuleList });

                TreeViewItem imageList = new TreeViewItem { Header = "Image List" };
                foreach (string imageUrl in parsedData.ImageList)
                {
                    TreeViewItem item = new TreeViewItem { Header = imageUrl, Cursor = Cursors.Hand };
                    item.MouseDoubleClick += (sender1, e1) =>
                    {
                        new ImageViewerWindow(
                            imageUrl.StartsWith("/") ? parsedData.Url.Remove(parsedData.Url.Length - 1) + imageUrl : imageUrl
                        ).ShowDialog();
                    };
                    imageList.Items.Add(item);
                }
                treeParseData.Items.Add(imageList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUrl.Focus();
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            Fetch();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItem source = (MenuItem)e.Source;
            if (source == mnuFetch)
            {
                Fetch();
            }
            else if (source == mnuExit)
            {
                this.Close();
            }
            else if (source == mnuAbout)
            {
                new AboutWindow().ShowDialog();
            }
        }
    }
}