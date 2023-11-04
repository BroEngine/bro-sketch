using System;

namespace Game.Client
{
    public static class AssemblyInfo
    {
        public static string Version => UnityEngine.Application.version;

        public static long VersionCode
        {
            get
            {
                var version = UnityEngine.Application.version;
                return ConvertVersion(version);
            }
        }

        private static long ConvertVersion(string version)
        {
            long result = 0;
            var offset = 10000;
            var data = version.Split('.');
            var count = data.Length;
            for (var i = count - 1; i >= 0; --i)
            {
                var integer = 0;
                var power = (count - i - 1);
                try
                {
                    integer = int.Parse(data[i]);
                }
                catch (Exception) { /* ignored */ }
                result += integer * (long)Math.Pow(offset, power);
            }
            return result;
        }
    }
}