using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RangedEnemy : MonoBehaviour {
    [Header("References")]
    [SerializeField] private EnemyWeapon enemyWeapon;

    [Header("Settings")]
    [Tooltip("The increase amount of speed this enemy will gain per wave.")]
    [SerializeField] private float speedIncrease;

    private Transform playerTransform;
    private NavMeshAgent agent;

    private void Awake() {
        playerTransform = GameManager.Instance.player.transform;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        agent.speed += speedIncrease * GameManager.Instance.wave;
    }

    private void Update() {

        // TODO: Check if enemy has a clear line of sight to hit. No obstacles right now so it doesn't matter at the moment.
        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if ((distance > enemyWeapon.shootRange && !enemyWeapon.isInShootingState) || (distance > enemyWeapon.deaggroRange && enemyWeapon.isInShootingState)) {
            // Walk towards the player to get into shooting range.
            agent.isStopped = false;
            enemyWeapon.isInShootingState = false;
            agent.destination = playerTransform.position;

            // Rotate enemy to follow player
            Vector3 dir = agent.steeringTarget - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } else {
            // Stop and shoot at the player since we are in range.
            agent.isStopped = true;

            Vector3 dir = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            enemyWeapon.Fire();
        }

    }
}
