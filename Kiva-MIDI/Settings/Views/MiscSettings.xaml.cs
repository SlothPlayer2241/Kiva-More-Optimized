using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kiva.Settings.Views
{
    public partial class MiscSettings : UserControl
    {
        private KivaSettings settings;

        public KivaSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                SetValues();
            }
        }

        public MiscSettings() => InitializeComponent();

        void SetValues()
        {
            var g = settings.General;

            backgroundColor.Color = g.BackgroundColor;
            barColor.Color = g.BarColor;

            hideInfoCard.IsChecked = g.HideInfoCard;
            windowTopmost.IsChecked = g.MainWindowTopmost;
            skipLoad.IsChecked = g.SkipLoadSettings;
            discordRP.IsChecked = g.DiscordRP;

            timeLabel.IsChecked = g.InfoCardParams.HasFlag(CardParams.Time);
            fpsLabel.IsChecked = g.InfoCardParams.HasFlag(CardParams.FPS);
            renderedNotesLabel.IsChecked = g.InfoCardParams.HasFlag(CardParams.RenderedNotes);
        }

        private void BackgroundColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (IsInitialized)
                settings.General.BackgroundColor = e.NewValue;
        }

        private void BarColor_ValueChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            if (IsInitialized)
                settings.General.BarColor = e.NewValue;
        }

        private void hideInfoCard_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
            => settings.General.HideInfoCard = hideInfoCard.IsChecked;

        private void windowTopmost_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
            => settings.General.MainWindowTopmost = windowTopmost.IsChecked;

        private void skipLoad_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
            => settings.General.SkipLoadSettings = skipLoad.IsChecked;

        private void discordRP_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
            => settings.General.DiscordRP = discordRP.IsChecked;

        private void cardLabel_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            var cp = CardParams.None;

            if (timeLabel.IsChecked) cp |= CardParams.Time;
            if (fpsLabel.IsChecked) cp |= CardParams.FPS;
            if (renderedNotesLabel.IsChecked) cp |= CardParams.RenderedNotes;

            settings.General.InfoCardParams = cp;
        }
    }
}
