using System;

namespace Bro.Client
{
    public static class LanguageTool
    {
        public static Language SystemLanguage
        {
            get
            {
                var systemLanguage = UnityEngine.SystemLanguage.English;

                try
                {
                    systemLanguage = UnityEngine.Application.systemLanguage;
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError("language provider :: " + e);
                }

                switch (systemLanguage)
                {
                    case UnityEngine.SystemLanguage.Spanish: 
                        return Language.Spanish;
                    case UnityEngine.SystemLanguage.Portuguese:
                        return Language.Portuguese;
                    case UnityEngine.SystemLanguage.Russian:
                    case UnityEngine.SystemLanguage.Belarusian:
                        return Language.Russian;
                    case UnityEngine.SystemLanguage.Ukrainian:
                        return Language.Ukrainian;
                    case UnityEngine.SystemLanguage.Polish: 
                        return Language.Polish;
                    case UnityEngine.SystemLanguage.French: 
                        return Language.French;
                    case UnityEngine.SystemLanguage.German: 
                        return Language.German;
                    case UnityEngine.SystemLanguage.Czech:
                        return Language.Czech;
                    case UnityEngine.SystemLanguage.Greek: 
                        return Language.Greek;
                    case UnityEngine.SystemLanguage.Italian:
                        return Language.Italian;
                    case UnityEngine.SystemLanguage.Chinese:
                    case UnityEngine.SystemLanguage.ChineseSimplified:
                        return Language.Chinese;
                    case UnityEngine.SystemLanguage.Turkish: 
                        return Language.Turkish;
                    default:
                        return Language.English;
                }
            }
        }
    }
}