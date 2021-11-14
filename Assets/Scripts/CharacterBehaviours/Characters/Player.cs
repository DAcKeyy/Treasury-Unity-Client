using Movement;
using UnityEngine;

namespace CharacterBehaviours
{
    public class Player: Character
    {
        [SerializeField] private Pointer Pointer;

        private void FixedUpdate()
        {
            _characterView.Walk(!Pointer.isCalm);
            if(!Pointer.isCalm) GoToPoint(Pointer.transform.position);
        }
        
        public override void AttackTargets(Transform[] targets)
        {
            if(targets.Length == 0) return;
            if(_characterView.IsWallking) return;
            
            var target = GetClosestTransform(targets);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            
            if(_characterView.IsAttacking == false) 
                _characterView.Attack();
        }
    }
} 