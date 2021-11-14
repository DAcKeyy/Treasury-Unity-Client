using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Game/Character")]
    [Serializable]
    public class CharacterSettings : ScriptableObject
    {
        public int hp;
        public int coins;
        public CharacterType characterType;
        public WeaponSettings weaponData;
            
        public enum CharacterType
        {
            Player,
            Zombie,
            SkeletonArcher,
            SkeletonMelee
        }
    }
}