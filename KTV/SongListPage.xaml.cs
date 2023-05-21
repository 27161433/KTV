
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.ApplicationModel;
using Microsoft.WindowsAppSDK.Runtime;
using Newtonsoft.Json;
using System.Collections;
using System.Data;

namespace KTV
{
    public sealed partial class SongListPage : Page
    {

        public SongListPage()
        {
            InitializeComponent();

            DispatcherTimer closetimer = new();
            closetimer.Interval = TimeSpan.FromMilliseconds(100);
            closetimer.Tick += (s, args) =>
            {
                if (SongData.CheckClose)
                {
                    SongData.CheckClose = false;
                    Song_List.Clear();
                }

                if (SongData.NextSong)
                {
                    SongData.NextSong = false;
                    Song_List[SongData.SongCount].FontColor = "LightSkyBlue";
                    if (SongData.SongCount != 0)
                    {
                        Song_List[SongData.SongCount - 1].FontColor = "Gray";
                        Song_List[SongData.SongCount - 1].DelButton_Enabled = false;
                    }
                }

                if (SongData.SongFinish)
                {
                    SongData.SongFinish = false;
                    Song_List[SongData.SongCount].FontColor = "Gray";
                    Song_List[SongData.SongCount].DelButton_Enabled = false;
                }
            };

            closetimer.Start();
        }

        private readonly ObservableCollection<ListObj> Song_List = new();

        private static ListObj UpdateList(SearchData item)
        {
            int int_dt = item.Dt;
            int m = int_dt > 60000 ? int_dt / 60000 : 0;
            int s = int_dt > 60000 ? int_dt % 60000 / 1000 : int_dt / 1000;
            string dt = $"{m.ToString().PadLeft(2, '0')}:{s.ToString().PadLeft(2, '0')}";

            return new ListObj
            {
                Img = item.Img,
                Title = item.Title,
                Dt = dt,
                Id = item.Id,
                Type = item.Type,
                Artis = item.Artis,
                Dtr = item.Dt,
                Progress = 0,
                Progress_Visibility = "Visible",
                Progress_Ring = true,
                Count = item.Count,
                FontColor = "White",
                DelButton_Enabled = true,
            };
        }

        public void AddList(SearchData item)
        {
            Song_List.Add(UpdateList(item));
            ListObj newItem = Song_List.FirstOrDefault(i => i.Id == item.Id && i.Count == item.Count);
            Update(newItem);
        }

        public void Update(ListObj item)
        {
            DispatcherTimer progresstimer = new();
            progresstimer.Interval = TimeSpan.FromMilliseconds(10);
            progresstimer.Tick += (s, args) =>
            {
                if (SongData.CheckClose)
                {
                    progresstimer.Stop();
                    return;
                }
                SearchData data = SongData.DownloadSongList.FirstOrDefault(i => i.Id == item.Id);
                string p0 = "yt";
                if (item.Type == 0) p0 = "ncm";
                string musicPath = $"{Package.Current.InstalledLocation.Path}\\cache\\{p0}\\{item.Id}.mp3";
                string videoPath = $"{Package.Current.InstalledLocation.Path}\\cache\\{p0}\\{item.Id}.webm";

                if (data != null)
                {
                    item.Progress = data.ProgressValue;
                }
                else
                {
                    item.Progress_Visibility = "Collapsed";
                    item.Progress_Ring = false;
                    progresstimer.Stop();
                }

                if (File.Exists(musicPath) && (item.Type == 0 || File.Exists(videoPath)))
                {
                    item.Progress_Visibility = "Collapsed";
                    item.Progress_Ring = false;
                    progresstimer.Stop();
                }
            };

            progresstimer.Start();
        }

        public void InsertList(SearchData item)
        {
            Song_List.Insert(SongData.Standby ? SongData.SongCount : SongData.SongCount + 1, UpdateList(item));
            ListObj newItem = Song_List.FirstOrDefault(i => i.Id == item.Id && i.Count == item.Count);
            Update(newItem);
        }

        private async void Del_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ListObj song = (ListObj)button.DataContext;

            ContentDialog dialog = new()
            {
                Title = "提示",
                Content = "是否確認刪除",
                CloseButtonText = "取消",
                PrimaryButtonText = "確認",
                XamlRoot = Content.XamlRoot,
                DefaultButton = ContentDialogButton.Primary
            };
            ContentDialogResult result = await dialog.ShowAsync();
            switch (result)
            {
                case ContentDialogResult.None: return;
                case ContentDialogResult.Primary:
                    break;
            }

            Song_List.Remove(song);
            
            SearchData newItem = SongData.SongList.FirstOrDefault(i => i.Count == song.Count && i.Id == song.Id);
            int index = SongData.SongList.IndexOf(newItem);
            SongData.SongList.Remove(newItem);

            if (index ==  SongData.SongCount)
            {
                ContentDialog dialog2 = new()
                {
                    Title = "提示",
                    Content = "當前正在播放該首歌曲\n是否要立刻切歌",
                    CloseButtonText = "取消",
                    PrimaryButtonText = "確認",
                    XamlRoot = Content.XamlRoot,
                    DefaultButton = ContentDialogButton.Primary
                };
                ContentDialogResult result2 = await dialog2.ShowAsync();
                switch (result2)
                {
                    case ContentDialogResult.None: break;
                    case ContentDialogResult.Primary:
                        if (SongData.SongCount >= SongData.SongList.Count)
                        {
                            SongData.Standby = true;
                            return;
                        }
                        _ = PlayMusic.SendSongData();
                        break;
                }

            }

        }

        private void Favorite_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ListObj song = (ListObj)button.DataContext;

            ListObj newItem = Song_List.FirstOrDefault(item => item.Id == song.Id);

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
