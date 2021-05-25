using OBS.WebSocket.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerPointToOBSSceneSwitcher.OBS
{
    public class OBSManager : IOBSManager
    {
        private ObsWebSocket _OBS;
        private List<string> _validScenes;
        private bool _DisposedValue;
        private string _defaultScene;
        private string _currentScene;

        public OBSManager(){ }

        public string DefaultScene
        {
            get { return _defaultScene; }
            set
            {
                if (_validScenes.Contains(value))
                {
                    _defaultScene = value;
                    Console.WriteLine($"  Setting the default OBS Scene to \"{value}\"");
                }
                else
                {
                    Console.WriteLine($"Scene named {value} does not exist and cannot be set as default");
                }
            }
        }

        public Task Connect()
        {
            _OBS = new ObsWebSocket();
            _OBS.Connect($"ws://127.0.0.1:4444", "");
            return Task.CompletedTask;
        }

        public void Init()
        {
            GetScenes();
        }

        private void GetScenes()
        {
            var allScene = _OBS.Api.GetSceneList();
            var list = allScene.Scenes.Select(s => s.Name).ToList();
            Console.WriteLine("Valid Scenes:");
            foreach (var l in list)
            {
                Console.WriteLine(l);
            }
            _validScenes = list;
            _defaultScene = _validScenes.FirstOrDefault();
        }

        public bool ChangeScene(string sceneName)
        {
            if (!_validScenes.Contains(sceneName))
            {
                Console.WriteLine($"Scene named {sceneName} does not exist");
                if (String.IsNullOrEmpty(_defaultScene))
                {
                    Console.WriteLine("No default scene has been set!");
                    return false;
                }

                sceneName = _defaultScene;
            }

            if(_currentScene != sceneName)
            {
                Console.WriteLine($"  Switching to OBS Scene named \"{sceneName}\"");
                _OBS.Api.SetCurrentScene(sceneName);
                _currentScene = sceneName;
            }

            return true;
        }

        public void StartRecording()
        {
            try 
            { 
                _OBS.Api.StartRecording();
                Console.WriteLine($"Recording Started");
            }
            catch {  /* Recording already started */ }
        }

        public void StopRecording()
        {
            try 
            { 
                _OBS.Api.StopRecording();
                Console.WriteLine($"Recording Stopped");
            }
            catch {  /* Recording already stopped */ }
        }

        ~OBSManager()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_DisposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                _OBS.Disconnect();
                _OBS = null;
                _DisposedValue = true;
            }
        }
    }
}
