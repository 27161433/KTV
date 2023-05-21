
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Windows.ApplicationModel;

namespace KTV
{
    public sealed partial class FavoritesPageListPage : Page
    {
        public FavoritesPageListPage()
        {
            InitializeComponent();
        }

        private readonly ObservableCollection<ListObj> FList = new();

        string filePath = Path.Combine(Package.Current.InstalledLocation.Path, "favorites.json");

        private string json;
        private List<FListJsonObj> person;

        private void CheckFile()
        {
            if (!File.Exists(filePath))
            {
                using StreamWriter sw = File.CreateText(filePath);
                sw.WriteLine("[]");
            }

            json = File.ReadAllText(filePath);
            person = JsonConvert.DeserializeObject<List<FListJsonObj>>(json);
        }

        private FListObj _list;

        public void UpdateList(FListObj list)
        {
            CheckFile();
            _list = list;
            FListJsonObj newfov = person.FirstOrDefault(item => item.Title == list.Title);
            if (newfov != null)
            {
                List<SearchData> datas = newfov.Songs;
                ListName.Text = newfov.Title;

                FList.Clear();
                foreach (SearchData music in datas)
                {
                    string title = $"{music.Artis} - {music.Title}";
                    int int_dt = music.Dt;
                    string img = music.Img;
                    int m = int_dt > 60000 ? int_dt / 60000 : 0;
                    int s = int_dt > 60000 ? int_dt % 60000 / 1000 : int_dt / 1000;
                    string dt = $"{m.ToString().PadLeft(2, '0')}:{s.ToString().PadLeft(2, '0')}";

                    FList.Add(new ListObj
                    {
                        Img = img,
                        Title = title,
                        Dt = dt,
                        Id = music.Id,
                        Type = music.Type,
                        Artis = music.Artis,
                        Dtr = music.Dt,
                        Progress = 0,
                    });
                }

            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SongData.m_window.SetFavoriteListPage(SongData.fvr);
        }

        private void All_Play_Button_Click(object sender, RoutedEventArgs e)
        {
            All_Play_Button.IsEnabled = false;
            foreach (ListObj newItem in FList)
            {
                int s = SongData.SongList.Count(i => i.Id == newItem.Id);

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
            }
            SongData.m_window.SetDownloadCount();
            if (!SongData.DownloadStart) PlayMusic.DownloadSong();
            All_Play_Button.IsEnabled = true;
        }

        private async void Del_Button_Click(object sender, RoutedEventArgs e)
        {
            FListJsonObj newfov = person.FirstOrDefault(item => item.Title == _list.Title);
            if (newfov != null)
            {
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
                        person.Remove(newfov);
                        string newJson = JsonConvert.SerializeObject(person);
                        File.WriteAllText(filePath, newJson);
                        SongData.fvr.UpdateList();
                        SongData.m_window.SetFavoriteListPage(SongData.fvr);
                        break;
                }
            }
        }

        private async void Del_Song_Button_Click(object sender, RoutedEventArgs e)
        {
            FListJsonObj newfov = person.FirstOrDefault(item => item.Title == _list.Title);
            if (newfov != null)
            {
                List<SearchData> datas = newfov.Songs;
                Button button = sender as Button;
                ListObj song = (ListObj)button.DataContext;

                SearchData newSong = datas.FirstOrDefault(item => item.Id == song.Id);
                if (newSong != null)
                {
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
                            datas.Remove(newSong);
                            if (datas.Count == 0)
                            {
                                person.Remove(newfov);
                                string newJson = JsonConvert.SerializeObject(person);
                                File.WriteAllText(filePath, newJson);
                                SongData.fvr.UpdateList();
                                SongData.m_window.SetFavoriteListPage(SongData.fvr);
                            }
                            else
                            {
                                string newJson = JsonConvert.SerializeObject(person);
                                File.WriteAllText(filePath, newJson);
                                ListObj newItem = FList.FirstOrDefault(item => item.Id == song.Id);
                                FList.Remove(newItem);
                                SongData.fvr.UpdateList();
                            }
                            break;
                    }
                }

            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ListObj song = (ListObj)button.DataContext;

            ListObj newItem = FList.FirstOrDefault(item => item.Id == song.Id);
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

        private void Shift_Play_Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ListObj song = (ListObj)button.DataContext;

            ListObj newItem = FList.FirstOrDefault(item => item.Id == song.Id);
            int s = SongData.SongList.Count(i => i.Id == newItem.Id);
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





    }
}
