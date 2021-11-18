using System;using UnityEngine;

namespace UI
{
    public class BillBoard : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(transform.position + Camera.main.transform.forward);
            Debug.Log("SADASDS");
        }
    }
}