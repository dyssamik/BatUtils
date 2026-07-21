using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Input;
using BatUtils.Models;
using BatUtils.Views;

namespace BatUtils
{
    public partial class MainWindow : Window
    {
        private const int WM_GETMINMAXINFO = 0x0024;
        private const uint MONITOR_DEFAULTTONEAREST = 2;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private static readonly IntPtr HTCAPTION = (IntPtr)2;

        private readonly ObservableCollection<Client> _clients =
            new ObservableCollection<Client>
            {
                new Client { Code = "1", Name = "cli_1" },
                new Client { Code = "2", Name = "cli_2" }
            };

        private readonly ClientsView _clientsView;

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(
            IntPtr hwnd,
            uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetMonitorInfo(
            IntPtr hMonitor,
            ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(
            IntPtr hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam);

        public MainWindow()
        {
            InitializeComponent();

            StateChanged += MainWindow_StateChanged;

            _clientsView = new ClientsView(_clients);

            MainContent.Content = _clientsView;
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = Marshal.PtrToStructure<MINMAXINFO>(lParam);

            IntPtr monitor = MonitorFromWindow(
                hwnd,
                MONITOR_DEFAULTTONEAREST);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MONITORINFO
                {
                    cbSize = Marshal.SizeOf(typeof(MONITORINFO))
                };

                if (GetMonitorInfo(monitor, ref monitorInfo))
                {
                    RECT workArea = monitorInfo.rcWork;
                    RECT monitorArea = monitorInfo.rcMonitor;

                    mmi.ptMaxPosition.X = workArea.Left - monitorArea.Left;
                    mmi.ptMaxPosition.Y = workArea.Top - monitorArea.Top;

                    mmi.ptMaxSize.X = workArea.Right - workArea.Left;
                    mmi.ptMaxSize.Y = workArea.Bottom - workArea.Top;
                }
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }

        private IntPtr WndProc(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            ref bool handled)
        {
            if (msg == WM_GETMINMAXINFO)
            {
                WmGetMinMaxInfo(hwnd, lParam);
                handled = true;
            }

            return IntPtr.Zero;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var source = (HwndSource)PresentationSource.FromVisual(this);
            source.AddHook(WndProc);
        }

        private void MainWindow_StateChanged(object sender, EventArgs e)
        {
            TitleBar.SetMaximized(WindowState == WindowState.Maximized);
        }

        private void TitleBar_MinimizeClicked(object sender, EventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void TitleBar_MaximizeClicked(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                SystemCommands.RestoreWindow(this);
            else
                SystemCommands.MaximizeWindow(this);
        }

        private void TitleBar_CloseClicked(object sender, EventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void TitleBar_DragRequested(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Maximized)
                    SystemCommands.RestoreWindow(this);
                else
                    SystemCommands.MaximizeWindow(this);

                return;
            }

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (WindowState == WindowState.Maximized)
            {
                // Position of the mouse within the title bar
                Point mouse = e.GetPosition(TitleBar);

                // Keep approximately the same relative horizontal position
                double percent = mouse.X / TitleBar.ActualWidth;

                double restoredWidth = RestoreBounds.Width;

                SystemCommands.RestoreWindow(this);

                Left = Mouse.GetPosition(null).X - restoredWidth * percent;
                Top = 0;
            }

            ReleaseCapture();

            SendMessage(
                new WindowInteropHelper(this).Handle,
                WM_NCLBUTTONDOWN,
                HTCAPTION,
                IntPtr.Zero);
        }
    }
}