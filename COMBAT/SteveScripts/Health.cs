using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class Health : MonoBehaviour
    {
        public Slider slider;
        public int health;
        private int initialHealth;
        public TextMeshProUGUI healthText;
        private void Start()
        {
            initialHealth = health;
            slider.maxValue = health;
            slider.value = health;
            healthText.text = health.ToString();
        }
        private void SetHealth(int value)
        {
            health = value;
            CalculateHealth();
            slider.value = health;
            healthText.text = health.ToString();
        }
        public void TakeDamage(int damage)
        {
            SetHealth(health-damage);
        }
        public void HealDamage(int damage)
        {
            SetHealth(health+damage);
        }
        private void CalculateHealth()
        {
            health = (int)Mathf.Clamp(health, 0, slider.maxValue);
        }
    }
}