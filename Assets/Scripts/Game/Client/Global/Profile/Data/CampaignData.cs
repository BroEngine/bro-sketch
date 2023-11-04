using System.Collections.Generic;
using System.Linq;

namespace Game.Client
{
    public class CampaignData : LocalProfileData
    {
        private readonly LocalProfileModule _module;
        private readonly LocalProfile.Campaign _data;

        public string LastCompletedLevelId => _data.LastCompletedLevelId;
        public IReadOnlyList<string> CompletedLevels => _data.CompletedLevels.Keys.ToList(); // copy
        
        public CampaignData(LocalProfile.Campaign data, LocalProfileModule module) : base(module)
        {
            _data = data;
            _module = module;
        }
        
        public override void Load()
        {
            
        }

        public void SetProgress(string levelId /*, data*/)
        {
            if (!_data.CompletedLevels.ContainsKey(levelId) || string.IsNullOrEmpty(_data.LastCompletedLevelId))
            {
                _data.LastCompletedLevelId = levelId;
            }

            if (!_data.CompletedLevels.ContainsKey(levelId))
            {
                _data.CompletedLevels[levelId] = new LocalProfile.CampaignItem();
            }

            // update data
            
            Save(true);
        }
        
        public bool IsLevelCompleted(string levelId)
        {
            return _data.CompletedLevels.ContainsKey(levelId);
        }

        public LocalProfile.CampaignItem GetLevelProgress(string levelId)
        { 
            if (_data.CompletedLevels.ContainsKey(levelId))
            {
                return _data.CompletedLevels[levelId];
            }
            return null;
        }
    }
}