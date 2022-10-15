using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Office.Interop.PowerPoint;
using PowerPointToOBSSceneSwitcher.OBS;
using PowerPointToOBSSceneSwitcher.SceneManagers;
//Thanks to CSharpFritz and EngstromJimmy for their gists, snippets, and thoughts.

namespace PowerPointToOBSSceneSwitcher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IOBSManager OBS = new OBSManager();

            PowerPointSwitcherController controller = new PowerPointSwitcherController(OBS);
            await controller.Connect();

            Console.ReadLine();
        }
    }
}