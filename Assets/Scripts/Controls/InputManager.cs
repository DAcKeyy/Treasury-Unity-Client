using System;
using Treasury.Miscellaneous.Optimization.System;
using Treasury.Controls.VirtualGamepads;
using UnityEngine.InputSystem;
using IngameDebugConsole;
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
        public Action<InputAction.CallbackContext> OnActionTriggered = delegate(InputAction.CallbackContext context) {  };
        
        public InputActionAsset GameActionAsset => _gameActionAsset;
        public InputActionMap CurrentActionMap => _playerInput.currentActionMap;
        public string CurrentControlScheme => _playerInput.currentControlScheme;
        public InputAction LastAction;
        
        [SerializeField] private InputActionAsset _gameActionAsset;
        private Camera _controlCamera;
        private PlayerInput _playerInput;
        private EventSystem _eventSystem;
        private InputSystemUIInputModule _inputSystemUIInputModule;

        private void Awake()
        {
            _playerInput = GetComponentCached<PlayerInput>();
            _eventSystem = GetComponentCached<EventSystem>();
            _inputSystemUIInputModule = GetComponentCached<InputSystemUIInputModule>();
            
            SwitchActionAsset(_gameActionAsset);
            SetControlCamera(Camera.main);//TODO: Make by injection
            
            _inputSystemUIInputModule.actionsAsset = _gameActionAsset;
            _playerInput.uiInputModule = _inputSystemUIInputModule;
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.onActionTriggered += OnActionTriggered;
            _playerInput.onActionTriggered += (callback) => { LastAction = callback.action;};
        }

        private void Start()
        {
            SwitchActionMap(_playerInput.actions.FindActionMap("3dPerson"));
            OnActionTriggered += context => Debug.Log(context);
        }

        public void SwitchActionMap(InputActionMap map)
        {
            _playerInput.SwitchCurrentActionMap(map.name);
        }

        public void SwitchActionAsset(InputActionAsset asset)
        {
            _playerInput.actions = _gameActionAsset;
        }

        public void SwitchControlScheme(InputDevice[] newDevices)
        {
            _playerInput.SwitchCurrentControlScheme(newDevices);
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

        public void SetControlCamera(Camera newCamera)
        {
            _controlCamera = newCamera;
            _playerInput.camera = _controlCamera;
        }

        public void EnableCursor(bool enable)
        {
            Cursor.visible = enable;
            Cursor.lockState = enable ? CursorLockMode.Confined : CursorLockMode.Locked;
        }
    }
}
