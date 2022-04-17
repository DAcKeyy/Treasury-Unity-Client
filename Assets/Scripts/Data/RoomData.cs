using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Treasury.LevelConstructions.Models;
using UnityEditor;
using UnityEngine;

namespace Treasury.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Room", menuName = "Game/Room")]
    public class RoomData : ScriptableObject
    {
        //Очень умно на самом деле сохранять всё что угодно в байтареях а потом это конвертировать обратно ;)
        public BlockModel.BlockType[,] RoomGrid => RoomSettingsEditor.FromByteArray<BlockModel.BlockType[,]>(roomGridBinary);
        public Vector2Int RoomSize => roomSize;
        public List<Character.Basic.Character> Enemies => enemies; 
        
        

        [SerializeField] private List<Character.Basic.Character> enemies = new List<Character.Basic.Character>();
        
        [SerializeField] private Vector2Int roomSize = new Vector2Int(20,20);
        
        [SerializeField][HideInInspector] private byte[] roomGridBinary; //Smartest programmer in the world

        
        /// <summary>
        /// For Editor Only!!!
        /// </summary>
        public void SetNewRoomBinary(byte[] newRoomData) => roomGridBinary = newRoomData; //TODO Подумать с инкапсуляцией этого чуда
    }
    
    #region Editor
    [CustomEditor(typeof(RoomData))]
    public class RoomSettingsEditor : Editor
    {
        private RoomData _roomData;
        private BlockModel.BlockType[,] _roomGridTemp;

        private void OnEnable()
        {
            _roomData = (RoomData)target;
            _roomGridTemp = _roomData.RoomGrid;
            if (_roomGridTemp == null) 
                _roomGridTemp = new BlockModel.BlockType[_roomData.RoomSize.x, _roomData.RoomSize.y];
        }

        private void OnDisable()
        {
            //при закрытии Editor окна сохраняем массив из опретивы (ну чтобы проц не жрать во время создания комнаты)
            _roomData.SetNewRoomBinary(ToByteArray(_roomGridTemp)); 
            EditorUtility.SetDirty(_roomData);
            Debug.Log("Room Setting Closed");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (_roomGridTemp.GetLength(0) != _roomData.RoomSize.x || 
                _roomGridTemp.GetLength(1) != _roomData.RoomSize.y) {

                _roomGridTemp = (BlockModel.BlockType[,]) ResizeArray(
                    _roomGridTemp, 
                    new int[]
                    {
                        _roomData.RoomSize.x,
                        _roomData.RoomSize.y
                    } );
            }
            
            EditorGUILayout.Space(10);
            
            //TODO Переделать отрисовку команты на норм
            //открисовка карты комнаты
            
            EditorGUILayout.LabelField("RoomGrid");
            EditorGUILayout.BeginHorizontal ();
            for (int x = _roomGridTemp.GetLength(0) - 1; x >= 0; x--){
                EditorGUILayout.BeginVertical ();
                for (int y = _roomGridTemp.GetLength(1) - 1; y > 0 ; y--) 
                {
                    GUIStyle style = new GUIStyle(GUI.skin.button);//из EnumPopup гыгыгыгыгыгыг 
                    
                    switch (_roomGridTemp[x, y])
                    {
                        case BlockModel.BlockType.Empty:
                            GUI.color = new Color(255,255,255, 0.05f);
                            break;
                        case BlockModel.BlockType.Barricade:
                            GUI.color = Color.white;
                            break;
                        case BlockModel.BlockType.Border:
                            GUI.color = Color.gray;
                            break;
                        case BlockModel.BlockType.Exit:
                            GUI.color = Color.green;
                            break;
                        case BlockModel.BlockType.Ground:
                            GUI.color = new Color(17,122,101, 1);
                            break;
                        case BlockModel.BlockType.Spikes:
                            GUI.color = new Color(136,78,160, 1);
                            break;
                        case BlockModel.BlockType.SpawnPoint_Enemy:
                            GUI.color = Color.red;
                            break;
                    }
                    
                    _roomGridTemp[x, y] = (BlockModel.BlockType) EditorGUILayout.EnumPopup(_roomGridTemp[x, y],style,GUILayout.Width(30),GUILayout.Height(30));
                }
                EditorGUILayout.EndVertical ();
            }
            EditorGUILayout.EndHorizontal ();
        }
        
        #region Metods
        public static byte[] ToByteArray<T>(T obj)
        {
            if(obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using(MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(byte[] data)
        {
            if(data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using(MemoryStream ms = new MemoryStream(data))
            {
                object obj = null;
            
                try
                {
                    obj = bf.Deserialize(ms);
                }
                catch (SerializationException exception)
                {
                    return default;
                }

                return (T)obj;
            }
        }
        private static Array ResizeArray(Array arr, int[] newSizes)
        {
            if (newSizes.Length != arr.Rank)
                throw new ArgumentException("arr must have the same number of dimensions " +
                                            "as there are elements in newSizes", "newSizes");

            var temp = Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
            int length = arr.Length <= temp.Length ? arr.Length : temp.Length;
            Array.ConstrainedCopy(arr, 0, temp, 0, length);
            return temp;
        }
        #endregion
    }
    #endregion
}