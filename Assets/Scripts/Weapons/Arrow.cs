using System.Collections;
using UnityEngine;

namespace Weapons
{
    [RequireComponent(typeof(BoxCollider))]
    public class Arrow : MonoBehaviour, IBullet
    {
        [SerializeField] private float speedMultiplier;

        public int Damage { get; set; }

        public void Fly(Vector3 destanation)
        {
            StartCoroutine(FlyCoroutine());
        }

        private IEnumerator FlyCoroutine()
        {
            while (true)
            {
                transform.position += transform.forward * speedMultiplier; 
                yield return null;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            StopCoroutine(FlyCoroutine());
            transform.parent = other.transform;
        }
    }
}