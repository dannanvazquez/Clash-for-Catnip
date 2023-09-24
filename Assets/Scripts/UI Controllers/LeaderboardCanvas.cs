using UnityEngine;

public class LeaderboardCanvas : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject scorePanelPrefab;
    [SerializeField] private Transform scoresContainerTransform;

    [Header("Settings")]
    [Tooltip("The amount of top scores displayed in the leaderboard.")]
    [SerializeField] private int topScoresCount;

    private void Awake() {
        for (int i = 0; i < topScoresCount; i++) {
            GameObject score = Instantiate(scorePanelPrefab, scoresContainerTransform);
            score.GetComponent<ScorePanel>().SetScoreInfo("Test", 42);
        }
    }
}
