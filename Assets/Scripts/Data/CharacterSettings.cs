using System;
using UnityEngine;

namespace Treasury.Data
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