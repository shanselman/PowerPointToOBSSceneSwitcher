using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Office.Interop.PowerPoint;
//Thanks to CSharpFritz and EngstromJimmy for their gists, snippets, and thoughts.

namespace PowerPointToOBSSceneSwitcher
{
    class Program
    {
        private static Application ppt = new Microsoft.Office.Interop.PowerPoint.Application();
        private static ObsLocal OBS;
        static async Task Main(string[] args)
        {
            Console.Write("Connecting to PowerPoint...");
            ppt.SlideShowNextSlide += App_SlideShowNextSlide;
            Console.WriteLine("connected");

            Console.Write("Connecting to OBS...");
            OBS = new ObsLocal();
            await OBS.Connect();
            Console.WriteLine("connected");

            OBS.GetScenes();

            Console.ReadLine();
        }

        async static void App_SlideShowNextSlide(SlideShowWindow Wn)
        {
            if (Wn != null)
            {
                Console.WriteLine($"Moved to Slide Number {Wn.View.Slide.SlideNumber}");
                //Text starts at Index 2 ¯\_(ツ)_/¯
                var note = String.Empty;
                try { note = Wn.View.Slide.NotesPage.Shapes[2].TextFrame.TextRange.Text; }
                catch { /*no notes*/ }

                bool sceneHandled = false;

                var splitnotes = note.Split('\r');

                foreach (var line in splitnotes)
                {
                    if (line.StartsWith("OBS:"))
                    {
                        var sceneLine = line.Substring(4);
                        Console.WriteLine($"  Switching to OBS Scene named \"{sceneLine}\"");
                        sceneHandled = OBS.ChangeScene(sceneLine);
                    }

                    if(line.StartsWith("OBSDEF:"))
                    {
                        OBS.DefaultScene = line.Substring(7);
                        Console.WriteLine($"  Setting the default OBS Scene to \"{OBS.DefaultScene}\"");
                    }

                    if (!sceneHandled)
                    {
                        OBS.ChangeScene(OBS.DefaultScene);
                        Console.WriteLine($"  Switching to OBS Default Scene named \"{OBS.DefaultScene}\"");

                    }
                }
            }
        }
    }
}