using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Treasury.Character.Basic
{
    public class CharacterVision : MonoBehaviour
    {
        public Action<Transform[]> TargetFounded = delegate { };
        public float ViewAngle => viewAngle;
        public float ViewRadius => viewRadius;
        public List<Transform> VisableTargets => visableTargets;
        
        [SerializeField] [Range(0,360)] private float viewAngle;
        [SerializeField] [Range(.5f,30)] private float viewRadius;
        [SerializeField] [Range(.01f,1f)] private float checkUpdateTime = .2f;
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private List<Transform> visableTargets = new List<Transform>();
        private int _prevAmountTargets;

        private void Start()
        {
            StartCoroutine(FindTargetsWithDelay(checkUpdateTime));
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

            if (_prevAmountTargets != visableTargets.Count)
            {
                _prevAmountTargets = visableTargets.Count;
                if(visableTargets.Count != 0) TargetFounded(visableTargets.ToArray());
                else TargetFounded(null);
            }
        }

        public static Transform GetClosestTarget(Transform[] targets, Transform origin)
        {
            if (targets == null) return null;
            
            var minDistanceToTarget = Mathf.Infinity;
            Transform closestTarget = null;
            foreach (var target in targets)
            {
                var distance = Vector3.Distance(target.position, origin.position);
                if (distance < minDistanceToTarget)
                {
                    minDistanceToTarget = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
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
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.ViewRadius);

            if (fow.ViewAngle != 360)
            {
                Vector3 viewAngleA = fow.DirectoryFromAngle(-fow.ViewAngle / 2, false);
                Vector3 viewAngleB = fow.DirectoryFromAngle(fow.ViewAngle / 2, false);

                Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.ViewRadius);
                Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.ViewRadius);
            }

            Handles.color = Color.red;
            foreach (Transform visableTarget in fow.VisableTargets)
            {
                Handles.DrawLine(fow.transform.position, visableTarget.position);
            }
        }
    }
    
    #endregion
}
