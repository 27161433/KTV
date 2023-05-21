using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Windows.ApplicationModel;

namespace KTV
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            CheckFile();
        }

        readonly string filePath = Path.Combine(Package.Current.InstalledLocation.Path, "settings.json");

        private void CheckFile()
        {
            if (!File.Exists(filePath))
            {
                using StreamWriter sw = File.CreateText(filePath);
                SettingDatas newDatas = new() { NcmToken = "" };
                string newJson = JsonConvert.SerializeObject(newDatas);
                sw.WriteLine(newJson);
            }
        }

        private void Ncm_Login_Api_Click(object sender, RoutedEventArgs e)
        {
            SongData.m_window.Set_MWBorder(true, SongData.nlap);
        }

        private void NcmLogin_Token_Click(object sender, RoutedEventArgs e)
        {
            SongData.m_window.Set_MWBorder(true, SongData.nltp);
        }
    }
}
