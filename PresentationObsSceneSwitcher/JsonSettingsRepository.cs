using Newtonsoft.Json;
using PowerPointToOBSSceneSwitcher.Obs;
using System.IO;
using System.Threading.Tasks;

namespace PresentationObsSceneSwitcher
{
    public class JsonSettingsRepository
    {
        public async Task SaveAsync(ObsWebSocketClientSettings settings)
        {
            string json = JsonConvert.SerializeObject(settings);
            await File.WriteAllTextAsync(@".\settings.json", json).ConfigureAwait(false); // Maybe AppData is a better location, but...
        }

        public async Task<ObsWebSocketClientSettings> LoadAsync()
        {
            try
            {
                string json = await File.ReadAllTextAsync(@".\settings.json").ConfigureAwait(false);

                return JsonConvert.DeserializeObject<ObsWebSocketClientSettings>(json);
            }
            catch (FileNotFoundException ex)
            {
                return null;
            }
        }
    }
}
