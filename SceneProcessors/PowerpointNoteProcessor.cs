using PowerPointToOBSSceneSwitcher.OBS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PowerPointToOBSSceneSwitcher.SceneProcessors
{
    class PowerpointNoteProcessor : ISceneDataProcessor
    {
        private readonly IOBSManager _obs;

        public PowerpointNoteProcessor(IOBSManager obs)
        {
            _obs = obs;
        }
        public void ProcessData(string data)
        {
            var notereader = new StringReader(data);
            bool sceneHandled = false;
            string line;

            while ((line = notereader.ReadLine()) != null)
            {
                if (line.StartsWith("OBS:"))
                {
                    line = line.Substring(4).Trim();
                    sceneHandled = HandleChangeScene(line, sceneHandled);
                }

                if (line.StartsWith("OBSDEF:"))
                {
                    _obs.DefaultScene = line.Substring(7).Trim();
                }

                if (line.StartsWith("**START"))
                {
                    _obs.StartRecording();
                }

                if (line.StartsWith("**STOP"))
                {
                    _obs.StopRecording();
                }

                if (!sceneHandled)
                {
                    Console.WriteLine($"  Switching to OBS Default Scene named \"{_obs.DefaultScene}\"");
                    _obs.ChangeScene(_obs.DefaultScene);
                }
            }
        }

        private bool HandleChangeScene(string sceneName, bool sceneHandled)
        {
            if (!sceneHandled)
            {
                try
                {
                    sceneHandled = _obs.ChangeScene(sceneName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ERROR: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine($"  WARNING: Multiple scene definitions found.  I used the first and have ignored \"{sceneName}\"");
            }

            return sceneHandled;
        }
    }
}
