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
    [Tooltip("The maximum amount of health this entity has.")]
    [SerializeField] private int maxHealth;

    public int currentHealth { get; private set; }

    private void Awake() {
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
        Destroy(gameObject);
    }
}
