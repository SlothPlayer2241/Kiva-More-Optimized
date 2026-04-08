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
    public partial class AdvancedSettings : UserControl
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

        public AdvancedSettings()
        {
            InitializeComponent();
        }

        bool valuesSet = false;

        public void SetValues()
        {
            threadCount.Maximum = Environment.ProcessorCount;
            threadCount.Value = settings.General.MaxRenderThreads;
            forceSingleThread.IsChecked = !settings.General.MultiThreadedRendering;
            disableTransparency.IsChecked = settings.General.DisableTransparency;
            audioBufferSize.Value = settings.General.AudioBufferSize;
            maxRenderedNotes.Value = settings.General.MaxRenderedNotes;
            performanceMode.IsChecked = settings.General.PerformanceMode;
            enableVisualEffects.IsChecked = settings.General.EnableVisualEffects;
            enableCulling.IsChecked = settings.General.EnableCulling;
            enableBatching.IsChecked = settings.General.EnableNoteBatching;
            enableAutoScaling.IsChecked = settings.General.EnableAutoScaling;
            autoScalingMinFps.Value = settings.General.AutoScalingMinFps;
            enableMidiCaching.IsChecked = settings.General.EnableMidiCaching;
            lowQualityAudio.IsChecked = settings.General.LowQualityAudio;
            lazyLoadSoundfont.IsChecked = settings.General.LazyLoadSoundfont;
            valuesSet = true;
        }

        private void disableTransparency_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.DisableTransparency = disableTransparency.IsChecked;
        }

        private void forceSingleThread_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.MultiThreadedRendering = !forceSingleThread.IsChecked;
        }

        private void threadCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!valuesSet) return;
            settings.General.MaxRenderThreads = (int)threadCount.Value;
        }

        private void audioBufferSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!valuesSet) return;
            settings.General.AudioBufferSize = (int)audioBufferSize.Value;
        }

        private void maxRenderedNotes_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!valuesSet) return;
            settings.General.MaxRenderedNotes = (int)maxRenderedNotes.Value;
        }

        private void performanceMode_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.PerformanceMode = performanceMode.IsChecked;
            if (performanceMode.IsChecked)
            {
                settings.General.EnableVisualEffects = false;
                settings.General.EnableCulling = true;
                settings.General.EnableNoteBatching = true;
                settings.General.MultiThreadedRendering = true;
                settings.General.MaxRenderThreads = Environment.ProcessorCount;
                enableVisualEffects.IsChecked = false;
                forceSingleThread.IsChecked = false;
                threadCount.Value = Environment.ProcessorCount;
            }
        }

        private void enableVisualEffects_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.EnableVisualEffects = enableVisualEffects.IsChecked;
        }

        private void enableCulling_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.EnableCulling = enableCulling.IsChecked;
        }

        private void enableBatching_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.EnableNoteBatching = enableBatching.IsChecked;
        }

        private void enableAutoScaling_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.EnableAutoScaling = enableAutoScaling.IsChecked;
        }

        private void autoScalingMinFps_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!valuesSet) return;
            settings.General.AutoScalingMinFps = (int)autoScalingMinFps.Value;
        }

        private void enableMidiCaching_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.EnableMidiCaching = enableMidiCaching.IsChecked;
        }

        private void lowQualityAudio_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.LowQualityAudio = lowQualityAudio.IsChecked;
        }

        private void lazyLoadSoundfont_CheckToggled(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!valuesSet) return;
            settings.General.LazyLoadSoundfont = lazyLoadSoundfont.IsChecked;
        }
    }
}