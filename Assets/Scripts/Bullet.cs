using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject impactEffectPrefab;

    [HideInInspector] public int damage;
    [HideInInspector] public bool belongsToPlayer;

    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        switch (collision.tag) {
            case "Wall":
                Impact();
                break;
            case "Enemy":
                if (!belongsToPlayer) return;

                if (collision.TryGetComponent(out Health enemyHealth)) {
                    enemyHealth.TakeDamage(damage);
                }
                Impact();
                break;
            case "Player":
                if (belongsToPlayer) return;

                if (collision.TryGetComponent(out Health playerHealth)) {
                    playerHealth.TakeDamage(damage);
                }
                Impact();
                break;
        }
    }

    public void Impact() {
        Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
