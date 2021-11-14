using Data;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform hand;
    private WeaponSettings _equippedWeaponData;
    public bool IsAttacking { get => isAttacking; }
    public bool IsWallking { get => isWallking; }
    private bool isAttacking;
    private bool isWallking;

    public void Setup(WeaponSettings weaponSettings)
    {
        _equippedWeaponData = weaponSettings;
        animator.SetInteger("WeaponID", (int) weaponSettings.type);
        var weapon = Instantiate(_equippedWeaponData.prefub, hand);
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
        //_animator.SetTrigger("Take Damage");
    }

    public void Die()
    {
        //_animator.SetTrigger("Die");
    }
}
