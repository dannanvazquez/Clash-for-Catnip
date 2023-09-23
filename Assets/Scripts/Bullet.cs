using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject impactEffectPrefab;

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
                // TODO: Deal damage to enemy
                Impact();
                break;
        }
    }

    public void Impact() {
        Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
