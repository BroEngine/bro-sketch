namespace Bro.Client.Context
{
    public class ApplicationFocusEvent : Event
    {
        public readonly bool IsFocus;
   
        public ApplicationFocusEvent(bool isFocus)
        {
            IsFocus = isFocus;
        }
    }
}