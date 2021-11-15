using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace CharacterBehaviours
{
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
            CharacterView.Walk(ThisAgent.remainingDistance > 0.1f);
            isPatrolling = IsCalm;
        }

        public override void AttackTargets(Transform[] targets)
        {
            if (targets.Length == 0)
            {
                CharacterView.ShootTarget = null;
                return;
            }

            if (CharacterView.IsWallking) return;
            ThisAgent.isStopped = true;
            CharacterView.Walk(false);
            ThisAgent.ResetPath();
            var target = GetClosestTransform(targets);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));

            CharacterView.ShootTarget = target;

            if (CharacterView.IsAttacking == false)
                CharacterView.Attack();
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
            ThisAgent.isStopped = false;
            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            GoToPoint(hit.position);
        }
    }
}