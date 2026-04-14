using Kiva.UI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Kiva.Settings.Views
{
    public partial class VisualSettings : UserControl
    {
        private KivaSettings settings;
        private readonly Brush selectBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));

        public KivaSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                SetValues();
            }
        }

        public VisualSettings()
        {
            InitializeComponent();
        }

        private bool Ready() => IsInitialized && settings != null;

        private void SetValues()
        {
            switch (settings.General.KeyRange)
            {
                case KeyRangeTypes.Key88: key88Range.IsChecked = true; break;
                case KeyRangeTypes.Key128: key128Range.IsChecked = true; break;
                case KeyRangeTypes.Key256: key256Range.IsChecked = true; break;
                case KeyRangeTypes.KeyMIDI: midiRange.IsChecked = true; break;
                case KeyRangeTypes.KeyDynamic: dynamicRange.IsChecked = true; break;
                case KeyRangeTypes.Custom: customRange.IsChecked = true; break;
            }

            switch (settings.General.KeyboardStyle)
            {
                case KeyboardStyle.Big: bigKeyboard.IsChecked = true; break;
                case KeyboardStyle.Small: smallKeyboard.IsChecked = true; break;
                case KeyboardStyle.None: noKeyboard.IsChecked = true; break;
            }

            fpsLock.Value = settings.General.FPSLock;
            firstKey.Value = settings.General.CustomFirstKey;
            lastKey.Value = settings.General.CustomLastKey;

            syncFps.IsChecked = settings.General.SyncFPS;
            fpsLock.IsEnabled = !syncFps.IsChecked;

            randomisePaletteOrder.IsChecked = settings.General.PaletteRandomized;

            SetPalettes();
        }

        private void SetPalettes()
        {
            palettesPanel.Children.Clear();

            foreach (var name in settings.PaletteSettings.Palettes.Keys)
            {
                var item = new Grid { Tag = name };

                item.Children.Add(new RippleEffectDecorator
                {
                    Content = new Label { Content = name }
                });

                item.PreviewMouseDown += (_, __) =>
                {
                    if (settings.General.PaletteName == name) return;

                    foreach (Grid g in palettesPanel.Children)
                        g.Background = Brushes.Transparent;

                    settings.General.PaletteName = name;
                    item.Background = selectBrush;
                };

                if (name == settings.General.PaletteName)
                    item.Background = selectBrush;

                palettesPanel.Children.Add(item);
            }
        }

        private void RangeChanged(object sender, RoutedEventArgs e)
        {
            if (!Ready()) return;

            if (sender == key88Range) settings.General.KeyRange = KeyRangeTypes.Key88;
            else if (sender == key128Range) settings.General.KeyRange = KeyRangeTypes.Key128;
            else if (sender == key256Range) settings.General.KeyRange = KeyRangeTypes.Key256;
            else if (sender == midiRange) settings.General.KeyRange = KeyRangeTypes.KeyMIDI;
            else if (sender == dynamicRange) settings.General.KeyRange = KeyRangeTypes.KeyDynamic;
            else if (sender == customRange) settings.General.KeyRange = KeyRangeTypes.Custom;
        }

        private void KBStyleChanged(object sender, RoutedEventArgs e)
        {
            if (!Ready()) return;

            if (sender == noKeyboard) settings.General.KeyboardStyle = KeyboardStyle.None;
            else if (sender == smallKeyboard) settings.General.KeyboardStyle = KeyboardStyle.Small;
            else if (sender == bigKeyboard) settings.General.KeyboardStyle = KeyboardStyle.Big;
        }

        private void FirstKey_ValueChanged(object s, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!Ready()) return;
            settings.General.CustomFirstKey = (int)firstKey.Value;
        }

        private void LastKey_ValueChanged(object s, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!Ready()) return;
            settings.General.CustomLastKey = (int)lastKey.Value;
        }

        private void FpsLock_ValueChanged(object s, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!Ready()) return;
            settings.General.FPSLock = (int)fpsLock.Value;
        }

        private void OpenPaletteFolder_PreviewMouseDown(object s, MouseButtonEventArgs e)
        {
            var path = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                "Palettes");

            Process.Start("explorer.exe", path);
        }

        private void ReloadPalettes_PreviewMouseDown(object s, MouseButtonEventArgs e)
        {
            settings.PaletteSettings.Reload();

            if (!settings.PaletteSettings.Palettes.ContainsKey(settings.General.PaletteName))
                settings.General.PaletteName = settings.PaletteSettings.Palettes.Keys.First();

            SetPalettes();
        }

        private void RandomisePaletteOrder_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.PaletteRandomized = randomisePaletteOrder.IsChecked;
        }

        private void syncFps_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;

            settings.General.SyncFPS = syncFps.IsChecked;
            fpsLock.IsEnabled = !syncFps.IsChecked;
        }
    }
}
