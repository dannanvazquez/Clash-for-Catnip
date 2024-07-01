using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
public class RangedEnemy : MonoBehaviour {
    [Header("References")]
    [SerializeField] private EnemyWeapon enemyWeapon;

    [Header("Settings")]
    [Tooltip("The movement speed of this enemy.")]
    [SerializeField] private float moveSpeed;

    private Transform playerTransform;
    private Rigidbody2D rb;

    private void Awake() {
        playerTransform = GameManager.Instance.player.transform;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (playerTransform == null) return;

        // TODO: Check if enemy has a clear line of sight to hit. No obstacles right now so it doesn't matter at the moment.
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if ((distance > enemyWeapon.shootRange && !enemyWeapon.isInShootingState) || (distance > enemyWeapon.deaggroRange && enemyWeapon.isInShootingState)) {
            enemyWeapon.isInShootingState = false;

            // Walk towards the player to get into shooting range.
            Vector3 dir = playerTransform.position - transform.position;
            rb.velocity = dir.normalized * moveSpeed;

            // Rotate enemy to follow player
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } else {
            // Stop and shoot at the player since we are in range.
            Vector3 dir = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            enemyWeapon.Fire();
        }

    }
}
