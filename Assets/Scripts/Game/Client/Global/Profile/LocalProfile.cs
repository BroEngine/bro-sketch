using System;
using System.Collections.Generic;
using Bro.Client.Json;
using Newtonsoft.Json;

namespace Game.Client
{
    /*
     *
     * time = seconds
     * timestamp = milliseconds
     * 
     */
    public class LocalProfile
    {
        [JsonProperty("settings")] public Settings SettingsData =  new Settings();
        [JsonProperty("user")] public User UserData = new User();
        [JsonProperty("campaign")] public Campaign CampaignData = new Campaign();
        
        [Serializable] [JsonType("campaign_data")]
        public class Campaign
        {
            [JsonProperty("last_completed_level")] public string LastCompletedLevelId;
            [JsonProperty("completed_levels")] public Dictionary<string, CampaignItem> CompletedLevels = new Dictionary<string, CampaignItem>();
        }
        
        [Serializable] [JsonType("campaign_item")]
        public class CampaignItem
        {
            // todo
        }
        
        [Serializable][JsonType("settings_data")]
        public class Settings
        {
            [JsonProperty("vibration")] public bool VibrationEnabled = true;
            [JsonProperty("music")] public bool MusicEnabled = true;
            [JsonProperty("sound")] public bool SoundEnabled = true;
            [JsonProperty("ads_enabled")] public bool AdsEnabled = true;
        } 
        
        [Serializable][JsonType("user")]
        public class User
        {
            [JsonProperty("is_initialized")] public bool IsInitialized;
            
            [JsonProperty("name")] public string Name;
            [JsonProperty("vehicle_id")] public VehicleId VehicleId;
            [JsonProperty("soft_currency")] public int SoftCurrency;
            [JsonProperty("hard_currency")] public int HardCurrency;
        }  
    }
}