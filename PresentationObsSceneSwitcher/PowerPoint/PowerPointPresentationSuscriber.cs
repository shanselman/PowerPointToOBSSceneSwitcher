﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PresentationObsSceneSwitcher.PowerPoint
{
    /// <summary>
    /// Subscribes to a PowerPoint presentation.
    /// </summary>
    public class PowerPointPresentationSubscriber : IPresentationSubscriber
    {
        /// <summary>
        /// \( --> Literal
        ///     ((?<AppName>[^\)]+)\)    --> Avoid backtracking searching until a ) is found. No wildcard in regex please.
        /// \) --> Literal
        /// :  --> Literal
        /// \[
        ///     (?<Info>[^\]]+)         --> Avoid backtracking searching until a ] is found. No wildcard in regex please.
        /// \]
        ///
        /// Making it static and RegexOptions.Compiled is the best for Regex performance.
        /// </summary>
        private static readonly Regex extractInfoFromNotesRegex = new Regex(@"\((?<AppName>[^\)]+)\):\[(?<Info>[^\]]+)\]", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        //private static Application powerPoint = new Application();

        private readonly Dictionary<string, PresentationSuscription> suscriptions = new Dictionary<string, PresentationSuscription>();

        /// <summary>
        /// Constructor
        /// </summary>
        public PowerPointPresentationSubscriber()
        {
            PowerPointInterop.PowerPoint.SubscribeSlideShowNextSlide(async (string note) =>
           {
               foreach (Match match in extractInfoFromNotesRegex.Matches(note))
               {
                   string appName = match.Groups["AppName"].Value;
                   string info = match.Groups["Info"].Value;

                   if (suscriptions.TryGetValue(appName, out PresentationSuscription suscription))
                   {
                       await suscription(info).ConfigureAwait(false);
                   }
               }
           });
        }

        /// <summary>
        /// Subscribes an app to powerpoint slide changes
        /// </summary>
        /// <param name="appName">The app subscribing</param>
        /// <param name="suscription">The handler of the slide change</param>
        /// <returns></returns>
        public bool Subscribe(string appName, PresentationSuscription suscription) => suscriptions.TryAdd(appName, suscription);
    }
}
