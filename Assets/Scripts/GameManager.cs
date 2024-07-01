using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour {
    [Header("References")]
    public GameObject player;
    [SerializeField] private TMP_Text timeElapsedText;
    [SerializeField] private TMP_Text catnipText;
    [SerializeField] private GameObject catnipPrefab;
    [SerializeField] private GameOverCanvas gameOverCanvas;
    [SerializeField] private AudioSource catnipAudioSource;
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private Canvas joystickCanvas;

    [Header("Settings")]
    [Tooltip("The amount of seconds before starting the game.")]
    [SerializeField] private float startTime;
    [Tooltip("The minimum distance from the player an enemy can spawn.")]
    [SerializeField] private float minimumEnemySpawnDistance;
    [Tooltip("The maximum distance from the player an enemy can spawn.")]
    [SerializeField] private float maximumEnemySpawnDistance;
    [Tooltip("The amount of seconds a wave lasts before going to the next.")]
    [SerializeField] private float waveDuration;

    [Serializable]
    private class Wave {
        [Tooltip("The prefbas of enemies being spawned this wave.")]
        public GameObject[] enemyPrefabs;
        [Tooltip("The minimum amount of enemies to have at once during this wave.")]
        public int minimumCount;
        [Tooltip("The amount of seconds in between enemy spawns this wave.")]
        public float spawnInterval;
    }

    [SerializeField] private Wave[] waves;

    public int waveIndex { get; private set; } = -1;
    [HideInInspector] public int enemyCount = 0;
    public int catnip { get; private set; } = 0;

    public static GameManager Instance { get; private set; }

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool IsMobile();

    public bool isMobile() {
#if !UNITY_EDITOR && UNITY_WEBGL
        return IsMobile();
#endif
        return false;
    }

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
        if (isMobile()) {
            joystickCanvas.enabled = true;
        }

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

        StartCoroutine(TimeElapsedCoroutine());
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy() {
        if (waveIndex < waves.Length-1) waveIndex++;

        int spawnIntervalAmount = (int)((1/waves[waveIndex].spawnInterval) * waveDuration);
        for (int i = 0; i < spawnIntervalAmount; i++) {
            if (enemyCount < waves[waveIndex].minimumCount) {
                for (int j = 0; j < waves[waveIndex].enemyPrefabs.Length; j++) {
                    Vector2 randomPoint = RandomPointInAnnulus(player.transform.position, minimumEnemySpawnDistance, maximumEnemySpawnDistance);
                    Instantiate(waves[waveIndex].enemyPrefabs[j], randomPoint, Quaternion.identity);
                    enemyCount++;
                }
            }
            yield return new WaitForSeconds(waves[waveIndex].spawnInterval);
        }

        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator TimeElapsedCoroutine() {
        int secondsElapsed = 0;

        while (this) {
            yield return new WaitForSeconds(1);

            secondsElapsed++;
            timeElapsedText.text = $"Time Elapsed: {GetTimeAsString(secondsElapsed)}";
        }
    }

    private string GetTimeAsString(float seconds) {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        if (seconds >= 3600f) {
            return timeSpan.ToString(@"hh\:mm\:ss");
        } else {
            return timeSpan.ToString(@"mm\:ss");
        }
    }

    public Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius, float maxRadius) {
        var randomDirection = (UnityEngine.Random.insideUnitCircle).normalized;
        var randomDistance = UnityEngine.Random.Range(minRadius, maxRadius);
        var point = origin + randomDirection * randomDistance;
        return point;
    }

    public void EnemyKilled(Vector3 enemyDeathPosition) {
        enemyCount--;
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
