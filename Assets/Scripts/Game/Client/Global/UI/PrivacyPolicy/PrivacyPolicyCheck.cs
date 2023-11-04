using Bro.Client.Context;
using Bro.Client.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Client.UI
{
    public class PrivacyPolicyCheck
    {
        private readonly UIModule _uiModule;
        
        public PrivacyPolicyCheck(IClientContext context)
        {
            _uiModule = context.GetGlobal<UIModule>();
        }

        public async UniTask Process()
        {
            if (!IsPrivacyPolicyAccepted)
            {
                await _uiModule.AwaitShow<PrivacyPolicyWindow>(new PrivacyPolicyWindowArgs(()=>
                {
                    IsPrivacyPolicyAccepted = true;
                }));

                var window = _uiModule.GetWindow<PrivacyPolicyWindow>();
                await UniTask.WaitUntil(() => window.CurrentState == Window.State.Closed);
            }
        }
        
        private static bool IsPrivacyPolicyAccepted
        {
            /* stored separately */
            get => PlayerPrefs.GetInt("privacy_policy_accepted", 0) == 1;
            set => PlayerPrefs.SetInt("privacy_policy_accepted", value ? 1 : 0);
        }
    }
}