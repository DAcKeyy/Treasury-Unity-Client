using System;
using Treasury.Data;
using UnityEditor;
using UnityEngine;

namespace Treasury.CameraControls
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraAutoSize : MonoBehaviour
    {
        [SerializeField] private ReferenceResolution referenceResolution = new ReferenceResolution() {width = 1024, height = 1920};

        private UnityEngine.Camera _thisCamera;

        private void Start()
        {
            Init();
        }

        public void SaveDefaultCameraSize()
        {
            _thisCamera = gameObject.GetComponent<UnityEngine.Camera>();
            StaticPrefs.CameraSize = _thisCamera.orthographicSize;
        }
        
        public void Init()
        {
            _thisCamera = gameObject.GetComponent<UnityEngine.Camera>();

            var screenResolution = Screen.currentResolution;

            if (Screen.orientation != ScreenOrientation.Portrait)
            {
                Debug.LogWarning("Установите Default orientation в Portrait во Player Settings");
                return;
            }

            if (screenResolution.height < screenResolution.width)
            {
                Debug.LogWarning("Некорректное соотношение высоты и ширины экрана, вам следует использовать Devise simulator для отоброжения игры во вкладке Game");
                (screenResolution.height, screenResolution.width) = (screenResolution.width, screenResolution.height);
            }
            
            //Debug.Log($"height:{screenResolution.height }  width:{screenResolution.width} orientation:{Screen.orientation}");
            Debug.Log($"Соотношение сторон: {screenResolution.height / CommonDivisor(screenResolution.height,screenResolution.width)} на {screenResolution.width / CommonDivisor(screenResolution.height,screenResolution.width)}");
            
            var commonDivisorRef = CommonDivisor(referenceResolution.width, referenceResolution.height);
            var differenceCoefficientRef =  ((float)referenceResolution.height / commonDivisorRef) / ((float)referenceResolution.width / commonDivisorRef);
            var screenCommonDivisorCurrent = CommonDivisor(screenResolution.width, screenResolution.height);
            var differenceCoefficientCurrent =  ((float)screenResolution.height / screenCommonDivisorCurrent) / ((float)screenResolution.width / screenCommonDivisorCurrent);
            var sizeMultiplier = (float)differenceCoefficientCurrent / (float)differenceCoefficientRef;
            
            //Debug.Log($"Коифиценты: {differenceCoefficientCurrent} / {differenceCoefficientRef} = {sizeMultiplier}");
            _thisCamera.orthographicSize = StaticPrefs.CameraSize; 
            _thisCamera.orthographicSize *= sizeMultiplier;
        }

        private void Reset()
        {
            SaveDefaultCameraSize();
        }

        static int CommonDivisor(int a, int b)
        {
            while (b > 0)
            {
                int rem = a % b;
                a = b;
                b = rem;
            }
            return a;
        }
        
        [Serializable]
        public struct ReferenceResolution
        {
            public int width;
            public int height;
        }
    }
    
    #region Editor
    [CustomEditor(typeof(CameraAutoSize))]
    public class CameraAutoSizeEditor : Editor
    {
        private CameraAutoSize _cameraAutoSize;
        
        private void OnEnable()
        {
            _cameraAutoSize = (CameraAutoSize) target;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Установить авторазмер"))
            {
                _cameraAutoSize.Init();
            }

            if (GUILayout.Button("Сохранить размер ортокамеры"))
            {
                _cameraAutoSize.SaveDefaultCameraSize();
            }
        }
    }
    #endregion
}