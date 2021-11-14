using System;
using System.Collections.Generic;
using CharacterBehaviours;
using LevelConstructions;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Room", menuName = "Game/Room")]
    public class RoomSettings : ScriptableObject
    {
        public BlockModel.BlockType[,] RoomGrid = new BlockModel.BlockType[25,11];
        public List<Character> enemies = new List<Character>(); 
        [Range(10,100)]public int RoomLenght = 10;
        
        #region Editor
        [CustomEditor(typeof(RoomSettings))]
        public class RoomSettingsEditor : Editor
        {
            private RoomSettings _roomSettings;
            public void OnEnable()
            {
                _roomSettings = (RoomSettings)target;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (_roomSettings.RoomGrid.GetLength(0) != _roomSettings.RoomLenght)
                {
                    var room = _roomSettings.RoomGrid;
                    _roomSettings.RoomGrid = (BlockModel.BlockType[,]) ResizeArray(_roomSettings.RoomGrid, new int[] {_roomSettings.RoomLenght, _roomSettings.RoomGrid.GetLength(1)} );
                }
                
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("RoomGrid");
                EditorGUILayout.BeginHorizontal ();
                for (int y = 0; y < _roomSettings.RoomGrid.GetLength(1); y++) {
                    EditorGUILayout.BeginVertical ();
                    for (int x = _roomSettings.RoomGrid.GetLength(0) - 1; x >= 0; x--)
                    {
                        _roomSettings.RoomGrid[x, y] = (BlockModel.BlockType)EditorGUILayout.EnumPopup(_roomSettings.RoomGrid[x, y]);
                    }
                    EditorGUILayout.EndVertical ();
                }
                EditorGUILayout.EndHorizontal ();
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