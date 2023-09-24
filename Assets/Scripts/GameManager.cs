using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Settings")]
    [Tooltip("The amount of seconds before starting the game.")]
    [SerializeField] private float startTime;
    [Tooltip("The minimum distance from the player an enemy can spawn.")]
    [SerializeField] private float minimumEnemySpawnDistance;
    [Tooltip("The maximum distance from the player an enemy can spawn.")]
    [SerializeField] private float maximumEnemySpawnDistance;
    [Tooltip("The base amount of enemies that will spawn in a wave.")]
    [SerializeField] private float baseEnemySpawnCount;
    [Tooltip("The ratio increase of enemy spawns per wave count.")]
    [SerializeField] private float ratioEnemySpawnCount;

    public int wave { get; private set; } = 0;
    [HideInInspector] public int enemyCount = 0;

    private GameObject player;

    public static GameManager Instance { get; private set; }

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

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
        wave++;

        for (int i = 0; i < baseEnemySpawnCount + (ratioEnemySpawnCount * wave); i++) {
            while (this) {
                Vector2 randomPoint = RandomPointInAnnulus(player.transform.position, minimumEnemySpawnDistance, maximumEnemySpawnDistance);
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 1f, NavMesh.AllAreas)) {
                    Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], hit.position, Quaternion.identity);
                    break;
                }

                yield return null;
            }
            enemyCount++;
        }

        while (this) {
            if (enemyCount == 0) {
                StartCoroutine(SpawnEnemy());
                break;
            }

            yield return null;
        }
    }

    public Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius) {
        var randomDirection = (Random.insideUnitCircle * origin).normalized;
        var randomDistance = Random.Range(minRadius, maxRadius);
        var point = origin + randomDirection * randomDistance;
        return point;
    }

}
