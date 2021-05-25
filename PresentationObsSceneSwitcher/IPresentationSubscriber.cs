using System.Threading.Tasks;

namespace PresentationObsSceneSwitcher
{
    /// <summary>
    /// Receives the name of the OBS scene.
    /// </summary>
    /// <param name="scene"></param>
    public delegate Task PresentationSuscription(string scene);

    /// <summary>
    /// Manages the suscriptions to a Presetation
    /// </summary>
    public interface IPresentationSubscriber
    {
        bool Subscribe(string appName, PresentationSuscription suscription);
    }
}
