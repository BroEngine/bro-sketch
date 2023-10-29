using System;
using Newtonsoft.Json;

namespace Bro.Client
{
    [Serializable]
    public struct Range<T>
    {
        [JsonProperty("min")] public T Min;
        [JsonProperty("max")] public T Max;

        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }
        
        public static Range<T> Default()
        {
            return new Range<T>();
        }

        public override string ToString()
        {
            return $"[{Min}...{Max}]";
        }
    }
}