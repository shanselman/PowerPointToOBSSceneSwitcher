using System;
using System.Threading.Tasks;

namespace PowerPointToOBSSceneSwitcher.OBS
{
    public interface IOBSManager : IDisposable
    {
        public String DefaultScene { get; set; }
        Task Connect();
        void Init();
        bool ChangeScene(string sceneName);
        void StartRecording();
        void StopRecording();
    }
}
