using UnityEngine;

namespace Weapons
{
    public class Bow : RangeWeapon
    {
        [SerializeField] private Arrow _arrow;

        public override void Shoot()
        {
            var arrow = Instantiate(_arrow, _arrow.transform);
            arrow.Damage = WeaponData.damage;
            arrow.transform.parent = null;
            Debug.Log(transform.forward);
            arrow.Fly(transform.forward);
        }
    }
}