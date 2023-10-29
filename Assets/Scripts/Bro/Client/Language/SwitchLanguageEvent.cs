namespace Bro.Client
{
    public class SwitchLanguageEvent : Event
    {
        public readonly Language Language;

        public SwitchLanguageEvent(Language newLanguage)
        {
            Language = newLanguage;
        }
    }
}