using Kiva.Audio.APIs;
using Kiva.UI;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kiva.Settings.Views
{
    public partial class WinMMAudioSettings : UserControl
    {
        private struct DeviceData
        {
            public uint Id;
            public string Name;
        }

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

        public WinMMAudioSettings()
        {
            InitializeComponent();
            LoadDevices();
            ClearSelection();
        }

        private void LoadDevices()
        {
            uint devCount = WinMM.midiOutGetNumDevs();

            for (uint i = 0; i < devCount; i++)
            {
                WinMM.midiOutGetDevCaps(i, out MIDIOUTCAPS device, (uint)Marshal.SizeOf<MIDIOUTCAPS>());

                var data = new DeviceData
                {
                    Id = i,
                    Name = device.szPname
                };

                var item = CreateDeviceItem(data, i);
                devicesList.Children.Add(item);
            }
        }

        private Grid CreateDeviceItem(DeviceData data, int index)
        {
            var item = new Grid { Tag = data };

            item.Children.Add(new RippleEffectDecorator
            {
                Content = new Label
                {
                    Content = data.Name,
                    FontSize = 14,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                }
            });

            item.PreviewMouseDown += (_, __) => SelectDevice(index);

            return item;
        }

        public void SelectDevice(int index)
        {
            if (index < 0 || index >= devicesList.Children.Count)
                return;

            ClearSelection();

            var item = (Grid)devicesList.Children[index];
            item.Background = selectBrush;

            var data = (DeviceData)item.Tag;

            settings.General.SelectedMIDIDevice = (int)data.Id;
            settings.General.SelectedMIDIDeviceName = data.Name;
        }

        public void SetValues()
        {
            if (settings == null || devicesList.Children.Count == 0)
                return;

            ClearSelection();

            for (int i = 0; i < devicesList.Children.Count; i++)
            {
                var item = (Grid)devicesList.Children[i];
                var data = (DeviceData)item.Tag;

                if (data.Name == settings.General.SelectedMIDIDeviceName &&
                    data.Id == settings.General.SelectedMIDIDevice)
                {
                    SelectDevice(i);
                    return;
                }
            }

            SelectDevice(0); // fallback
        }

        private void ClearSelection()
        {
            foreach (Grid item in devicesList.Children)
                item.Background = Brushes.Transparent;
        }
    }
}
