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
    public sealed partial class AddFavoritesPage : Page
    {
        public AddFavoritesPage()
        {
            InitializeComponent();

            if (!File.Exists(filePath))
            {
                using StreamWriter sw = File.CreateText(filePath);
                sw.WriteLine("[]");
            }

            json = File.ReadAllText(filePath);
            person = JsonConvert.DeserializeObject<List<FListJsonObj>>(json);
        }

        string filePath = Path.Combine(Package.Current.InstalledLocation.Path, "favorites.json");


        private readonly ObservableCollection<FListObj> FList = new();

        public void UpdateList()
        {
            json = File.ReadAllText(filePath);
            person = JsonConvert.DeserializeObject<List<FListJsonObj>>(json);

            FList.Clear();
            FList.Add(new FListObj { Img = "ms-appx:///img/add_icon.png", Title = "新增播放清單", Alignment = "Center" });

            foreach (FListJsonObj obj in person) FList.Add(new FListObj { Img = obj.Img, Title = obj.Title });
        }

        public SearchData FovCache;

        private string json;
        private List<FListJsonObj> person;

        private async void SongList_ItemClick(object sender, ItemClickEventArgs e)
        {

            FListObj list = (FListObj)e.ClickedItem;

            if (list.Title == "新增播放清單")
            {
                ListName.Text = "";
                AddNewFovGrid.Visibility = Visibility.Visible;
                AddNewFovGridShow.Begin();

                return;
            }

            FListJsonObj newfov = person.FirstOrDefault(item => item.Title == list.Title);

            if (newfov != null)
            {
                SearchData data = newfov.Songs.FirstOrDefault(item => item.Id == FovCache.Id);
                if (data != null)
                {
                    ContentDialog dialog = new()
                    {
                        Title = "提示",
                        Content = "無法重複添加歌曲",
                        CloseButtonText = "確認",
                        XamlRoot = Content.XamlRoot,
                        DefaultButton = ContentDialogButton.None
                    };
                    ContentDialogResult result = await dialog.ShowAsync();
                    switch (result)
                    {
                        case ContentDialogResult.None:
                            return;
                    }
                }
                newfov.Img = FovCache.Img;
                newfov.Songs.Insert(0, FovCache);

                string newJson = JsonConvert.SerializeObject(person);

                File.WriteAllText(filePath, newJson);
                SongData.m_window.Set_MWBorder(Visibility.Collapsed, false);
                SongData.fvr.UpdateList();
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            SongData.m_window.Set_MWBorder(Visibility.Collapsed, false);
        }

        private async void AddFrvButton_Click(object sender, RoutedEventArgs e)
        {
            FListJsonObj newfov = person.FirstOrDefault(item => item.Title == ListName.Text);
            if (newfov != null || ListName.Text == "新增播放清單")
            {
                ContentDialog dialog = new()
                {
                    Title = "提示",
                    Content = "不可使用已有相同名稱",
                    CloseButtonText = "確認",
                    XamlRoot = Content.XamlRoot,
                    DefaultButton = ContentDialogButton.None
                };
                ContentDialogResult result = await dialog.ShowAsync();
                switch (result)
                {
                    case ContentDialogResult.None:
                        return;
                }
            }
            if (ListName.Text.Length > 30)
            {
                ContentDialog dialog = new()
                {
                    Title = "提示",
                    Content = "名稱不可超過30字",
                    CloseButtonText = "確認",
                    XamlRoot = Content.XamlRoot,
                    DefaultButton = ContentDialogButton.None
                };
                ContentDialogResult result = await dialog.ShowAsync();
                switch (result)
                {
                    case ContentDialogResult.None:
                        return;
                }
            }
            List<SearchData> searchDatas = new()
            {
                FovCache
            };
            person.Insert(0, new FListJsonObj { Title = ListName.Text, Img = FovCache.Img, Songs = searchDatas });

            string newJson = JsonConvert.SerializeObject(person);
            File.WriteAllText(filePath, newJson);

            AddNewFovGridHide.Begin();
            SongData.m_window.Set_MWBorder(Visibility.Collapsed, false);
            SongData.fvr.UpdateList();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewFovGridHide.Begin();
        }

        private void FadeOutThemeAnimation_Completed(object sender, object e)
        {
            AddNewFovGrid.Visibility = Visibility.Collapsed;
        }
    }
}
