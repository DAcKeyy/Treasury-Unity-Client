using Movement;
using UnityEngine;

namespace CharacterBehaviours
{
    public class Player: Character
    {
        [SerializeField] private Pointer Pointer;

        private void FixedUpdate()
        {
            CharacterView.Walk(!Pointer.isCalm);
            if(!Pointer.isCalm) GoToPoint(Pointer.transform.position);
        }
        
        public override void AttackTargets(Transform[] targets)
        {
            if (targets.Length == 0)
            {
                CharacterView.ShootTarget = null;
                return;
            }
            
            if(CharacterView.IsWallking) return;
            
            var target = GetClosestTransform(targets);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            CharacterView.ShootTarget = target;
            
            if(CharacterView.IsAttacking == false) 
                CharacterView.Attack();
        }
    }
} 