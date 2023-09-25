using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class MusicManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip gameOverMusic;

    private AudioSource audioSource;

    public static MusicManager Instance { get; private set; }

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void StartMainMenuMusic() {
        audioSource.clip = mainMenuMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StartGameMusic() {
        audioSource.clip = gameMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StartGameOverMusic() {
        audioSource.clip = gameOverMusic;
        audioSource.loop = false;
        audioSource.Play();
    }
}
