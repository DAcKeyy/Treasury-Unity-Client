using System;
using Data;
using UnityEngine;

namespace Weapons
{
    public abstract class RangeWeapon : MonoBehaviour, IShootable, IAnimateiable, IWeapon, IChargeable
    {
        public bool IsCharged
        {
            get => _isLoaded;
            set => _isLoaded = value;
        }
        public bool IsReady
        {
            get => _isReady;
            set => _isReady = value;
        }

        public float ShootDelay
        {
            get => _shootDelay;
            set => _shootDelay = value;
        }

        public float RecoilForce
        {
            get => _recoilForce;
            set => _recoilForce = value;
        }

        public Transform BulletSpawnPoint { get; set; }
        public GameObject BulletPrefab { get; set; }
        public WeaponMode WeaponMode => weaponMode;
        public Animator thisAnimator => animator;
        
        [SerializeField] private WeaponSettings weaponSettings;
        [SerializeField] private AudioSource shootSound;
        [SerializeField] private AudioSource reloadSound;
        [SerializeField] private AudioSource switchModeSound;
        [SerializeField] private AudioSource pickUpSound;
        
        private bool _isReady;
        private bool _isLoaded;
        private float _shootDelay = 1;
        private float _recoilForce = 1;
        public WeaponMode weaponMode;
        public Animator animator;
        
        public virtual void Shoot()
        {
            if (IsReady) animator.SetTrigger(AnimationTags.SHOOT_TRIGGER);
        }
        
        public virtual void Reload()
        {
            animator.SetTrigger(AnimationTags.RELOAD_TRIGGER);
        }

        public virtual void SwitchMode(WeaponMode mode)
        {
            weaponMode = mode;
            //animator.SetTrigger(AnimationTags.SWITCH_MODE_TRIGGER);
        }

        public WeaponSettings WeaponData { get => weaponSettings; set => weaponSettings = value;}
        
        public void Show()
        {
            
        }

        public void Hide()
        {
            
        }

        public void Set_NotReady()
        {
            IsReady = true;
        }

        public void Set_Ready()
        {
            IsReady = false;
        }

        public void PlaySound_Shoot()
        {
            if (shootSound != null) shootSound.Play();
        }

        public void PlaySound_Reload()
        {
            if (reloadSound != null) reloadSound.Play();
        }

        public void PlaySound_SwitchMode()
        {
            if (switchModeSound != null) switchModeSound.Play();
        }
        
        public void PlaySound_PickUp()
        {
            if (pickUpSound != null) pickUpSound.Play();
        }

        public virtual void Charge()
        {
            animator.SetTrigger(AnimationTags.CHARGE_TRIGGER);
        }
    }
}