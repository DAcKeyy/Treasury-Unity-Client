using Treasury.Miscellaneous.Optimization.System;
using Treasury.Controls.VirtualGamepads;
using UnityEngine.InputSystem;
using IngameDebugConsole;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace Treasury.Controls
{
    [RequireComponent(
    typeof(PlayerInput),
    typeof(EventSystem), 
    typeof(InputSystemUIInputModule))]
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private InputActionAsset _gameActionAsset;
        private PlayerInput _playerInput;
        private EventSystem _eventSystem;
        private InputSystemUIInputModule _inputSystemUIInputModule;
        
        private void Awake()
        {
            _playerInput = GetComponentCached<PlayerInput>();
            _eventSystem = GetComponentCached<EventSystem>();
            _inputSystemUIInputModule = GetComponentCached<InputSystemUIInputModule>();
            
            SwitchActionAsset(_gameActionAsset);
        }

        private void Start()
        {

        }

        public void SwitchActionMap(InputActionMap map)
        {
            _playerInput.SwitchCurrentActionMap(map.name);
        }

        public void SwitchActionAsset(InputActionAsset asset)
        {
            _playerInput.actions = _gameActionAsset;
        }

        public void ActivateConsole(bool isEnabled)
        {
            if (isEnabled)
            {
                var consoleGameobj = Instantiate(
                    Resources.Load<DebugLogManager>("Prefubs/System/Console/IngameDebugConsole"), 
                    GameObject.FindGameObjectWithTag("GameController").transform, 
                    true);

                consoleGameobj.ShowLogWindow();
            }
            else
            {
                var console = FindObjectOfType<DebugLogManager>(true);
                Destroy(console.gameObject);
            }
        }

        public void EnableVirtualGamepad(bool isEnabled, VirtualGamepad type)
        {
            if (isEnabled)
            {
                /*
                switch (type)
                {
                    case 3dPresonVirtualGamepad
                }
                */
                var virtualGamepadObj = Instantiate(
                    Resources.Load<VirtualGamepad>("Prefubs/UI/Virtual Gamepads/3dPerson Virtual Gamepad Canvas"), 
                    GameObject.FindGameObjectWithTag("GameController").transform, 
                    true);
            }
            else
            {
                var virtualGamepad = FindObjectOfType<VirtualGamepad>(true);
                Destroy(virtualGamepad.gameObject);
            }
        }

        public void SetActiveCanvas([CanBeNull] Canvas canvas)
        {
            if (canvas == null)
            {
                
            }
            
            
        }
    }
}
