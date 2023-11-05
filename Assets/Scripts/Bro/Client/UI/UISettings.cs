using UnityEngine;

namespace Bro.Client.UI
{
    public class UISettings
    {
        public readonly Vector2 Resolution = new Vector2(2560, 1440);
        public readonly float MatchFactor = 1.0f;

        public UISettings()
        {
            /* default */
        }

        public UISettings(Vector2 resolution, float matchFactor = 1.0f)
        {
            Resolution = resolution;
            MatchFactor = matchFactor;
        }
        
        public UISettings(int width, int height, float matchFactor = 1.0f)
        {
            Resolution = new Vector2(width, height);
            MatchFactor = matchFactor;
        }
    }
}