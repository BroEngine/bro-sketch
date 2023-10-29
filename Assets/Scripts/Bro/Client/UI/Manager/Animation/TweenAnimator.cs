using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bro.Client.UI
{
    public abstract class TweenAnimator : MonoBehaviour
    {
        public abstract UniTask Show();
        public abstract UniTask Hide();
    }
}