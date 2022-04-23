using Treasury.Controls;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Treasury.Editor
{
	[CustomEditor(typeof(InputManager))]
	public class InputManagerEditor : UnityEditor.Editor
	{
		private InputManager _inputManager;
		private string _actionName;

		public void OnEnable()
		{
			_inputManager = (InputManager) target;
		}

		void Dolbaeb(InputAction.CallbackContext context)
		{
			_actionName = context.action.name;
			Debug.Log(_actionName);
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			
			if(!Application.isPlaying) return;

			EditorGUILayout.LabelField("Current Action Map", _inputManager.CurrentActionMap.name);
			EditorGUILayout.LabelField("Current Control Scheme", _inputManager.CurrentControlScheme);
			EditorGUILayout.LabelField("Action", _inputManager.LastAction.name);
		}
	}
}