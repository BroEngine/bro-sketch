using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bro.Client.UI
{
    public class UICreator
    {
        private const string CanvasName = "canvas_ui";
        private const string RootName = "root";
        private const string BlockerName = "blocker";
        private const string EventSystemName = "event_system_ui";
        
        private const int SortingOrder = 0;

        private readonly bool _isPermanent;
        private readonly bool _isEventSystem;

        private Transform _canvasTransform;
        private Transform _rootTransform;
        private GameObject _blocker;

        public Transform Root => _rootTransform;
        public GameObject Blocker => _blocker;
        
        public UICreator(bool isEventSystem, bool isPermanent)
        {
            _isEventSystem = isEventSystem;
            _isPermanent = isPermanent;
        }

        private void CreateEventSystem()
        {
            var eventSystem = new GameObject(EventSystemName);
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            if (_isPermanent)
            {
                Object.DontDestroyOnLoad(eventSystem);
            }
        }
        
        public void CreateCanvas(Vector2 resolution, float matchFactor)
        {
            if (_isEventSystem)
            {
                CreateEventSystem();
            }

            var objectCanvas = new GameObject(CanvasName);

            var canvas = objectCanvas.AddComponent<Canvas>();
            var scaler = objectCanvas.AddComponent<CanvasScaler>();
            objectCanvas.AddComponent<GraphicRaycaster>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = matchFactor;
            scaler.referenceResolution = resolution;
            canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.TexCoord1;

            _canvasTransform = objectCanvas.transform;
            
            if (_isPermanent)
            {
                Object.DontDestroyOnLoad(_canvasTransform.gameObject);
            }

            _rootTransform = CreateCanvasGroup(RootName);
            _blocker = CreateBlocker();
        }

        private GameObject CreateStretchedRect(string name, Transform root)
        {
            var canvasGroup = new GameObject(name, typeof(RectTransform));
            canvasGroup.transform.SetParent(root);
            var rectTransform = (RectTransform)canvasGroup.transform;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMin = new Vector2();
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = new Vector2();
            rectTransform.offsetMax = new Vector2();
            return canvasGroup;
        }

        public Transform CreateCanvasGroup(string name)
        {
            var canvasGroup = CreateStretchedRect(name, _canvasTransform);
            canvasGroup.AddComponent<CanvasGroup>();
            return canvasGroup.transform;
        }
        
        private GameObject CreateBlocker()
        {
            var objectBlocker = CreateStretchedRect(BlockerName, _canvasTransform);
            
            var canvas = objectBlocker.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = short.MaxValue;
            
            objectBlocker.AddComponent<GraphicRaycaster>();
            
            var image = objectBlocker.AddComponent<Image>();
            image.color = Color.clear;

            return objectBlocker;
        }
    }
}