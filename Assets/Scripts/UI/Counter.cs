using System;
using TMPro;
using UnityEngine;

namespace Treasury.UI
{
    public class Counter : MonoBehaviour
    {
        [SerializeField] TMP_Text CountText = null;
        public bool isCounter = false;
        private int count;

        public int GetCountValue
        {
            get 
            { 
                if(isCounter)
                {
                    return count;
                }
                else
                {
                    return -1488;
                }
            }
        }

        public void OnEnable()
        {
            if (CountText == null)
                CountText = this.GetComponent<TMP_Text>();
        }

        public void Increment()
        {
            count++;
            CountText.text = count.ToString();
        }
        public void Decrement()
        {
            count--;
            CountText.text = count.ToString();
        }

        public void SetText(string value)
        {
            CountText.text = value.ToString();
            if (isCounter) count = Convert.ToInt32(CountText.text);
        }
        public void SetNumber(int value)
        {
            CountText.text = value.ToString();
            if (isCounter) count = Convert.ToInt32(CountText.text);
        }
        public void SetNumber(float value)
        {
            CountText.text = value.ToString();
            if (isCounter) count = Convert.ToInt32(CountText.text);
        }
    }
}