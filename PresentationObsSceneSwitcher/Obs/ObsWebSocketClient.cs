using OBS.WebSocket.NET;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace PowerPointToOBSSceneSwitcher.Obs
{
    /// <summary>
    /// Defines all the settings a ObsWebSocketClient needs
    /// </summary>
    /// Configuring the ObsWebSocketClient with a Settings object allows to use it with DI
    public class ObsWebSocketClientSettings
    {
        private string ipAddress = "127.0.0.1";

        /// <summary>
        /// Ip address of the obs websocket server
        /// </summary>
        public string IpAddress
        {
            get => ipAddress;
            set => this.ipAddress = IPAddress.TryParse(value, out _) ? value
                    : throw new ValidationException($"The field {nameof(IpAddress)} should be a valid IP");
        }

        /// <summary>
        /// Port of the OBS WebSocket server
        /// </summary>
        public int Port { get; set; } = 4444;

        /// <summary>
        /// Optional Password of the OBS WebSocket server
        /// </summary>
        public string Password { get; set; } = "";
    }

    /// <summary>
    /// Connects to OBS WebSocket server
    /// </summary>
    public class ObsWebSocketClient : IDisposable
    {
        private bool disposedValue;
        private ObsWebSocket obsWebSocket;

        public bool IsConnected => obsWebSocket.IsConnected;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Settings to connect</param>
        public ObsWebSocketClient()
        {
            this.obsWebSocket = new ObsWebSocket(); // Always not null
        }

        /// <summary>
        /// Connects to the OBS WebSocket server
        /// </summary>
        /// <returns></returns>
        public void Connect(ObsWebSocketClientSettings settings)
        {
            if (obsWebSocket.IsConnected)
                obsWebSocket.Disconnect();

            obsWebSocket.Connect($"ws://{settings.IpAddress}:{settings.Port}", settings.Password ?? "");
        }

        /// <summary>
        /// Connects to the OBS WebSocket server
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync(ObsWebSocketClientSettings settings)
        {
            // At least this won't block the main thread if using from GUI
            if (!obsWebSocket.IsConnected)
                await Task.WhenAny(Task.Run(() => obsWebSocket.Connect($"ws://{settings.IpAddress}:{settings.Port}", "")), Task.Delay(3000));
        }

        /// <summary>
        /// Changes OBS current scene
        /// </summary>
        /// <param name="scene">The Scene name in OBS</param>
        /// <returns>Always true</returns>
        public bool ChangeScene(string scene)
        {
            obsWebSocket.Api.SetCurrentScene(scene);
            return true;
        }

        /// <summary>
        /// Changes OBS current scene
        /// </summary>
        /// <param name="scene">The Scene name in OBS</param>
        /// <returns>Always true</returns>
        public async Task<bool> ChangeSceneAsync(string scene)
        {
            await Task.Run(() => obsWebSocket.Api.SetCurrentScene(scene));
            return true;
        }

        /// <summary>
        /// Starts recording OBS
        /// </summary>
        /// <returns>Always true</returns>
        public bool StartRecording()
        {
            obsWebSocket.Api.StartRecording();
            return true;
        }

        /// <summary>
        /// Starts recording OBS
        /// </summary>
        /// <returns>Always true</returns>
        public async Task<bool> StartRecordingAsync()
        {
            await Task.Run(() => obsWebSocket.Api.StartRecording());
            return true;
        }

        /// <summary>
        /// Starts recording OBS
        /// </summary>
        /// <returns>Always true</returns>
        public bool StopRecording()
        {
            obsWebSocket.Api.StopRecording();
            return true;
        }

        /// <summary>
        /// Starts recording OBS
        /// </summary>
        /// <returns>Always true</returns>
        public async Task<bool> StopRecordingAsync()
        {
            await Task.Run(() => obsWebSocket.Api.StopRecording());
            return true;
        }

        #region Dispose

        ~ObsWebSocketClient() => Dispose(disposing: false);

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                obsWebSocket.Disconnect();
                obsWebSocket = null;
                disposedValue = true;
            }
        }

        #endregion
    }
}