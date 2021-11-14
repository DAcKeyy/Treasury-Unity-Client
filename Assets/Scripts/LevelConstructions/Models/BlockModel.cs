using System;
using UnityEngine;

namespace LevelConstructions
{
    [Serializable]
    public struct BlockModel
    {
        public string Name { get; set; }
        public BlockType blockType { get; set; }
        public Vector3 Position { get; set; }
        
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