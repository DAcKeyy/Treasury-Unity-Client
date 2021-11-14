using System.Collections;
using Data;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterBehaviours
{
    [RequireComponent(typeof(CharacterVision))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterView))]
    public class Character : MonoBehaviour, ICharacter
    {
        public CharacterSettings CharacterData;
        protected NavMeshAgent thisAgent;
        protected CharacterView _characterView;
        protected CharacterVision _characterVision;
        protected bool isCalm;

        public virtual void Start()
        {
            isCalm = true;
            thisAgent = GetComponent<NavMeshAgent>();
            
            _characterView = GetComponent<CharacterView>();
            _characterView.Setup(CharacterData.weaponData);
            
            _characterVision = GetComponent<CharacterVision>();
            _characterVision.TargetFounded += AttackTargets;

            StartCoroutine(CheckTargets());
        }

        private IEnumerator CheckTargets()
        {
            while (true)
            {
                if (_characterVision.visableTargets.Count == 0) isCalm = true;
                else isCalm = false;
                
                yield return new WaitForFixedUpdate();
            }
        }
        
        public virtual void AttackTargets(Transform[] targets)
        {
            GoToPoint(GetClosestTransform(targets).position);
        }

        public void GoToPoint(Vector3 point)
        {
            thisAgent.SetDestination(point);
        }

        public void TakeDamage(int damageValue)
        {
            throw new System.NotImplementedException();
        }
        
        public void Die()
        {
            //Poooff aniamtion!
            //Drop Coins
            Destroy(gameObject);
        }

        protected Transform GetClosestTransform(Transform[] targets)
        {
            if(targets == null) return null;
            
            var closestTargetDistance = Mathf.Infinity;
            Transform closestTarget = transform;
            
            foreach (var target in targets)
            {
                var distance = Vector3.Distance(transform.position, target.position);
                
                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }
    }
}