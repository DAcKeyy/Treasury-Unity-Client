using System;
using Treasury.Weapons.Basic;
using UnityEngine;

namespace Treasury.Data
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon")]
    [Serializable]
    public class WeaponSettings : ScriptableObject
    {
        public new string name;
        public int damage;
        public float speedMultiplier;
        public WeaponType type;
        public Weapon Weapon;

        public enum WeaponType
        {
            Sword = 0,
            Staff = 1,
            Bow = 2,
            Sphere = 3
        }
    }
}