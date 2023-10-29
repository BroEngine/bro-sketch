namespace Bro.Client.UI
{
    public static class ColorExtension
    {
        public static UnityEngine.Color SetAlpha(this UnityEngine.Color color, float a)
        {
            return new UnityEngine.Color(color.r, color.g, color.b, a);
        }
    }
}
