using Microsoft.Office.Interop.PowerPoint;
using PowerPointToOBSSceneSwitcher.OBS;
using PowerPointToOBSSceneSwitcher.SceneManagers;
using System;
using System.Threading.Tasks;

namespace PowerPointToOBSSceneSwitcher
{
    public class PowerPointSwitcherController
    {
        private readonly Application _ppt;
        private readonly IOBSManager _obsManager;

        public PowerPointSwitcherController(IOBSManager obsManager)
        {
            _ppt = new Application();
            _obsManager = obsManager;
        }

        public async Task Connect()
        {
            Console.Write("Connecting to PowerPoint...");
            _ppt.SlideShowNextSlide += App_SlideShowNextSlide;
            Console.WriteLine("connected");

            Console.Write("Connecting to OBS...");
            await _obsManager.Connect();
            Console.WriteLine("connected");
            _obsManager.Init();
        }

        private void App_SlideShowNextSlide(SlideShowWindow Wn)
        {
            if (Wn != null)
            {
                Console.WriteLine($"Moved to Slide Number {Wn.View.Slide.SlideNumber}");
                //Text starts at Index 2 ¯\_(ツ)_/¯
                var note = String.Empty;
                try { note = Wn.View.Slide.NotesPage.Shapes[2].TextFrame.TextRange.Text; }
                catch { /*no notes*/ }

                ISceneManager reader = new PowerpointNotesSceneManager(_obsManager);
                reader.ProcessData(note);
            }
        }
    }
}
