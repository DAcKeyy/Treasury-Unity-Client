using System;
using UnityEngine;

namespace UI
{
    public class Billboard : MonoBehaviour
    {
        private Transform _camera;

        private void Start()
        {
            if (Camera.main != null) _camera = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + _camera.forward);
        }
    }
}