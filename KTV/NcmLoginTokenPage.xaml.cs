using System.Collections.Generic;
using System.IO;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Windows.ApplicationModel;

namespace KTV
{
    public sealed partial class NcmLoginTokenPage : Page
    {
        public NcmLoginTokenPage()
        {
            InitializeComponent();
            CheckFile(true);
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            SongData.m_window.Set_MWBorder(false, SongData.nltp);
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            CheckFile();
            SongData.m_window.Set_MWBorder(false, SongData.nltp);
        }

        private void CheckFile(bool init=false)
        {
            SettingDatas newDatas = new() { NcmToken = Token.Text };
            string newJson = JsonConvert.SerializeObject(newDatas);

            if (!File.Exists(filePath))
            {
                using StreamWriter sw = File.CreateText(filePath);
                sw.WriteLine(newJson);
                return;
            }

            if (init)
            {
                string json = File.ReadAllText(filePath);
                SettingDatas person = JsonConvert.DeserializeObject<SettingDatas>(json);
                Token.Text = person.NcmToken;
            }
            else File.WriteAllText(filePath, newJson);

        }

        readonly string filePath = Path.Combine(Package.Current.InstalledLocation.Path, "settings.json");

    }
}
