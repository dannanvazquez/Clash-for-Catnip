using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Transform healthBarTransform;
    [SerializeField] private RectTransform[] healthRectTransforms;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private AudioSource hitAudioSource;
    [SerializeField] private AudioClip[] hitClips;

    [Header("Settings")]
    [Tooltip("The base amount of health this entity will have.")]
    [SerializeField] private int healthBase;

    public int currentHealth { get; private set; }

    private void Start() {
        currentHealth = healthBase;
    }

    private void Update() {
        if (healthBarTransform) {
            healthBarTransform.rotation = Quaternion.identity;
        }
    }

    public void TakeDamage(int damage) {
        if (hitAudioSource != null && hitClips.Length > 0) {
            hitAudioSource.clip = hitClips[Random.Range(0, hitClips.Length)];
            hitAudioSource.Play();
        }

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
        if (currentHealth > healthBase) currentHealth = healthBase;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        if (healthRectTransforms.Length > 0) {
            foreach (RectTransform rect in healthRectTransforms) {
                Vector3 updatedLocalScale = rect.localScale;
                updatedLocalScale.x = (float)currentHealth / (float)healthBase;
                rect.localScale = updatedLocalScale;
            }
        }

        if (healthText) {
            healthText.text = $"{currentHealth} / {healthBase}";
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
