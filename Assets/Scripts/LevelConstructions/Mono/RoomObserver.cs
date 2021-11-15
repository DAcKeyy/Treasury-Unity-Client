using System;
using System.Collections.Generic;
using CharacterBehaviours;
using Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace LevelConstructions
{
    public class RoomObserver : MonoBehaviour
    {
        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private Player player;
        public Action StageClear = delegate {  };
        private List<Character> Enemys = new List<Character>();
        
        public void Awake()
        {
            CreateRoom();
        }

        public void CreateRoom()
        {
            RoomSettings[] roomSettings = Resources.LoadAll<RoomSettings>("Rooms");
            var room = roomSettings[Random.Range(0, roomSettings.Length - 1)];
            
            var points = levelGenerator.Generate(room);
            player.transform.position = new Vector3(points.playerSpawnPoint.position.x, points.playerSpawnPoint.position.y, points.playerSpawnPoint.position.z);

            for(int i = 0; i < room.enemies.Count; i++)
            {
                var enemy = Instantiate(room.enemies[i], points.EnemysSpawnPoints[i]);
                enemy.Died += CharactrerDies;
                Enemys.Add(enemy);
            }
        }

        public void CharactrerDies(Character died)
        {
            Enemys.Remove(died);
            if (Enemys.Count == null) StageClear();
        }

        public void GoToNextRoom()
        {
            SceneManager.LoadScene("The Loop Game", LoadSceneMode.Single);
        }

        public void GoToMenu()
        {
            
        }
        
        
        #region Editor

        [CustomEditor(typeof(RoomObserver))]
        public class RoomObserverEditor : Editor
        {
            private RoomObserver _roomObserver;
            
            public void OnEnable()
            {
                _roomObserver = (RoomObserver)target;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (GUILayout.Button("Создать уровень"))
                {
                    _roomObserver.CreateRoom();
                }
            }
        }

        #endregion
    }
}