using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

namespace Kiva.Settings.Views
{
    public partial class SettingsWindow : Window
    {
        private KivaSettings settings;

        public SettingsWindow(KivaSettings settings)
        {
            this.settings = settings;

            InitializeComponent();

            SourceInitialized += (s, e) =>
            {
                var handle = new WindowInteropHelper(this).Handle;
                HwndSource.FromHwnd(handle)?.AddHook(WindowProc);
            };

            visualSettings.Settings = settings;
            audioSettings.Settings = settings;
            miscSettings.Settings = settings;
            advancedSettings.Settings = settings;

            var selectedBrush = new SolidColorBrush(Color.FromArgb(50, 255, 255, 255));

            SelectTab(0, selectedBrush);

            for (int i = 0; i < tabPanel.Children.Count; i++)
            {
                int index = i;
                if (tabPanel.Children[i] is Button btn)
                {
                    btn.PreviewMouseDown += (s, e) => SelectTab(index, selectedBrush);
                }
            }
        }

        private void SelectTab(int index, Brush selectedBrush)
        {
            for (int i = 0; i < tabPanel.Children.Count; i++)
            {
                if (tabPanel.Children[i] is Button btn)
                    btn.Background = Brushes.Transparent;

                if (i < content.Children.Count)
                    content.Children[i].Visibility = Visibility.Collapsed;
            }

            if (tabPanel.Children[index] is Button selectedBtn)
                selectedBtn.Background = selectedBrush;

            if (index < content.Children.Count)
                content.Children[index].Visibility = Visibility.Visible;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Window Chrome Fix (required for proper maximize behavior)

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0024) // WM_GETMINMAXINFO
            {
                WmGetMinMaxInfo(hwnd, lParam);
                handled = true;
            }
            return IntPtr.Zero;
        }

        private static void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = Marshal.PtrToStructure<MINMAXINFO>(lParam);

            IntPtr monitor = MonitorFromWindow(hwnd, 0x00000002);
            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);

                var work = monitorInfo.rcWork;
                var monitorArea = monitorInfo.rcMonitor;

                mmi.ptMaxPosition.x = Math.Abs(work.left - monitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(work.top - monitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(work.right - work.left);
                mmi.ptMaxSize.y = Math.Abs(work.bottom - work.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int x, y; }

        [StructLayout(LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public POINT ptReserved, ptMaxSize, ptMaxPosition, ptMinTrackSize, ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf<MONITORINFO>();
            public RECT rcMonitor = new RECT();
            public RECT rcWork = new RECT();
            public int dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left, top, right, bottom;
        }

        [DllImport("user32")]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("user32")]
        private static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        #endregion
    }
}
