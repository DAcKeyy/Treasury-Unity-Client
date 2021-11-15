using Data;
using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private WeaponSettings weaponData;
        public WeaponSettings WeaponData { get => weaponData; }
        public bool IsReady { get; set; }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}