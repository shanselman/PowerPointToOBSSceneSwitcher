using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Threading.Tasks;

namespace PowerPointInterop
{
    /// <summary>
    /// This projects allows you to compile a .dll with an embed COM Reference. 
    /// Used to add compilation support using dotnet cli as it doesn't allow you to use COMReference.
    /// </summary>
    public static class PowerPoint
    {
        private static Application powerPoint = new Application();

        public static void SubscribeSlideShowNextSlide(Func<string, Task> subscription)
        {
            powerPoint.SlideShowNextSlide += async (SlideShowWindow Wn) =>
            {
                var command = String.Empty;
                try { command = Wn.View.Slide.NotesPage.Shapes[2].TextFrame.TextRange.Text; }
                catch { /*no notes*/ }

                await subscription(command);
            };
        }
    }
}
