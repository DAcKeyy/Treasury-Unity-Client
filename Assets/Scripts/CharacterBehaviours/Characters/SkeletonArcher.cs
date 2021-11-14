using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterBehaviours
{
    [RequireComponent(typeof(CharacterVision))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterView))]
    public class SkeletonArcher : Character, IAI
    {
        [SerializeField] private float patrollingRateTime = 1f;
        private bool isPatrolling;
        
        public override void Start()
        {
            base.Start();
            StartCoroutine(GetRandomPoint());
        }

        private void FixedUpdate()
        {
            _characterView.Walk(thisAgent.remainingDistance > 0.1f);
            isPatrolling = isCalm;
        }
        
        public override void AttackTargets(Transform[] targets)
        {
            if(targets.Length == 0) return;
            
            thisAgent.isStopped = true;
            _characterView.Walk(false);

            var target = GetClosestTransform(targets);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            
            if(_characterView.IsAttacking == false) 
                _characterView.Attack();
        }

        IEnumerator GetRandomPoint()
        {
            while (true)
            {
                if (isPatrolling)
                {
                    Patrolling();
                    yield return new WaitForSecondsRealtime(patrollingRateTime);
                }

                yield return null;
            }
        }

        public void Patrolling()
        {
            var walkRadius = 5;
            thisAgent.isStopped = false;
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            GoToPoint(hit.position);
        }
    }
}