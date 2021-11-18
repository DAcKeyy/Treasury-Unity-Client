using System.Linq;
using Data;
using UnityEngine;
using Weapons;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform hand;
    private WeaponSettings _equippedWeaponData;
    private Weapon weaponInstance;
    public Transform ShootTarget { get; set; }
    public bool IsAttacking { get => isAttacking;  }
    public bool IsWallking { get => isWallking; }
    private bool isAttacking;
    private bool isWallking;

    public void StopAttacking()
    {
        isAttacking = false;
    }
    public void Setup(WeaponSettings weaponSettings)
    {
        _equippedWeaponData = weaponSettings;
        animator.SetInteger("WeaponID", (int) weaponSettings.type);
        weaponInstance = Instantiate(_equippedWeaponData.Weapon, hand);
    }
    
    public void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    public void Walk(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
        isWallking = isWalking;
    }

    public void TakeDamage()
    {
        animator.SetTrigger("Take Damage");
    }

    public void Shoot()
    {
        if (weaponInstance.GetType().GetInterfaces().Contains(typeof(IAutoAimable)))
            weaponInstance.gameObject.GetComponent<IAutoAimable>().Target = ShootTarget.position;
        
        if (weaponInstance.GetType().GetInterfaces().Contains(typeof(IShootable)))
            weaponInstance.gameObject.GetComponent<IShootable>().Shoot();
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
