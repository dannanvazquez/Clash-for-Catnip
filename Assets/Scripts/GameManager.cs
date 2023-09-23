using UnityEngine;

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
        InvokeRepeating(nameof(SpawnEnemy), startTime, enemySpawnTimeInterval);
    }
    private void SpawnEnemy() {
        Instantiate(enemyPrefab, RandomPointInAnnulus(player.transform.position, minimumEnemySpawnDistance, maximumEnemySpawnDistance), Quaternion.identity);
    }

    public Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius) {
        var randomDirection = (Random.insideUnitCircle * origin).normalized;
        var randomDistance = Random.Range(minRadius, maxRadius);
        var point = origin + randomDirection * randomDistance;
        return point;
    }

}
