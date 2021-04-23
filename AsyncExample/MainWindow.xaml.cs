using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace AsyncExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// In this example, you can see
    /// - to use the async method correctly
    /// - when await is used or not
    /// - the diff. between sync vs async ( 3 times faster in this example )
    /// - This program shows how long to scan several web sites. 
    /// If you want to become a server programmer, u must use async.
    /// 
    /// Ref: https://www.youtube.com/watch?v=2moh18sh5p4&ab_channel=IAmTimCorey
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> url_list = new List<string>();
        public MainWindow()
        {
            InitializeComponent();

            InitializeURLlist();
        }
        private void InitializeURLlist()
        {

            url_list.Add("https://www.microsoft.com");
            url_list.Add("https://www.yahoo.com");
            url_list.Add("https://www.google.com");
            url_list.Add("https://www.naver.com");
            url_list.Add("https://www.cnn.com");
            url_list.Add("https://www.facebook.com");
            url_list.Add("https://www.codeproject.com");
            url_list.Add("https://www.stackoverflow.com");
        }
        private void btnSync_Click(object sender, RoutedEventArgs e)
        {
            tbkMessage.Text = "";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ScanWebSites();
            watch.Stop();
            tbkMessage.Text += $"Total Excution Time by Sync Method : {watch.ElapsedMilliseconds} ms";
        }

        private async void btnAsync_Click(object sender, RoutedEventArgs e)
        {
            tbkMessage.Text = "";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await ScanWebSitesAsync();
            watch.Stop();
            tbkMessage.Text += $"Total Excution Time by Async Method : {watch.ElapsedMilliseconds} ms";

        }
        private async void btnParalleAsync_Click(object sender, RoutedEventArgs e)
        {
            tbkMessage.Text = "";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await ScanWebSitesParalleAsync();
            watch.Stop();
            tbkMessage.Text += $"Total Excution Time by Async Method : {watch.ElapsedMilliseconds} ms";

        }

        private void ScanWebSites()
        {
            foreach ( string site in url_list)
            {
                DisplayWebSiteInfo(DownloadWebSite(site) );
            }
        }

        //Don't use void as a return type in Async methods.
        //private async void ScanWebSitesAsynchronosly()
        //private async Task<string> ScanWebSitesAsynchronosly()
        //private async Task<int> ScanWebSitesAsynchronosly()
        private async Task ScanWebSitesAsync()
        {
            foreach (string site in url_list)
            {
                DisplayWebSiteInfo(await Task.Run(() => DownloadWebSiteAsync(site)));
            }
        }
        private async Task ScanWebSitesParalleAsync()
        {
            List<Task<WebPage>> tasks = new List<Task<WebPage>>();
            foreach (string site in url_list)
            {
                tasks.Add ( Task.Run(() => DownloadWebSiteAsync(site)));

            }
            var web_pages = await Task.WhenAll(tasks);
            foreach ( var web_page in web_pages)
            {
                DisplayWebSiteInfo(web_page);
            }

        }
        private WebPage DownloadWebSite(string site)
        {
            WebPage web_page = new WebPage();
            WebClient client = new WebClient();
            web_page.url = site;
            web_page.data = client.DownloadString(site);
            return web_page;
        }
        private async Task<WebPage> DownloadWebSiteAsync(string site)
        {
            WebPage web_page = new WebPage();
            WebClient client = new WebClient();
            web_page.url = site;
            web_page.data = await client.DownloadStringTaskAsync (site);
            return web_page;
        }
        private void DisplayWebSiteInfo(WebPage web_page)
        {
            tbkMessage.Text += $"{web_page.url} has {web_page.data.Length } bites {Environment.NewLine} ";
        }

    }
}
