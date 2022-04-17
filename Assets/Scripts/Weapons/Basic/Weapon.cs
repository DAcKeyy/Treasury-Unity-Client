using Treasury.Data;
using UnityEngine;

namespace Treasury.Weapons.Basic
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