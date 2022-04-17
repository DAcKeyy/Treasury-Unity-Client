// -------------------------------------------------------------------------------------------
// The MIT License
// MonoCache is a fast optimization framework for Unity https://github.com/MeeXaSiK/MonoCache
// Copyright (c) 2021 Night Train Code
// -------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using Treasury.Miscellaneous.Console;
using Treasury.Miscellaneous.Optimization.System;
using UnityEngine;

namespace Treasury.Miscellaneous.Optimization.MonoCache
{
    [DisallowMultipleComponent]
    public sealed class GlobalUpdate : Singleton<GlobalUpdate>
    {
        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnLateUpdate;

        private const string OnEnable = nameof(OnEnable);
        private const string OnDisable = nameof(OnDisable);
        
        private const string UpdateName = nameof(Update);
        private const string FixedUpdateName = nameof(FixedUpdate);
        private const string LateUpdateName = nameof(LateUpdate);

        private const BindingFlags MethodFlags = BindingFlags.Public | 
                                                 BindingFlags.NonPublic | 
                                                 BindingFlags.Instance |
                                                 BindingFlags.DeclaredOnly;

        private void Awake()
        {
            CheckForErrors();
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
        
        private void CheckForErrors()
        {
            var subclassTypes = Assembly
                .GetAssembly(typeof(MonoCache))
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(MonoCache)));
            
            foreach (var type in subclassTypes)
            {
                var methods = type.GetMethods(MethodFlags);
                
                foreach (var method in methods)
                {
                    if (method.Name == OnEnable)
                    {
                        Debug.LogException(new Exception(
                            $"{TextExtentions.GetExceptionBaseText(OnEnable, type.Name)}" +
                            $"{TextExtentions.GetColoredHTMLText(TextExtentions.BLUE_COLOR, "protected override void")} " +
                            $"{TextExtentions.GetColoredHTMLText(TextExtentions.ORANGE_COLOR, "OnEnabled()")}"));
                    }

                    if (method.Name == OnDisable)
                    {
                        Debug.LogException(new Exception(
                            $"{TextExtentions.GetExceptionBaseText(OnDisable, type.Name)}" +
                            $"{TextExtentions.GetColoredHTMLText(TextExtentions.BLUE_COLOR, "protected override void")} " +
                            $"{TextExtentions.GetColoredHTMLText(TextExtentions.ORANGE_COLOR, "OnDisabled()")}"));
                    }
                    
                    if (method.Name == UpdateName)
                    {
                        Debug.LogWarning(
                            TextExtentions.GetWarningBaseText(
                                method.Name, "Run()", type.Name));
                    }
                    
                    if (method.Name == FixedUpdateName)
                    {
                        Debug.LogWarning(
                            TextExtentions.GetWarningBaseText(
                                method.Name, "FixedRun()", type.Name));
                    }
                    
                    if (method.Name == LateUpdateName)
                    {
                        Debug.LogWarning(
                            TextExtentions.GetWarningBaseText(
                                method.Name, "LateRun()", type.Name));
                    }
                }
            }
        }
    }
}