using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The amount of damage this enemy will do against the player per hit.")]
    [SerializeField] private int damage;
    [Tooltip("The amount of seconds before this enemy can hit again.")]
    [SerializeField] private int hitCooldown;

    private Transform playerTransform;
    private NavMeshAgent agent;

    private bool canHit = true;

    private void Awake() {
        playerTransform = FindFirstObjectByType<PlayerController>().transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void Update() {
        agent.destination = playerTransform.position;

        // Rotate enemy to follow player
        Vector3 dir = agent.steeringTarget - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (canHit && collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Health health)) {
            StartCoroutine(HitCooldown());
            health.TakeDamage(damage);
        }
    }

    public IEnumerator HitCooldown() {
        canHit = false;
        yield return new WaitForSeconds(hitCooldown);
        canHit = true;
    }
}