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
    public sealed partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            InitializeComponent();
            if (!File.Exists(filePath))
            {
                using StreamWriter sw = File.CreateText(filePath);
                sw.WriteLine("[]");
            }

            json = File.ReadAllText(filePath);
            person = JsonConvert.DeserializeObject<List<FListJsonObj>>(json);
            UpdateList();

        }

        string filePath = Path.Combine(Package.Current.InstalledLocation.Path, "favorites.json");


        private readonly ObservableCollection<FListObj> FavoritesList = new();

        public void UpdateList()
        {
            FavoritesList.Clear();
            json = File.ReadAllText(filePath);
            person = JsonConvert.DeserializeObject<List<FListJsonObj>>(json);

            foreach (FListJsonObj obj in person) FavoritesList.Add(new FListObj { Img = obj.Img, Title = obj.Title });
        }

        public SearchData FovCache;

        private string json;
        private List<FListJsonObj> person;

        private void MoreIcon_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void SongList_ItemClick(object sender, ItemClickEventArgs e)
        {
            FListObj list = (FListObj)e.ClickedItem;
            SongData.fplp.UpdateList(list);
            SongData.m_window.SetFavoriteListPage(SongData.fplp);
        }
    }
}
