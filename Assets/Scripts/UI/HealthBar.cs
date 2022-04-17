using UnityEngine;
using UnityEngine.UI;

namespace Treasury.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Counter hpCounter;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Character.Basic.Character owner;
        private int maxHp;
        private int hp;

        public void Start()
        {
            Init(owner.characterData.hp);
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