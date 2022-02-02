using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using LevelConstructions;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Room", menuName = "Game/Room")]
    public class RoomSettings : ScriptableObject
    {
        public BlockModel.BlockType[,] RoomGrid
        {
            get => FromByteArray<BlockModel.BlockType[,]>(roomGridBinary);
            set => roomGridBinary = ToByteArray(value);
        }
        
        public List<Character.Basic.Character> enemies = new List<Character.Basic.Character>();
        
        [Range(10,100)] 
        public int roomLenght = 10;
        
        [HideInInspector] 
        public byte[] roomGridBinary;
        
        #region Editor
        [CustomEditor(typeof(RoomSettings))]
        public class RoomSettingsEditor : Editor
        {
            private RoomSettings _roomSettings;
            private BlockModel.BlockType[,] _roomGridTemp;

            private void OnEnable()
            {
                _roomSettings = (RoomSettings)target;
                _roomGridTemp = _roomSettings.RoomGrid;
            }

            private void OnDisable()
            {
                _roomSettings.RoomGrid = _roomGridTemp;
                EditorUtility.SetDirty(_roomSettings);
                Debug.Log("Closed");
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                if (_roomGridTemp == null) 
                    _roomGridTemp = new BlockModel.BlockType[_roomSettings.roomLenght, 11];
                if (_roomGridTemp.GetLength(0) != _roomSettings.roomLenght)
                {
                    var room = _roomSettings.RoomGrid;
                    
                    _roomGridTemp = (BlockModel.BlockType[,]) ResizeArray(_roomGridTemp, new int[] {_roomSettings.roomLenght, _roomGridTemp.GetLength(1)} );
                }
                
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("RoomGrid");
                EditorGUILayout.BeginHorizontal ();
                for (int y = 0; y < _roomGridTemp.GetLength(1); y++) {
                    EditorGUILayout.BeginVertical ();
                    for (int x = _roomGridTemp.GetLength(0) - 1; x >= 0; x--)
                    {
                        GUIStyle style = new GUIStyle(GUI.skin.button);
                        
                        switch (_roomGridTemp[x, y])
                        {
                            case BlockModel.BlockType.Barricade:
                                style.normal.textColor = Color.blue;
                                break;

                                throw new ArgumentOutOfRangeException();
                        }
                        
                        _roomGridTemp[x, y] = (BlockModel.BlockType) EditorGUILayout.EnumPopup(_roomGridTemp[x, y],style);
                    }
                    EditorGUILayout.EndVertical ();
                }
                EditorGUILayout.EndHorizontal ();
            }
        }
        #endregion
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
}