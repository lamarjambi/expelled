using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public float maxHealth = 20f;

    public event System.Action OnHealthChanged;
    public event System.Action OnDeath;

    private float _currentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = maxHealth;
    }


    void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        OnHealthChanged?.Invoke();
        if (_currentHealth <= 0)
        {
            Die();
            OnDeath?.Invoke();
        }
    }
    void Heal(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > maxHealth)
            _currentHealth = maxHealth;
        OnHealthChanged?.Invoke();
    }

    void Die()
    {
        //Handle death logic here
    }
}
