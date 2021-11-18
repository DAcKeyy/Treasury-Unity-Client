using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        [JsonConverter(typeof(StringEnumConverter))]
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