using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace KTV
{
    public class SettingDatas
    {
        public string NcmToken { get; set; }
    }

    public class FListObj
    {
        public string Img { get; set; }
        public string Title { get; set; }
        public string Alignment { get; set; }
    }

    public class FListJsonObj
    {
        public string Img { get; set; }
        public string Title { get; set; }
        public List<SearchData> Songs { get; set; }
    }


    public class LrcData
    {
        public string Lrc { get; set; }
        public string Tlyric { get; set; }
        public string Status { get; set; }
    }

    public class SearchData
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Img { get; set; }
        public int Dt { get; set; }
        public string Artis { get; set; }
        public int Type { get; set; }
        public string Lrc { get; set; } = "null";
        public int ProgressValue { get; set; }
        public int Count { get; set; }
        public string Status { get; set; } = "ok";
    }

    public class ListObj : INotifyPropertyChanged
    {
        private string _title;
        private string _img;
        private string _dt;
        private string _id;
        private int _type;
        private string _artis;
        private int _progress;
        private string _Progress_Visibility;
        private string _Play_Button;
        private string _Shift_Play_Button;
        private string _Favorite_Button;
        private bool _DelButton_Enabled;
        private bool _Shift_Play_Button_Enabled;
        private bool _Progress_Ring;
        private int _Dtr;
        private string _FontColor;

        public string Lrc { get; set; } = "null";

        public string True_Title { get; set; }

        public int Count { get; set; }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Img
        {
            get { return _img; }
            set
            {
                if (_img != value)
                {
                    _img = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Dt
        {
            get { return _dt; }
            set
            {
                if (_dt != value)
                {
                    _dt = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Artis
        {
            get { return _artis; }
            set
            {
                if (_artis != value)
                {
                    _artis = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Progress_Visibility
        {
            get { return _Progress_Visibility; }
            set
            {
                if (_Progress_Visibility != value)
                {
                    _Progress_Visibility = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Play_Button
        {
            get { return _Play_Button; }
            set
            {
                if (_Play_Button != value)
                {
                    _Play_Button = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Shift_Play_Button
        {
            get { return _Shift_Play_Button; }
            set
            {
                if (_Shift_Play_Button != value)
                {
                    _Shift_Play_Button = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Favorite_Button
        {
            get { return _Favorite_Button; }
            set
            {
                if (_Favorite_Button != value)
                {
                    _Favorite_Button = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool DelButton_Enabled
        {
            get { return _DelButton_Enabled; }
            set
            {
                if (_DelButton_Enabled != value)
                {
                    _DelButton_Enabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Shift_Play_Button_Enabled
        {
            get { return _Shift_Play_Button_Enabled; }
            set
            {
                if (_Shift_Play_Button_Enabled != value)
                {
                    _Shift_Play_Button_Enabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool Progress_Ring
        {
            get { return _Progress_Ring; }
            set
            {
                if (_Progress_Ring != value)
                {
                    _Progress_Ring = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Dtr
        {
            get { return _Dtr; }
            set
            {
                if (_Dtr != value)
                {
                    _Dtr = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FontColor
        {
            get { return _FontColor; }
            set
            {
                if (_FontColor != value)
                {
                    _FontColor = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public static class SongData
    {
        public static int SongCount = 0;
        public static bool Creat = false;
        public static bool Standby = true;
        public static List<SearchData> SongList = new();
        public static List<SearchData> DownloadSongList = new();
        public static bool PlayerIsCreated = false;
        public static SearchPage0 ncm = new();
        public static SearchPage0 yt = new();
        public static SongListPage sl = new();
        public static FavoritesPage fvr = new();
        public static SettingsPage sp = new();
        public static MainWindow m_window = new ();
        public static AddFavoritesPage afp = new();
        public static FavoritesPageListPage fplp = new();
        public static NcmLoginAPI_Page nlap = new();
        public static NcmLoginTokenPage nltp = new();
        public static bool DownloadStart = false;
        public static bool TimerStart = false;
        public static bool CheckClose = false;
        public static bool NextSong = false;
        public static bool SongFinish = false;

    }

    public class Opt
    {
        public string Id { get; set; }
        public int Value { get; set; }
    }

    public static class PlayMusic
    {
        readonly static string filePath = Path.Combine(Package.Current.InstalledLocation.Path, "settings.json");

        public static async void DownloadSong()
        {
            if (SongData.DownloadSongList.Count == 0)
            {
                SongData.DownloadStart = false;
                return;
            }

            SongData.DownloadStart = true;

            if (!File.Exists(filePath))
            {
                using StreamWriter sw = File.CreateText(filePath);
                SettingDatas newDatas = new() { NcmToken = "" };
                string newJson = JsonConvert.SerializeObject(newDatas);
                sw.WriteLine(newJson);
            }

            string json = File.ReadAllText(filePath);
            SettingDatas person = JsonConvert.DeserializeObject<SettingDatas>(json);


            SearchData song = SongData.DownloadSongList[0];
            string server = "getyt.exe";

            if (song.Type == 0) server = "getncm.exe";

            string executablePath = Path.Combine(Package.Current.InstalledLocation.Path, server);

            string commend = $"d -yt \"{song.Id}\"";

            if (server == "getncm.exe") commend = $"d --ncm \"{person.NcmToken}(<>){song.Id}\"";

            await DownloadVideo(executablePath, commend, song);

        }

        public static async Task DownloadVideo(string fileName, string command, SearchData song)
        {
            using Process process = new();

            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = command;
            process.StartInfo.WorkingDirectory = Package.Current.InstalledLocation.Path;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            StringBuilder sb = new();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    sb.AppendLine(e.Data);

                    if (string.IsNullOrWhiteSpace(e.Data.ToString())) return;

                    if (song.Type == 0)
                    {
                        if (e.Data.ToString().StartsWith("{"))
                        {
                            song.ProgressValue = 100;

                            LrcData datas = JsonConvert.DeserializeObject<LrcData>(e.Data.ToString());
                            song.Lrc = datas.Lrc;
                            if (datas.Status == "err") song.Status = datas.Status;
                            _ = Create_Player();
                            return;
                        }

                        string str = e.Data.ToString();
                        int intValue = int.Parse(str);

                        song.ProgressValue = intValue;

                    }
                    else
                    {
                        if (e.Data.ToString() == "OK")
                        {
                            song.ProgressValue = 100;
                            _ = Create_Player();
                            return;
                        }

                        string str = e.Data.ToString();
                        int startIndex = str.IndexOf("[download]") + 12;
                        int endIndex = str.IndexOf('%');
                        string percentage = str[startIndex..endIndex];
                        double result = double.Parse(percentage);
                        int intValue = Convert.ToInt32(Math.Round(result));

                        song.ProgressValue = intValue;
                    }
                }
            };

            process.Start();
            process.BeginOutputReadLine();

            await process.WaitForExitAsync();

            SongData.DownloadSongList.RemoveAt(0);

            SongData.m_window.SetDownloadCount();

            DownloadSong();
        }

        public static async Task Create_Player()
        {
            if (SongData.Standby && SongData.PlayerIsCreated)
            {
                SongData.Standby = false;
                SongData.SongCount++;
                _ = SendSongData();
                return;
            }

            if (SongData.PlayerIsCreated)
            {
                SongData.Creat = true;
                return;
            }

            using Process process = new();

            process.StartInfo.FileName = Path.Combine(Package.Current.InstalledLocation.Path, "Player");
            process.StartInfo.WorkingDirectory = Package.Current.InstalledLocation.Path;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;

            StringBuilder sb = new();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    sb.AppendLine(e.Data);

                    if (e.Data.ToString() == "finish")
                    {
                        SongData.Standby = false;
                        SongData.Creat = true;
                        _ = SendSongData();
                    }
                    else if (e.Data.ToString() == "next_song")
                    {
                        if (SongData.SongCount + 1 >= SongData.SongList.Count)
                        {
                            SongData.Standby = true;
                            SongData.SongFinish = true;
                            return;
                        }
                        SongData.SongCount++;

                        _ = SendSongData();
                    }
                }
            };

            string s = sb.ToString();

            process.Start();
            SongData.PlayerIsCreated = true;
            process.BeginOutputReadLine();

            await process.WaitForExitAsync();
            SongData.PlayerIsCreated = false;
            SongData.SongCount = 0;
            SongData.SongList.Clear();
            SongData.CheckClose = true;
        }

        public static async Task SendSongData()
        {
            string p0 = "yt";
            if (SongData.SongList[SongData.SongCount].Type == 0) p0 = "ncm";
            string musicPath = $"{Package.Current.InstalledLocation.Path}\\cache\\{p0}\\{SongData.SongList[SongData.SongCount].Id}.mp3";
            string videoPath = $"{Package.Current.InstalledLocation.Path}\\cache\\{p0}\\{SongData.SongList[SongData.SongCount].Id}.webm";

            SongData.NextSong = true;

            if (!File.Exists(musicPath) || (SongData.SongList[SongData.SongCount].Type != 0 && !File.Exists(videoPath)))
            {
                if (SongData.SongList[SongData.SongCount].Status != "err")
                {
                    if (SongData.SongCount != 0) SongData.SongCount--;
                    SongData.Standby = true;
                    return;
                }
            }

            List<SearchData> item = SongData.SongList;
            int count = SongData.SongCount;
            string path = item[count].Type == 1 ? "yt" : "ncm";
            string json = $"{{\"id\":\"{item[count].Id}\",\"title\":\"{item[count].Title}\",\"img\":\"./cache/{path}/{item[count].Id}.jpg\",\"dt\":{item[count].Dt},\"artis\":\"{item[count].Artis}\",\"lrc\":\"{item[count].Lrc}\",\"type\":{item[count].Type},\"status\":\"{item[count].Status}\"}}";
            StringContent content = new(json, Encoding.UTF8, "application/json");
            string url = "http://localhost:8000/";
            using HttpClient client = new();
            using HttpResponseMessage response = await client.PostAsync(url, content);
            string responseContent = await response.Content.ReadAsStringAsync();
        }
    }



}
