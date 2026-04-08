using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kiva.Settings.Views
{
    /// <summary>
    /// Interaction logic for MiscSettings.xaml
    /// </summary>
    public partial class MiscSettings : UserControl
    {
        private KivaSettings settings;

        public KivaSettings Settings
        {
            get => settings; set
            {
                settings = value;
                SetValues();
            }
        }

        public MiscSettings()
        {
            InitializeComponent();
        }

        void SetValues()
        {
            backgroundColor.Color = settings.General.BackgroundColor;
            barColor.Color = settings.General.BarColor;
            hideInfoCard.IsChecked = settings.General.HideInfoCard;
            windowTopmost.IsChecked = settings.General.MainWindowTopmost;
            skipLoad.IsChecked = settings.General.SkipLoadSettings;
            discordRP.IsChecked = settings.General.DiscordRP;

            var cp = settings.General.InfoCardParams;

            timeLabel.IsChecked = (cp & CardParams.Time) > 0;
            fpsLabel.IsChecked = (cp & CardParams.FPS) > 0;
            renderedNotesLabel.IsChecked = (cp & CardParams.RenderedNotes) > 0;
        }

        private void BackgroundColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (IsInitialized)
                settings.General.BackgroundColor = backgroundColor.Color;
        }

        private void BarColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (IsInitialized)
                settings.General.BarColor = barColor.Color;
        }

        private void hideInfoCard_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            settings.General.HideInfoCard = hideInfoCard.IsChecked;
        }

        private void windowTopmost_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            settings.General.MainWindowTopmost = windowTopmost.IsChecked;
        }

        private void cardLabel_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            CardParams cp = 0;

            if (timeLabel.IsChecked) cp |= CardParams.Time;
            if (fpsLabel.IsChecked) cp |= CardParams.FPS;
            if (renderedNotesLabel.IsChecked) cp |= CardParams.RenderedNotes;

            settings.General.InfoCardParams = cp;
        }

        private void skipLoad_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            settings.General.SkipLoadSettings = skipLoad.IsChecked;
        }

        private void discordRP_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            settings.General.DiscordRP = discordRP.IsChecked;
        }
    }
}
