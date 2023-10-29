using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bro.Client.UI
{
    public class ExtendedButton : Button
    {
        [SerializeField] protected bool _invokeAudioEvent = true;
        [SerializeField] protected float _animationScale = 1.05f;
        
        private bool _lastValue = true;
        private bool _isChanged = false;

        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(OnButton);
        }

        private void OnButton()
        {
            if (_invokeAudioEvent)
            {
                // new UIAudioEvent(UIAudioType.ButtonPress).Launch();;
            }
        }

        private void Update()
        {
            if (!ExtendedButtonState.Enabled)
            {
                if (!_isChanged)
                {
                    _isChanged = true;
                    _lastValue = interactable;
                    interactable = false;
                }
            }
            else
            {
                if (_isChanged)
                {
                    interactable = _lastValue;
                    _isChanged = false;
                }
            }
        }
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            AnimateOn();
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            AnimateOff();
            base.OnPointerUp(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            AnimateOff();
            base.OnPointerExit(eventData);
        }

        private void AnimateOn()
        {
             targetGraphic.gameObject.transform.localScale = new Vector3(_animationScale,_animationScale,_animationScale);
        }

        private void AnimateOff()
        {
            targetGraphic.gameObject.transform.localScale = Vector3.one;
        }
        
    }
    
    #if UNITY_EDITOR
    [CustomEditor(typeof(ExtendedButton))]
    public class MenuButtonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // targetMenuButton._invokeAudioEvent = EditorGUILayout.Toggle("Text", targetMenuButton._invokeAudioEvent);
            DrawDefaultInspector();
        }
    }
    #endif
}