using Bro.Client.Context;

namespace Game.Client.Entry
{
    public class EntryContext : ClientContext
    {
        private readonly LoadingModule _loadingModule = new LoadingModule();
        
        public EntryContext(IApplication application) : base(application)
        {

        }
    }
}