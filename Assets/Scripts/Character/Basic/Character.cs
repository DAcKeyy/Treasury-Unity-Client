using System;
using System.Collections;
using Treasury.Data;
using Treasury.Weapons;
using UnityEngine;
using UnityEngine.AI;

namespace Treasury.Character.Basic
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(CharacterVision))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Character : MonoBehaviour, ICharacter
    {
        public Action<int> HealthChanged = delegate(int i) {  };
        public Action<Character> Died = delegate {  };
        public CharacterSettings characterData;
        protected NavMeshAgent ThisAgent;
        [SerializeField]
        protected CharacterView CharacterView;
        protected CharacterVision CharacterVision;
        protected bool IsCalm;
        private int health;
        
        public virtual void Start()
        {
            IsCalm = true;
            ThisAgent = GetComponent<NavMeshAgent>();
            
            CharacterView.Setup(characterData.weaponData);
            health = characterData.hp;
            CharacterVision = GetComponent<CharacterVision>();
            CharacterVision.TargetFounded += AttackTargets;

            StartCoroutine(CheckTargets());
        }

        public void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.gameObject.tag);
            if (other.gameObject.tag == "Arrow")
            {
                health -= other.gameObject.GetComponent<Arrow>().Damage;
                HealthChanged(health);
                if(health < 0) Die();
            }
        }

        private IEnumerator CheckTargets()
        {
            while (true)
            {
                if (CharacterVision.VisableTargets.Count == 0) IsCalm = true;
                else IsCalm = false;
                
                yield return new WaitForFixedUpdate();
            }
        }
        
        public virtual void AttackTargets(Transform[] targets)
        {
            GoToPoint(GetClosestTransform(targets).position);
        }

        public void GoToPoint(Vector3 point)
        {
            ThisAgent.SetDestination(point);
        }

        public void TakeDamage(int damageValue)
        {
            throw new System.NotImplementedException();
        }
        
        public void Die()
        {
            //TODO: Poooff aniamtion!
            //TODO: Drop Coins
            Died(this);
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