using Data;
using UnityEngine;

namespace Weapons
{
    public class Bow : Weapon , IShootable, IAnimateiable, IChargeable, IAutoAimable
    {
        [SerializeField] private Arrow _arrow;
        [SerializeField] private Animator animator;

        public bool IsCharged { get; set; }
        public float ShootDelay { get; set; }
        public float RecoilForce { get; set; }
        public Transform BulletSpawnPoint { get; set; }
        public GameObject BulletPrefab { get; set; }
        public Vector3 Target { get; set; }

        public void Shoot()
        {
            //Костыль так как не знаю как красиво закодить лук
            if (_arrow.gameObject.activeSelf == false)
                _arrow.gameObject.SetActive(true);
            
            var arrow = Instantiate(_arrow, _arrow.transform);
            arrow.Damage = WeaponData.damage;
            arrow.transform.parent = null;
            arrow.Fly(Target);
            
            _arrow.gameObject.SetActive(false);
        }

        public Animator thisAnimator { get => animator; }

        public void Charge()
        {
            thisAnimator.SetTrigger(AnimationTags.CHARGE_TRIGGER);
        }

        
    }
}