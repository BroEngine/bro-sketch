using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Bro.Client.Context
{
    public abstract class ClientContext : IClientContext
    {
        private readonly IList<IClientContextModule> _modules = new List<IClientContextModule>();
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private IList<IClientContextElement> _elements;
        
        public bool IsLoaded { get; private set; }
        public IScheduler Scheduler { get; private set; }
        public IApplication Application { get; private set; }

        protected ClientContext(IApplication application)
        {
            Application = application;
            Scheduler = application.GetScheduler(this);
        }
        
        private IList<T> Find<T>() where T: class
        {
            var hierarchy = GetType().GetHierarchy();
            var result = new List<T>();
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var targetInterfaceName = typeof(T).Name;
            foreach (var type in hierarchy)
            {
                var fields = type.GetFields(bindingFlags);
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(T) || field.FieldType.GetInterface(targetInterfaceName) != null)
                    {
                        var target = (T) field.GetValue(this);
                        result.Add(target);
                    }
                }
            }
            return result;
        }

        async UniTask IClientContext.Load()
        {   
            _elements = Find<IClientContextElement>();

            CheckRequiredElements(_elements);
            
            foreach (var element in _elements)
            {
                if (element is IClientContextModule module)
                {
                    _modules.Add(module);
                }
            }
            
            foreach (var element in _elements)
            {
                element.Setup(this);
            }
            foreach (var module in _modules)
            {
                await module.Load();
            }
            
            new ContextLoadedEvent(this).Launch();

            IsLoaded = true;
        }

        private void CheckRequiredElements(IList<IClientContextElement> elements)
        {
            var required = new List<RequireElementInContextAttribute>();
            foreach (var element in elements)
            {
                var requiredAttributes =  element.GetType().GetCustomAttributes<RequireElementInContextAttribute>();
                foreach (var attribute in requiredAttributes)
                {
                    required.Add(attribute);
                }
            }

            foreach (var requiredData in required)
            {
                bool contains = elements.FirstOrDefault(element => requiredData.RequiredType.IsInstanceOfType(element)) != null;
                if (!contains)
                {
                    foreach (var el in elements)
                    {
                        Debug.Log($" el {el.GetType()} => {el.GetType().IsAssignableFrom(requiredData.RequiredType)} => {requiredData.RequiredType.FullName}" );
                    }
                    Debug.LogError($"client context :: {requiredData.OwnerClassName} required {requiredData.RequiredType} in context, but it cannot be found");
                }
            }
            
        }

        async UniTask IClientContext.Unload()
        {
            foreach (var module in _modules)
            {
                await module.Unload();
            }
            
            foreach (var disposable in _disposables)
            {
                disposable?.Dispose();
            }
            
            Scheduler.Dispose();
            
            new ContextUnloadedEvent(this).Launch();
        }

        public T Get<T>() where T : class, IClientContextElement
        {
            T result = null;
            if (_elements != null)
            {
                for (int i = 0, max = _elements.Count; i < max; ++i)
                {
                    result = _elements[i] as T;
                    if (result != null)
                    {
                        break;
                    }
                }
            }
            else
            {
                Debug.LogError("client context :: elements wasn't prepared");
            }
            
            
            return result;
        }

        public void AddDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }
    }
}