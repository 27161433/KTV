using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;

namespace KTV
{
    public sealed partial class SearchPage0 : Page
    {
        public SearchPage0()
        {
            InitializeComponent();
        }

        private readonly ObservableCollection<ListObj> NCMList = new();

        public async void UpdateList(string input, string server)
        {
            Progress.IsActive = true;

            NCMList.Clear();

            string executablePath = Path.Combine(Package.Current.InstalledLocation.Path, server);

            string commend = $"s -yt \"{input}\"";

            if (server == "getncm.exe") commend = $"s --ncm \"(<>){input}\"";

            SearchData[] datas = await GetData(executablePath, commend);

            Progress.IsActive = false;

            foreach (SearchData music in datas)
            {
                string title = $"{music.Artis} - {music.Title}";
                int int_dt = music.Dt;
                int m = int_dt > 60000 ? int_dt / 60000 : 0;
                int s = int_dt > 60000 ? int_dt % 60000 / 1000 : int_dt / 1000;
                string dt = $"{m.ToString().PadLeft(2, '0')}:{s.ToString().PadLeft(2, '0')}";

                NCMList.Add(new ListObj
                {
                    Img = music.Img,
                    Title = title,
                    Dt = dt,
                    Id = music.Id,
                    Type = music.Type,
                    Artis = music.Artis,
                    Dtr = music.Dt,
                    True_Title = music.Title,
                });
            }
        }

        async Task<SearchData[]> GetData(string fileName, string command)
        {
            using Process process = new();

            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = command;
            process.StartInfo.WorkingDirectory = Package.Current.InstalledLocation.Path;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();

            await process.WaitForExitAsync();

            if (output == null || output == "")
            {
                return await ReadJsonAsync("s0.json");
            }

            SearchData[] datas = JsonConvert.DeserializeObject<SearchData[]>(output);

            return datas;
        }

        public async Task<SearchData[]> ReadJsonAsync(string fileName)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///cache/{fileName}"));

            using Stream stream = await file.OpenStreamForReadAsync();
            using StreamReader reader = new(stream);
            string json = await reader.ReadToEndAsync();
            SearchData[] obj = JsonConvert.DeserializeObject<SearchData[]>(json);
            return obj;
        }

        private async void SongList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListObj item = e.ClickedItem as ListObj;

            if (item != null)
            {
                int index = NCMList.IndexOf(item);

                ContentDialog dialog = new ContentDialog
                {
                    Title = "提示",
                    Content = "請選擇一個操作",
                    PrimaryButtonText = "點歌",
                    SecondaryButtonText = "插歌",
                    CloseButtonText = "取消"
                };

                dialog.XamlRoot = Content.XamlRoot;
                ContentDialogResult result = await dialog.ShowAsync();
                switch (result)
                {
                    case ContentDialogResult.Primary:

                        break;
                    case ContentDialogResult.Secondary:


                        break;
                    case ContentDialogResult.None:


                        break;
                }
            }
        }

        private readonly List<Opt> opts = new();

        private void Shift_Play_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ListObj song = (ListObj)button.DataContext;

            ListObj newItem = NCMList.FirstOrDefault(item => item.Id == song.Id);
            int s = SongData.SongList.Count(i => i.Id == song.Id);

            SearchData data = new()
            {
                Id = newItem.Id,
                Img = newItem.Img,
                Title = newItem.Title,
                Dt = newItem.Dtr,
                Artis = newItem.Artis,
                Type = newItem.Type,
                Lrc = newItem.Lrc,
                ProgressValue = 0,
                Count = s,
            };

            SongData.sl.InsertList(data);

            SongData.SongList.Insert(SongData.Standby ? SongData.SongCount : SongData.SongCount + 1, data);
            SearchData newSong = SongData.DownloadSongList.FirstOrDefault(x => x.Id == newItem.Id);
            if (newSong == null) SongData.DownloadSongList.Add(data);
            SongData.m_window.SetDownloadCount();
            if (!SongData.DownloadStart) PlayMusic.DownloadSong();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ListObj song = (ListObj)button.DataContext;

            ListObj newItem = NCMList.FirstOrDefault(item => item.Id == song.Id);

            int s = SongData.SongList.Count(i => i.Id == song.Id);

            SearchData data = new()
            {
                Id = newItem.Id,
                Img = newItem.Img,
                Title = newItem.Title,
                Dt = newItem.Dtr,
                Artis = newItem.Artis,
                Type = newItem.Type,
                Lrc = newItem.Lrc,
                ProgressValue = 0,
                Count = s,
            };

            SongData.sl.AddList(data);
            SongData.SongList.Add(data);
            SearchData newSong = SongData.DownloadSongList.FirstOrDefault(x => x.Id == newItem.Id);
            if (newSong == null) SongData.DownloadSongList.Add(data);
            SongData.m_window.SetDownloadCount();
            if (!SongData.DownloadStart) PlayMusic.DownloadSong();
        }


        private void Favorite_Button_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;
            ListObj song = (ListObj)button.DataContext;

            ListObj newItem = NCMList.FirstOrDefault(item => item.Id == song.Id);

            if (newItem != null)
            {
                SongData.afp.UpdateList();
                SongData.afp.FovCache = new SearchData
                {
                    Id = newItem.Id,
                    Artis = newItem.Artis,
                    Title = newItem.True_Title,
                    Img = newItem.Img,
                    Dt = newItem.Dtr,
                    Lrc = newItem.Lrc,
                    Type = newItem.Type,
                };

                SongData.m_window.Set_MWBorder(Visibility.Visible, true);

            }

        }
    }
}
