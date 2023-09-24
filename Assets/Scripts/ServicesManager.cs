using LootLocker;
using LootLocker.Requests;
using TMPro;
using UnityEngine;

public class ServicesManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text loginInfoText;
    [SerializeField] private LeaderboardCanvas leaderboardCanvas;

    public delegate void OnLogin(LootLockerGuestSessionResponse response);
    public static event OnLogin onLogin;

    private void Start() {
        StartGuestSession();
    }

    public void StartGuestSession() {
        LootLockerSDKManager.StartGuestSession((response) => {
            if (response.success) {
                Debug.Log("Successfully started LootLocker session.");
                loginInfoText.text = "Connected to services successfully.";

                PlayerPrefs.SetInt("PlayerID", response.player_id);

                leaderboardCanvas.InitializeTopScores();
            } else {
                Debug.Log("Error starting LootLocker session.");
                loginInfoText.text = "Failed to connect to services.";
            }

            onLogin?.Invoke(response);
        });
    }
}
