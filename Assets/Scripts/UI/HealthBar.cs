using System;
using CharacterBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Counter hpCounter;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Character owner;
        private int maxHp;
        private int hp;

        public void Start()
        {
            Init(owner.CharacterData.hp);
            owner.HealthChanged += delegate(int i) { TakeDamage(hp - i);};
        }
        
        public void Init(int hp)
        {
            maxHp = hp;
            this.hp = hp;
            hpCounter.SetNumber(hp);
        }

        public void ChangeMaxHP(int hp)
        {
            maxHp = hp;
        }
        
        public void TakeDamage(int Damage)
        {
            hp -= Damage;
            hpCounter.SetNumber(hp);
            hpSlider.value = (float)hp / (float)maxHp;
        }
    }
}