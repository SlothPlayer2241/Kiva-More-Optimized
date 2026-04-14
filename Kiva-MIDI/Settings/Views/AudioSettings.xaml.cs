using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Kiva.Audio.APIs;

namespace Kiva.Settings.Views
{
    public partial class AudioSettings : UserControl
    {
        private KivaSettings settings;
        private readonly Brush selectBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
        private bool kdmapiAvailable;

        public KivaSettings Settings
        {
            get => settings;
            set
            {
                settings = value;

                SetValues();
                winmmSettings.Settings = settings;
                prerenderSettings.Settings = settings;
            }
        }

        public AudioSettings()
        {
            InitializeComponent();

            TryDetectKdmapi();
            SetupEngineHandlers();
        }

        private void TryDetectKdmapi()
        {
            try
            {
                kdmapiAvailable = KDMAPI.IsKDMAPIAvailable();
            }
            catch
            {
                kdmapiAvailable = false;
            }

            if (!kdmapiAvailable)
                kdmapiEngine.Visibility = Visibility.Collapsed;
        }

        private void SetupEngineHandlers()
        {
            kdmapiEngine.PreviewMouseDown += (_, __) => SetEngine(AudioEngine.KDMAPI);
            winmmEngine.PreviewMouseDown += (_, __) => SetEngine(AudioEngine.WinMM);
            prerenderEngine.PreviewMouseDown += (_, __) => SetEngine(AudioEngine.PreRender);
        }

        public void SetValues()
        {
            SetEngine(settings.General.SelectedAudioEngine);
        }

        private void SetEngine(AudioEngine engine)
        {
            ResetUI();

            settings.General.SelectedAudioEngine = engine;

            switch (engine)
            {
                case AudioEngine.KDMAPI:
                    kdmapiEngine.Background = selectBrush;
                    kdmapiSettings.Visibility = Visibility.Visible;
                    break;

                case AudioEngine.WinMM:
                    winmmEngine.Background = selectBrush;
                    winmmSettings.Visibility = Visibility.Visible;
                    break;

                case AudioEngine.PreRender:
                    prerenderEngine.Background = selectBrush;
                    prerenderSettings.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ResetUI()
        {
            kdmapiEngine.Background = Brushes.Transparent;
            winmmEngine.Background = Brushes.Transparent;
            prerenderEngine.Background = Brushes.Transparent;

            kdmapiSettings.Visibility = Visibility.Collapsed;
            winmmSettings.Visibility = Visibility.Collapsed;
            prerenderSettings.Visibility = Visibility.Collapsed;
        }
    }
}
