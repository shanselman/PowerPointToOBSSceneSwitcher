using System;
using System.Runtime.InteropServices;

namespace PresentationObsSceneSwitcherConsole
{
    public class ConsoleWindowController
    {
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static void Show()
        {
            ShowWindow(GetConsoleWindow(), SW_SHOW);
        }

        public static void Hide()
        {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }
    }
}
