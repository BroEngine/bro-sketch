using System.Diagnostics;

namespace Bro
{
    public static class StopwatchExtensions
    {
        public static float ElapsedSeconds(this Stopwatch stopwatch)
        {
            return ((float)stopwatch.ElapsedMilliseconds) / 1000f;
        }
    }
}