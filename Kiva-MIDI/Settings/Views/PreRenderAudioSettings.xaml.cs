using Kiva.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Midi;

namespace Kiva.Settings.Views
{
    public partial class PreRenderAudioSettings : UserControl
    {
        private KivaSettings settings;
        private readonly Brush selectBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));
        private DockPanel selectedItem;

        public KivaSettings Settings
        {
            get => settings;
            set
            {
                if (settings != null)
                    settings.Soundfonts.SoundfontsUpdated -= OnSoundfontsUpdated;

                settings = value;

                settings.Soundfonts.SoundfontsUpdated += OnSoundfontsUpdated;
                SetValues();
            }
        }

        public PreRenderAudioSettings()
        {
            InitializeComponent();
        }

        private void OnSoundfontsUpdated(bool reload)
        {
            if (reload)
                Dispatcher.Invoke(SetSfs);
        }

        public void SetValues()
        {
            bufferLength.Value = settings.General.RenderBufferLength;
            voices.Value = settings.General.RenderVoices;
            disableFx.IsChecked = settings.General.RenderNoFx;
            simulatedLag.Value = (decimal)(settings.General.RenderSimulateLag * 1000);

            UpdateBufferLabel();
            SetSfs();
        }

        private void SetSfs()
        {
            sfList.Children.Clear();

            foreach (var sf in settings.Soundfonts.Soundfonts)
                sfList.Children.Add(CreateSfItem(sf));
        }

        private DockPanel CreateSfItem(SoundfontData sf)
        {
            var check = new BetterCheckbox
            {
                IsChecked = sf.enabled,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            var label = new Label
            {
                Content = System.IO.Path.GetFileName(sf.path),
                FontSize = 14,
                Padding = new Thickness(3)
            };

            var panel = new DockPanel
            {
                Tag = sf,
                Background = Brushes.Transparent
            };

            panel.Children.Add(check);
            panel.Children.Add(label);

            check.CheckToggled += (_, __) =>
            {
                sf.enabled = check.IsChecked;
                UpdateFonts();
            };

            panel.MouseDown += (_, __) => SetSelected(panel);

            return panel;
        }

        private void SetSelected(DockPanel panel)
        {
            if (selectedItem != null)
                selectedItem.Background = Brushes.Transparent;

            selectedItem = panel;

            if (selectedItem != null)
                selectedItem.Background = selectBrush;
        }

        private void UpdateFonts()
        {
            settings.Soundfonts.Soundfonts = sfList.Children
                .Cast<FrameworkElement>()
                .Select(x => (SoundfontData)x.Tag)
                .ToArray();

            settings.Soundfonts.SaveList();
        }

        private void UpdateBufferLabel()
        {
            double size = settings.General.RenderBufferLength * 48000 * 2 * 4 / 1_000_000.0;
            bufferSizeLabel.Content = $"(~{Math.Round(size)}mb)";
        }

        private bool IsValidSF(string path)
        {
            try
            {
                int handle = BassMidi.BASS_MIDI_FontInit(path, BASSFlag.BASS_DEFAULT);

                if (Bass.BASS_ErrorGetCode() == 0)
                {
                    BassMidi.BASS_MIDI_FontFree(handle);
                    return true;
                }
            }
            catch { }

            return false;
        }

        private void AddSoundfonts(IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                var ext = System.IO.Path.GetExtension(file).ToLowerInvariant();

                if (!new[] { ".sf1", ".sf2", ".sfz", ".sfark", ".sfpack" }.Contains(ext))
                    continue;

                if (!IsValidSF(file))
                {
                    MessageBox.Show($"Invalid soundfont: {System.IO.Path.GetFileName(file)}");
                    continue;
                }

                var sf = new SoundfontData(ext == ".sfz") { path = file };
                sfList.Children.Add(CreateSfItem(sf));
            }

            UpdateFonts();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Soundfonts|*.sf1;*.sf2;*.sfz;*.sfark;*.sfpack;",
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
                AddSoundfonts(dialog.FileNames);
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem == null) return;

            sfList.Children.Remove(selectedItem);
            selectedItem = null;

            UpdateFonts();
        }

        private void MoveSelected(int offset)
        {
            if (selectedItem == null) return;

            int index = sfList.Children.IndexOf(selectedItem);
            int newIndex = index + offset;

            if (index < 0 || newIndex < 0 || newIndex >= sfList.Children.Count)
                return;

            sfList.Children.RemoveAt(index);
            sfList.Children.Insert(newIndex, selectedItem);

            UpdateFonts();
        }

        private void upButton_Click(object sender, RoutedEventArgs e) => MoveSelected(-1);
        private void downButton_Click(object sender, RoutedEventArgs e) => MoveSelected(1);

        private void bufferLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!IsInitialized) return;

            settings.General.RenderBufferLength = (int)bufferLength.Value;
            UpdateBufferLabel();
        }

        private void voices_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (IsInitialized)
                settings.General.RenderVoices = (int)voices.Value;
        }

        private void simulatedLag_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            settings.General.RenderSimulateLag = (double)simulatedLag.Value / 1000.0;
        }

        private void disableFx_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            settings.General.RenderNoFx = disableFx.IsChecked;
        }

        private void sfPanel_PreviewDragEnter(object sender, DragEventArgs e)
        {
            if (IsInitialized)
                sfPanel.Background = selectBrush;
        }

        private void sfPanel_PreviewDragLeave(object sender, DragEventArgs e)
        {
            if (IsInitialized)
                sfPanel.Background = Brushes.Transparent;
        }

        private void sfPanel_PreviewDrop(object sender, DragEventArgs e)
        {
            sfPanel.Background = Brushes.Transparent;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                AddSoundfonts((string[])e.Data.GetData(DataFormats.FileDrop));
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (settings != null)
                settings.Soundfonts.SoundfontsUpdated -= OnSoundfontsUpdated;
        }
    }
}
