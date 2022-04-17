// -------------------------------------------------------------------------------------------
// The MIT License
// MonoCache is a fast optimization framework for Unity https://github.com/MeeXaSiK/MonoCache
// Copyright (c) 2021 Night Train Code
// -------------------------------------------------------------------------------------------

using Treasury.Miscellaneous.Optimization.System;
using Treasury.Miscellaneous.Sugar;

namespace Treasury.Miscellaneous.Optimization.MonoCache
{
    public abstract class MonoCache : MonoAllocation
    {
        private void OnEnable()
        {
            OnEnabled();
            
            GlobalUpdate.
                GetNotNull().
                IfNotNull(globalUpdate =>
                {
                    globalUpdate.OnUpdate += CachedUpdate;
                    globalUpdate.OnFixedUpdate += CachedFixedUpdate;
                    globalUpdate.OnLateUpdate += CachedLateUpdate;
                });
        }

        private void OnDisable()
        {
            GlobalUpdate.
                GetCanBeNull().
                IfNotNull(globalUpdate =>
                {
                    globalUpdate.OnUpdate -= CachedUpdate;
                    globalUpdate.OnFixedUpdate -= CachedFixedUpdate;
                    globalUpdate.OnLateUpdate -= CachedLateUpdate;
                });
            
            OnDisabled();
        }

        protected virtual void OnEnabled() { }
        protected virtual void OnDisabled() { }
        
        protected virtual void CachedUpdate() { }
        protected virtual void CachedFixedUpdate() { }
        protected virtual void CachedLateUpdate() { }
    }
}
