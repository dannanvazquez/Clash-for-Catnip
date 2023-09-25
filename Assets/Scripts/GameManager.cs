using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [Header("References")]
    public GameObject player;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text enemiesRemainingText;
    [SerializeField] private TMP_Text catnipText;
    [SerializeField] private GameObject catnipPrefab;
    [SerializeField] private GameOverCanvas gameOverCanvas;
    [SerializeField] private AudioSource catnipAudioSource;
    [SerializeField] private Canvas pauseCanvas;

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
    public int catnip { get; private set; } = 0;

    public static GameManager Instance { get; private set; }

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        if (MusicManager.Instance) MusicManager.Instance.StartGameMusic();
    }

    private void Start() {
        StartCoroutine(StartGame());
    }

    private void Update() {
        if (gameOverCanvas.GetComponent<Canvas>().enabled == false) {
            if (Input.GetButtonDown("Cancel")) {
                TogglePause();
            }
        }
    }

    public void TogglePause() {
        if (Time.timeScale == 0) {
            Time.timeScale = 1;
            pauseCanvas.enabled = false;
            MusicManager.Instance.audioSource.UnPause();
        } else {
            Time.timeScale = 0;
            pauseCanvas.enabled = true;
            MusicManager.Instance.audioSource.Pause();
        }
    }

    private IEnumerator StartGame() {
        yield return new WaitForSeconds(startTime);

        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy() {
        wave++;
        waveText.text = $"Wave: {wave}";

        for (int i = 0; i < baseEnemySpawnCount + (ratioEnemySpawnCount * wave); i++) {
            while (this) {
                Vector2 randomPoint = RandomPointInAnnulus(player.transform.position, minimumEnemySpawnDistance, maximumEnemySpawnDistance);
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 0.1f, NavMesh.AllAreas)) {
                    Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], hit.position, Quaternion.identity);
                    break;
                }

                yield return null;
            }
            enemyCount++;
            enemiesRemainingText.text = $"Enemies Remaining: {enemyCount}";
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

    public void EnemyKilled(Vector3 enemyDeathPosition) {
        enemyCount--;
        enemiesRemainingText.text = $"Enemies Remaining: {enemyCount}";
        GameObject catnip = Instantiate(catnipPrefab, enemyDeathPosition, Quaternion.identity);
        catnip.GetComponent<BobbingEffect>().canBob = true;
    }

    public void PickedUpCatnip(int healAmount) {
        catnip++;
        catnipText.text = $"Catnip: {catnip}";
        player.GetComponent<Health>().Heal(healAmount);
        catnipAudioSource.Play();
    }

    public void GameOver() {
        gameOverCanvas.EnableCanvas();
    }
}
