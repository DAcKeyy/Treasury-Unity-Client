using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Movement
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField]
        private FixedJoystick joystick;
        [SerializeField] 
        [Range(1, 5)]
        private float angleMultiplier = 1;
        [HideInInspector]
        public bool isCalm;

        private void FixedUpdate()
        {
            isCalm = (joystick.Horizontal, joystick.Vertical) == (0, 0);
            MovePointer(new Vector2(joystick.Horizontal, joystick.Vertical));
        }
        
        void MovePointer(Vector2 direction)
        {
            transform.position = new Vector3( transform.parent.transform.position.x + direction.x * angleMultiplier, transform.position.y, transform.parent.transform.position.z + direction.y * angleMultiplier);
        }
    }
}