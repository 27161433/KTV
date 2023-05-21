using Microsoft.UI.Xaml;

namespace KTV
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            SongData.m_window.Activate();
        }

    }
}
