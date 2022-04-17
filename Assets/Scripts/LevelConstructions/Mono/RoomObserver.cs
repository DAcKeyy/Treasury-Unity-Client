using System;
using System.Collections.Generic;
using Treasury.Character.Characters;
using Treasury.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Treasury.LevelConstructions.Mono
{
    public class RoomObserver : MonoBehaviour
    {
        [SerializeField] private BlockBehaviour block;
        [SerializeField] private Player player;
        public Action StageClear = delegate {  };
        public Action GameOver = delegate {  };
        private RoomGenerator _roomGenerator;
        private List<Character.Basic.Character> Enemys = new List<Character.Basic.Character>();
          
        private void Awake()
        {
            Init();
            CreateRoom();
        }

        public void Init()
        {
            _roomGenerator = new RoomGenerator(block);
            player.Died += character => { GameOver(); };
        }

        public void CreateRoom()
        {
            var rooms = Resources.LoadAll<RoomData>("Game Data/Rooms");
            Debug.Log(rooms.Length);
            if(rooms.Length == 0) return;
            
            var room = rooms[Random.Range(0, rooms.Length -1)];
            var points = _roomGenerator.Generate(room);

            var position = points.playerSpawnPoint.position;
            player.transform.position = new Vector3(position.x, position.y, position.z);

            for(int i = 0; i < room.Enemies.Count; i++)
            {
                var enemy = Instantiate(room.Enemies[i], points.EnemysSpawnPoints[i]);
                enemy.Died += CharacterDies;
                Enemys.Add(enemy);
            }
        }

        private void CharacterDies(Character.Basic.Character died)
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
                _roomObserver.Init();
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