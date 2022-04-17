using UnityEngine;

namespace Treasury.Character.Basic
{
    public interface ICharacter
    {
        void AttackTargets(Transform[] target);
        void GoToPoint(Vector3 point);
        void TakeDamage(int damageValue);
        void Die();
    }
    
    public interface IAI
    {
        public void Patrolling();
    }
}