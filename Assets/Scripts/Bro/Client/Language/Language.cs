using System.ComponentModel;

namespace Bro.Client
{
    public enum Language
    {
        [Description("--")] Unknown = 0,
        [Description("en")] English,
        [Description("ru")] Russian,
        [Description("ua")] Ukrainian,
        [Description("es")] Spanish,
        [Description("it")] Italian,
        [Description("pt")] Portuguese,
        [Description("de")] German,
        [Description("pl")] Polish,
        [Description("cz")] Czech,
        [Description("tr")] Turkish,
        [Description("gr")] Greek,
        [Description("fr")] French,
        [Description("ch")] Chinese
    }

    public static class LanguageExtension
    {
        public static string GetIsoCode(this Language language)
        {
            return language.GetAttributeOfType<DescriptionAttribute>().Description;
        }
    }
}