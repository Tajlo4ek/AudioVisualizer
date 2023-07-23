using System;
using System.Runtime.InteropServices;
using static Common.NativeConstants;
using static Common.NativeMethods;


namespace Common
{
    static class WindowUtils
    {
        public static void ShowBehindDesktop(IntPtr handle) {
            //var hwndSource = HwndSource.FromHwnd(handle);
            //hwndSource.AddHook(WindowProc);
            SetStyles(handle);
            EnableNoActive(handle, true);
            if (Environment.OSVersion.Version.Major < 6 || (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor < 2))
            {
                ShowAlwaysBehindDesktopBeforeWindows8(handle);
            }
            else
            {
                ShowAlwaysBehindDesktop(handle);
            }
        }

        private static IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeConstants.WM_WINDOWPOSCHANGING)
            {
                var windowPos = (WindowPos)Marshal.PtrToStructure(lParam, typeof(WindowPos));
                windowPos.hwndInsertAfter = new IntPtr(NativeConstants.HWND_BOTTOM);
                windowPos.flags &= ~(uint)NativeConstants.SWP_NOZORDER;
                handled = true;
            }
            return IntPtr.Zero;
        }

        private static void ShowAlwaysBehindDesktopBeforeWindows8(IntPtr hwnd)
        {
            var progmanHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);
            var shellHandle = FindWindowEx(progmanHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            if (shellHandle == IntPtr.Zero)
            {
                // Send 0x052C to Progman. This message directs Progman to spawn a 
                // WorkerW behind the desktop icons. If it is already there, nothing 
                // happens.
                var desktopHandle = GetDesktopWindow();
                var workerWHandle = IntPtr.Zero;
                do
                {
                    workerWHandle = FindWindowEx(desktopHandle, workerWHandle, "WorkerW", null);
                    shellHandle = FindWindowEx(workerWHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
                } while (shellHandle == IntPtr.Zero && workerWHandle != IntPtr.Zero);
            }
            if (shellHandle != IntPtr.Zero)
            {
                var sysListHandle = FindWindowEx(shellHandle, IntPtr.Zero, "SysListView32", null);
                if (sysListHandle != IntPtr.Zero)
                {
                    shellHandle = sysListHandle;
                }
            }
            SetParent(hwnd, shellHandle);
        }

        private static void ShowAlwaysBehindDesktop(IntPtr hwnd)
        {
            var progmanHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);
            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            //SendMessageTimeout(progmanHandle, 0x052C, new IntPtr(0), IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, 1000, out var result);
            SendMessage(progmanHandle, 0x052C, (IntPtr)0x0000000D, (IntPtr)0);
            SendMessage(progmanHandle, 0x052C, (IntPtr)0x0000000D, (IntPtr)1);
            var workerWHandle = IntPtr.Zero;
            // We enumerate all Windows, until we find one, that has the SHELLDLL_DefView 
            // as a child. 
            // If we found that window, we take its next sibling and assign it to workerw.
            EnumWindows(new EnumWindowsProc((topHandle, topParamHandle) =>
            {
                IntPtr shellHandle = FindWindowEx(topHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (shellHandle != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerWHandle = FindWindowEx(IntPtr.Zero, topHandle, "WorkerW", null);
                }

                return true;
            }), IntPtr.Zero);
            workerWHandle = workerWHandle == IntPtr.Zero ? progmanHandle : workerWHandle;
            SetParent(hwnd, workerWHandle);
        }

        private static void SetStyles(IntPtr hwnd)
        {
            var exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            exStyle |= WS_EX_TOOLWINDOW;
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
            SetWindowPos(hwnd, new IntPtr(HWND_BOTTOM), 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);
        }

        private static void EnableNoActive(IntPtr hwnd, bool enable)
        {
            var exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            if (enable)
            {
                exStyle |= WS_EX_NOACTIVATE;
            }
            else
            {
                exStyle &= ~WS_EX_NOACTIVATE;
            }
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
        }
    }
}