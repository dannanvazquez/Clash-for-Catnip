using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class LeaderboardCanvas : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject scorePanelPrefab;
    [SerializeField] private Transform scoresContainerTransform;
    [SerializeField] private TMP_Text errorText;

    [Header("Settings")]
    [Tooltip("The amount of top scores displayed in the leaderboard.")]
    [SerializeField] private int topScoresCount;

    public void InitializeTopScores() {
        LootLockerSDKManager.GetScoreList("Catnip", topScoresCount, 0, (response) => {
            if (response.statusCode == 200) {
                if (response.items.Length > 0) {
                    for (int i = 0; i < topScoresCount; i++) {
                        GameObject score = Instantiate(scorePanelPrefab, scoresContainerTransform);
                        score.GetComponent<ScorePanel>().SetScoreInfo(response.items[i].player.name, response.items[i].score);
                    }

                    errorText.enabled = false;
                } else {
                    errorText.text = "It seems there are no scores submitted at the moment. Be the first!";
                }
            } else {
                Debug.Log("Failed to retrieve scores.");
                errorText.text = "Failed to retrieve scores.";
            }
        });
    }
}
