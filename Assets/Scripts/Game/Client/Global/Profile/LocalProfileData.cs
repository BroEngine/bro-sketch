namespace Game.Client
{
    public abstract class LocalProfileData
    {
        private readonly LocalProfileModule _profile;
        
        protected LocalProfileData(LocalProfileModule profile)
        {
            _profile = profile;
        }

        public abstract void Load();
        
        public virtual void OnSave()
        {
            
        }
        
        protected void Save(bool force = false)
        {
            _profile.Save(force);
        }
    }
}