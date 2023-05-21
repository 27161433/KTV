using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Linq;
using WinUIEx;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace KTV
{
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow()
        {
            InitializeComponent();

            EnsureEarlyWindow();
        }


        private void EnsureEarlyWindow()
        {

            AppWindow.Title = "KTV";

            AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;

            Width = 1280;
            Height = 720;

            MinWidth = 1280;
            MinHeight = 720;

            this.CenterOnScreen();

        }


        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Content = SongData.ncm;
        }

        private NavigationViewItem Nvi;

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                p = SongData.sp;
                SearchDataHide.Begin();
            }
            else
            {
                Nvi = sender.MenuItems.OfType<NavigationViewItem>().First(x => (string)x.Content == (string)args.InvokedItem);
                SearchDataHide.Begin();
            }
        }


        private void NavView_Navigate()
        {
            if (p != null)
            {
                ContentFrame.Content = p;
                p = null;

                return;
            }
            switch (Nvi.Tag)
            {
                case "NCMSearch":
                    ContentFrame.Content = SongData.ncm;
                    break;

                case "YTSearch":
                    ContentFrame.Content = SongData.yt;
                    break;

                case "SongList":
                    ContentFrame.Content = SongData.sl;
                    break;

                case "Favorites":
                    ContentFrame.Content = SongData.fvr;
                    break;
            }
        }

        private void UpdatePage()
        {
            NavigationViewItem item = NavView.SelectedItem as NavigationViewItem;
            switch (item.Name)
            {
                case "NCMSearch":
                    ContentFrame.DataContext = SongData.ncm;
                    break;

                case "YTSearch":
                    ContentFrame.Content = SongData.yt;
                    break;

                case "SongList":
                    ContentFrame.Content = SongData.sl;
                    break;

                case "Favorites":
                    ContentFrame.Content = SongData.fvr;
                    break;
            }
        }

        public void SetDownloadCount()
        {
            DownloadCount.Text = SongData.DownloadSongList.Count.ToString();
            if (SongData.DownloadSongList.Count == 0)
            {
                DownloadCount.Visibility = Visibility.Collapsed;
                ProgressRing.IsActive = false;
                DownloadImg.Visibility = Visibility.Collapsed;
            }
            else
            {
                DownloadCount.Visibility = Visibility.Visible;
                ProgressRing.IsActive = true;
                DownloadImg.Visibility = Visibility.Visible;
            }
        }


        private void SearchBar_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                string input = SearchBar.Text;

                SongData.ncm.UpdateList(input, "getncm.exe");

                SongData.yt.UpdateList(input, "getyt.exe");
                //"https://www.youtube.com/watch?v=IjlO1oX_SKo&list=PLMYbN7xlpkc-AdBT6X6zW5y0YbnIe8Y8O&index=73&ab_channel=%E3%83%9F%E3%82%BB%E3%82%AB%E3%82%A4-misekai-";

                string isyt = @"^(https?\:\/\/)?(www\.youtube\.com|youtu\.?be|music\.youtube\.com)\/(watch\?v=|embed\/)?([a-zA-Z0-9\-_]+)";
                string isncm = @"^https://music\.163\.com/\S+";
                Regex regex0 = new(isyt, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                Regex regex1 = new(isncm, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (regex0.IsMatch(input))
                {
                    YTSearch.IsSelected = true;
                    p = SongData.yt;
                    SearchDataHide.Begin();

                } 
                else if (regex1.IsMatch(input))
                {
                    NCMSearch.IsSelected = true;
                    p = SongData.ncm;
                    SearchDataHide.Begin();
                }
                else if (!YTSearch.IsSelected && !NCMSearch.IsSelected)
                { 
                    NCMSearch.IsSelected = true;
                    p = SongData.ncm;
                    SearchDataHide.Begin();
                }
                else UpdatePage();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {   
            string input = SearchBar.Text;

            SongData.ncm.UpdateList(input, "getncm.exe");

            SongData.yt.UpdateList(input, "getyt.exe");
            UpdatePage();
            
        }



        public static async Task<string> GetResponse(string url)
        {
            using HttpClient client = new ();
            using HttpResponseMessage response = await client.GetAsync(url);
            using HttpContent content = response.Content;
            string result = await content.ReadAsStringAsync();
            return result;
        }

        public void Set_MWBorder(bool enabled, Page page)
        {
            BorderFrame.Content = page;
            if (enabled)
            {
                MWBorder.Visibility = Visibility.Visible;
                AddFovGrid.Visibility = Visibility.Visible;
                AddFovGridShow.Begin();
                MWBorderShow.Begin();
            }
            else
            {
                AddFovGridHide.Begin();
                MWBorderHide.Begin();
            }

        }

        private Page p = null;

        private void SearchDataHideAnimation_Completed(object sender, object e)
        {
            NavView_Navigate();

            SearchDataShow.Begin();
        }

        private void FadeOutThemeAnimation_Completed(object sender, object e)
        {
            MWBorder.Visibility = Visibility.Collapsed;
            AddFovGrid.Visibility= Visibility.Collapsed;
        }

        public void SetFavoriteListPage(Page page)
        {
            p = page;
            SearchDataHide.Begin();
        }


    }
}
