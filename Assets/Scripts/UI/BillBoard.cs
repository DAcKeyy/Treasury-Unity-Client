using System;using UnityEngine;

namespace UI
{
    public class BillBoard : MonoBehaviour
    {
        private new Transform _camera;

        private void Start()
        {
            if (Camera.main != null) _camera = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.forward);
            Debug.Log("SADASDS");
        }
    }
}