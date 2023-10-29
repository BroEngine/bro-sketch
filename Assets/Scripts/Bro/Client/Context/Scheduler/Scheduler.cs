using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bro.Client.Context
{
    public class UnityScheduler : MonoBehaviour, IScheduler
    {
        private class UpdateFunction : IDisposable
        {
            public readonly Action<float> Action;
            public bool IsValid { get; private set; } = true;

            public UpdateFunction(Action<float> action)
            {
                Action = action;
            }

            void IDisposable.Dispose()
            {
                IsValid = false;
            }
        }

        private readonly List<UpdateFunction> _updateFunctions = new List<UpdateFunction>();
        private readonly List<UpdateFunction> _lateUpdateFunctions = new List<UpdateFunction>();
        private readonly List<UpdateFunction> _fixedUpdateFunctions = new List<UpdateFunction>();

        IDisposable IScheduler.ScheduleUpdate(Action<float> update)
        {
            var upd = new UpdateFunction(update);
            _updateFunctions.Add(upd);
            return upd;
        }

        IDisposable IScheduler.ScheduleFixedUpdate(Action<float> update)
        {
            var upd = new UpdateFunction(update);
            _fixedUpdateFunctions.Add(upd);
            return upd;
        }

        IDisposable IScheduler.ScheduleLateUpdate(Action<float> update)
        {
            var upd = new UpdateFunction(update);
            _lateUpdateFunctions.Add(upd);
            return upd;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            for (var index = _updateFunctions.Count - 1; index >= 0;)
            {
                var upd = _updateFunctions[index];
                if (upd.IsValid)
                {
                    upd.Action.Invoke(dt);
                    index--;
                }
                else
                {
                    _updateFunctions.FastRemoveAtIndex(index);
                }
            }
        }

        private void LateUpdate()
        {
            float dt = Time.deltaTime;
            for (var index = _lateUpdateFunctions.Count - 1; index >= 0;)
            {
                var upd = _lateUpdateFunctions[index];
                if (upd.IsValid)
                {
                    upd.Action.Invoke(dt);
                    index--;
                }
                else
                {
                    _lateUpdateFunctions.FastRemoveAtIndex(index);
                }
            }
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;
            for (var index = _fixedUpdateFunctions.Count - 1; index >= 0;)
            {
                var upd = _fixedUpdateFunctions[index];
                if (upd.IsValid)
                {
                    upd.Action.Invoke(dt);
                    index--;
                }
                else
                {
                    _fixedUpdateFunctions.FastRemoveAtIndex(index);
                }
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            new ApplicationFocusEvent(focus).Launch();
        }

        private void OnApplicationPause(bool pause)
        {
            new ApplicationPauseEvent(pause).Launch();
        }

        void IDisposable.Dispose()
        {
            _updateFunctions.Clear();
            _lateUpdateFunctions.Clear();
            _fixedUpdateFunctions.Clear();
        }
    }
}