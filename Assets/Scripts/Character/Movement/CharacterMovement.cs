using System;
using Treasury.Controls;
using Treasury.Data.Animations;
using Treasury.Miscellaneous.Optimization.System;
using TreasurySettings.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Treasury.Character.Movement
{
	public class CharacterMovement : MonoBehaviour
	{
		[SerializeField] private Animator _characterAnimator;
		
		//TODO Уменьшить связность
		private TreasuryControlls _treasuryControls;
		private const float ROTATION_SPEED = 15;
		private Camera _camera;
		////////////////////

		private void Awake()
		{
			//TODO Сделать уместнее или вообще убрать
			if (_characterAnimator.avatar == null) throw new Exception("CharacterMovement require Humanoid avatar");
			if (_characterAnimator.avatar.isHuman == false) throw new Exception("CharacterMovement require Humanoid avatar");

			//TODO Уменьшить связность и убрать
			_treasuryControls = new TreasuryControlls();
			Singleton<InputManager>.Instance.OnActionTriggered += InputCallbackHandle;
			Singleton<InputManager>.Instance.EnableCursor(false);
			_camera = Camera.main;
			//////////////////
		}

		public void Move(Vector2 direction)
		{
			_characterAnimator.SetFloat(AnimatorParameters.Locomotion.MOVE_INPUT_X, direction.x);
			_characterAnimator.SetFloat(AnimatorParameters.Locomotion.MOVE_INPUT_Y, direction.y);
		}

		public void Rotate(float yEulerAngle)
		{
			//TODO Вращать через рут анимации, не через transform.rotation
			transform.rotation = Quaternion.Slerp(
				transform.rotation, 
				Quaternion.Euler(0, yEulerAngle, 0),
				ROTATION_SPEED * Time.fixedDeltaTime);
		}

		//TODO Убрать обработки инпута отсюда
		private void InputCallbackHandle(InputAction.CallbackContext context)
		{
			switch (context.action.id)
			{
				case var actionId when (actionId == _treasuryControls._3dPerson.Move.id):
					Move(context.action.ReadValue<Vector2>());
					break;
				case var actionId when (actionId == _treasuryControls._3dPerson.Look.id):
					Rotate(_camera.transform.rotation.eulerAngles.y);
					break;
			}
		}
	}
}