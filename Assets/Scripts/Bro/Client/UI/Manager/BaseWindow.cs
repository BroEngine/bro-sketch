#if ( UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE || UNITY_XBOXONE || UNITY_PS4 || UNITY_WEBGL || UNITY_WII ||CONSOLE_CLIENT )

using Bro.Client.Context;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bro.Client.UI
{
    public abstract class BaseWindow : MonoBehaviour
    {
        protected abstract void OnShow(IWindowArgs args);

        protected virtual void OnSetup(IClientContext context)
        {
        }
        
        protected virtual async UniTask OnHideStart()
        {
        }
        
        protected virtual async UniTask OnHideEnd()
        {
        } 
        
        protected virtual async UniTask OnShown()
        {
        }
    }
}

#endif