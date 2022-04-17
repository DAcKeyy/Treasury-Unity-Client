using System.Linq;
using Treasury.Data;
using Treasury.Weapons.Basic;
using UnityEngine;

namespace Treasury.Character.Basic
{
    public class CharacterView : MonoBehaviour
    {
        public Transform ShootTarget { get; set; }
        public bool IsAttacking => _isAttacking; 
        public bool IsWalking  => _isWalking; 
    
        [SerializeField] private Animator animator;
        [SerializeField] private Transform hand;
    
        private WeaponSettings _equippedWeaponData;
        private Weapon _weaponInstance;
        private bool _isAttacking;
        private bool _isWalking;

        public void StopAttacking()
        {
            _isAttacking = false;
        }
        public void Setup(WeaponSettings weaponSettings)
        {
            _equippedWeaponData = weaponSettings;
            animator.SetInteger("WeaponID", (int) weaponSettings.type);
            _weaponInstance = Instantiate(_equippedWeaponData.Weapon, hand);
        }
    
        public void Attack()
        {
            _isAttacking = true;
            animator.SetTrigger("Attack");
        }

        public void Walk(bool isWalking)
        {
            animator.SetBool("isWalking", isWalking);
            _isWalking = isWalking;
        }

        public void TakeDamage()
        {
            animator.SetTrigger("Take Damage");
        }

        public void Shoot()
        {
            if (_weaponInstance.GetType().GetInterfaces().Contains(typeof(IAutoAimable)))
                _weaponInstance.gameObject.GetComponent<IAutoAimable>().Target = ShootTarget.position;
        
            if (_weaponInstance.GetType().GetInterfaces().Contains(typeof(IShootable)))
                _weaponInstance.gameObject.GetComponent<IShootable>().Shoot();
        }
    
        public void ChargeWeapon()
        {
        
        
            /*
       
        if (weaponInstance.GetType().GetInterfaces().Contains(typeof(IChargeable)))
        {
            var chargableWeapon = weaponInstance.gameObject.GetComponent<IChargeable>();
            chargableWeapon.Charge();
        }
        */
        }
    
        public void Die()
        {
            //_animator.SetTrigger("Die");
        }
    }
}
