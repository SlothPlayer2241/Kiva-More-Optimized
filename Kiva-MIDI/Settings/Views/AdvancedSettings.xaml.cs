using System;
using System.Windows;
using System.Windows.Controls;

namespace Kiva.Settings.Views
{
    public partial class AdvancedSettings : UserControl
    {
        private KivaSettings settings;
        private bool valuesSet;

        public KivaSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                SetValues();
            }
        }

        public AdvancedSettings()
        {
            InitializeComponent();
        }

        private bool Ready() => valuesSet && settings != null;

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

        private void disableTransparency_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.DisableTransparency = disableTransparency.IsChecked;
        }

        private void forceSingleThread_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.MultiThreadedRendering = !forceSingleThread.IsChecked;
        }

        private void threadCount_ValueChanged(object s, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!Ready()) return;
            settings.General.MaxRenderThreads = (int)threadCount.Value;
        }

        private void audioBufferSize_ValueChanged(object s, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!Ready()) return;
            settings.General.AudioBufferSize = (int)audioBufferSize.Value;
        }

        private void maxRenderedNotes_ValueChanged(object s, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!Ready()) return;
            settings.General.MaxRenderedNotes = (int)maxRenderedNotes.Value;
        }

        private void performanceMode_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;

            bool enabled = performanceMode.IsChecked;
            settings.General.PerformanceMode = enabled;

            if (enabled)
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

        private void enableVisualEffects_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.EnableVisualEffects = enableVisualEffects.IsChecked;
        }

        private void enableCulling_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.EnableCulling = enableCulling.IsChecked;
        }

        private void enableBatching_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.EnableNoteBatching = enableBatching.IsChecked;
        }

        private void enableAutoScaling_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.EnableAutoScaling = enableAutoScaling.IsChecked;
        }

        private void autoScalingMinFps_ValueChanged(object s, RoutedPropertyChangedEventArgs<decimal> e)
        {
            if (!Ready()) return;
            settings.General.AutoScalingMinFps = (int)autoScalingMinFps.Value;
        }

        private void enableMidiCaching_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.EnableMidiCaching = enableMidiCaching.IsChecked;
        }

        private void lowQualityAudio_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.LowQualityAudio = lowQualityAudio.IsChecked;
        }

        private void lazyLoadSoundfont_CheckToggled(object s, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (!Ready()) return;
            settings.General.LazyLoadSoundfont = lazyLoadSoundfont.IsChecked;
        }
    }
}
