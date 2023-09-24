using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform healthBarTransform;
    [SerializeField] private RectMask2D healthMask;
    [SerializeField] private TMP_Text healthText;

    [Header("Settings")]
    [Tooltip("The base amount of health this entity will have.")]
    [SerializeField] private int healthBase;
    [Tooltip("The increase amount of health this entity will gain per wave.")]
    [SerializeField] private int healthIncrease;

    public int currentHealth { get; private set; }
    private int maxHealth;

    private void Start() {
        maxHealth = healthBase + healthIncrease * GameManager.Instance.wave;
        currentHealth = maxHealth;
    }

    private void Update() {
        healthBarTransform.rotation = Quaternion.identity;
    }

    public void TakeDamage(int damage) {
        if (currentHealth - damage <= 0) {
            currentHealth = 0;
            UpdateHealthBar();
            Death();
        } else {
            currentHealth -= damage;
            UpdateHealthBar();
        }

    }

    public void Heal(int healAmount) {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        if (healthMask) {
            float paddingZ = 960 - (960 * ((float)currentHealth / (float)maxHealth));
            healthMask.padding = new Vector4(0, 0, paddingZ, 0);
        }

        if (healthText) {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    private void Death() {
        switch (gameObject.tag) {
            case "Enemy":
                GameManager.Instance.EnemyKilled(transform.position);
                break;
            case "Player":
                GameManager.Instance.GameOver();
                break;
        }

        Destroy(gameObject);
    }
}
