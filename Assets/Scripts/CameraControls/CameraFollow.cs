using UnityEditor;
using UnityEngine;

namespace Treasury.CameraControls
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        [Range(3,30)] 
        private float camDistanceToTarget = 15f;

        public Transform target;
        private UnityEngine.Camera _thisCamera;
    
        private void Start()
        {
            
            Init();
        }

        public void Init()
        {
            _thisCamera = gameObject.GetComponent<UnityEngine.Camera>();
            MoveToTarget();
        }
    
        private void Update()
        {
            MoveToTarget();
        }

        public void MoveToTarget()
        {
            gameObject.transform.position = new Vector3(transform.position.x, target.position.y + camDistanceToTarget, target.position.z);
        }
    }

    #region Editor
    [CustomEditor(typeof(CameraFollow))]
    public class CameraFollowEditor : Editor
    {
        private CameraFollow _cameraFollow;
        
        public void OnEnable()
        {
            _cameraFollow = (CameraFollow)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Установить позицию"))
            {
                if (_cameraFollow.target == null) {
                    Debug.LogWarning("Цель следования не выбрана");
                    return;
                }
                
                _cameraFollow.Init();
            }
        }
    }
    #endregion
}