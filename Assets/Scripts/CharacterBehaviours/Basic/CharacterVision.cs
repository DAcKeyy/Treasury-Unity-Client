using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace CharacterBehaviours
{
    public class CharacterVision : MonoBehaviour
    {
        public Action<Transform[]> TargetFounded = delegate { }; 
        [Range(0,360)]
        public float viewAngle;
        public float viewRadius;
        public LayerMask targetMask;
        public LayerMask obstacleMask;
        public List<Transform> visableTargets;

        private void Start()
        {
            StartCoroutine(FindTargetsWithDelay(.2f));
        }

        IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }

        void FindVisibleTargets()
        {
            visableTargets.Clear();
            var targetsInView = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
            //Debug.Log(targetsInView.Length);
            for(int i =0; i < targetsInView.Length; i++)
            {
           
                Transform target = targetsInView[i].transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                if(Vector3.Angle (transform.forward, directionToTarget) < viewAngle / 2)
                {
                    float distanseToTarget = Vector3.Distance(transform.position, target.position);

                    if(!Physics.Raycast(transform.position, directionToTarget, distanseToTarget, obstacleMask))
                    {
                        visableTargets.Add(target);
                    }
                }
            }
            TargetFounded(visableTargets.ToArray());
        }

        public Vector3 DirectoryFromAngle(float angleInDeg, bool isAngleGlobal)
        {
            if(!isAngleGlobal)
            {
                angleInDeg += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
        }
    }
    
    #region Editor
    
    [CustomEditor(typeof(CharacterVision))]
    public class CharacterVisionEditor : Editor
    {
        private void OnSceneGUI()
        {
            CharacterVision fow = (CharacterVision) target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
            Vector3 viewAngleA = fow.DirectoryFromAngle(-fow.viewAngle / 2, false);
            Vector3 viewAngleB = fow.DirectoryFromAngle(fow.viewAngle / 2, false);

            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

            Handles.color = Color.red;
            foreach (Transform visableTarget in fow.visableTargets)
            {
                Handles.DrawLine(fow.transform.position, visableTarget.position);
            }
        }
    }
    
    #endregion
}