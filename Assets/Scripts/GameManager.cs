using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Settings")]
    [Tooltip("The amount of seconds before starting the game.")]
    [SerializeField] private float startTime;
    [Tooltip("The amount of seconds between enemy spawns.")]
    [SerializeField] private float enemySpawnTimeInterval;
    [Tooltip("The minimum distance from the player an enemy can spawn.")]
    [SerializeField] private float minimumEnemySpawnDistance;
    [Tooltip("The maximum distance from the player an enemy can spawn.")]
    [SerializeField] private float maximumEnemySpawnDistance;

    private GameObject player;

    private void Awake() {
        player = FindFirstObjectByType<PlayerController>().gameObject;
    }

    private void Start() {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame() {
        yield return new WaitForSeconds(startTime);

        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy() {
        while (this) {
            Vector2 randomPoint = RandomPointInAnnulus(player.transform.position, minimumEnemySpawnDistance, maximumEnemySpawnDistance);
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas)) {
                Instantiate(enemyPrefab, hit.position, Quaternion.identity);
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(enemySpawnTimeInterval);

        StartCoroutine(SpawnEnemy());
    }

    public Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius) {
        var randomDirection = (Random.insideUnitCircle * origin).normalized;
        var randomDistance = Random.Range(minRadius, maxRadius);
        var point = origin + randomDirection * randomDistance;
        return point;
    }

}
