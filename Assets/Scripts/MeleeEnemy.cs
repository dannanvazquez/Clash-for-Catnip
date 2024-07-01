using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
public class MeleeEnemy : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The base amount of damage this enemy will do against the player per hit.")]
    [SerializeField] private int damageBase;
    [Tooltip("The amount of seconds before this enemy can hit again.")]
    [SerializeField] private int hitCooldown;
    [Tooltip("The movement speed of this enemy.")]
    [SerializeField] private float moveSpeed;

    private Transform playerTransform;
    private Rigidbody2D rb;

    private bool canHit = true;

    private void Awake() {
        playerTransform = GameManager.Instance.player.transform;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (playerTransform == null) return;

        Vector3 dir = playerTransform.position - transform.position;
        rb.velocity = dir.normalized * moveSpeed;

        // Rotate enemy to follow player
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (canHit && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Health health)) {
            StartCoroutine(HitCooldown());
            health.TakeDamage(damageBase);
        }
    }

    public IEnumerator HitCooldown() {
        canHit = false;
        yield return new WaitForSeconds(hitCooldown);
        canHit = true;
    }
}
