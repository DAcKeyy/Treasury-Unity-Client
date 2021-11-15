using Data;
using UnityEngine;

namespace Weapons
{
    public interface IReloadable
    {
        void Reload();
    }

    public interface IShootable
    {
        bool IsCharged { get; set;}
        float ShootDelay { get; set; }
        float RecoilForce { get; set; }
        Transform BulletSpawnPoint { get; set; }
        GameObject BulletPrefab { get; set; }
        void Shoot();
    }
    
    public interface IWeapon
    {
        WeaponSettings WeaponData { get; }
        bool IsReady { get; set;}
        void Show();
        void Hide();
    }

    public interface IAutoAimable
    {
        public Vector3 Target { get; set; }
    }
    
    public interface IChargeable
    {
        void Charge();
    }
    
    public interface IBullet
    {
        int Damage { get; set; }
        void Fly(Vector3 destanation);
    }

    public interface IMeleeWeapon
    {
        void Attack();
    }

    public interface ISwitchMode
    {
        public WeaponMode WeaponMode { get; }
        void SwitchMode(WeaponMode mode);
    }
 
    public interface IAnimateiable
    {
        public UnityEngine.Animator thisAnimator { get; }
    }

    public interface IAmmoBasedWeapon 
    {
        public float CurrentAmmo { get; set;}
        public float MaximumAmmo  { get; set;}
    }
    
    public enum WeaponMode
    {
        ModeOne,
        ModeTwo,
        ModeThree
    }
}