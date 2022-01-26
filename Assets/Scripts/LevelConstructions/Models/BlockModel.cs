using System;
using UnityEngine;

namespace LevelConstructions
{
    [Serializable]
    public struct BlockModel
    {
        public string Name;
        public BlockType blockType;
        public Vector3 Position;
        
        [Serializable]
        public enum BlockType
        {
            Empty,
            Border,
            Barricade,
            Exit,
            SpawnPoint_Player,
            SpawnPoint_Enemy,
            Spikes,
            Ground
        }
    }
}