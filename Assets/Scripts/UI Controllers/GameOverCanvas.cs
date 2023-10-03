using LootLocker.Requests;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class GameOverCanvas : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text personalBestText;
    [SerializeField] private GameObject submitButton;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private Text nameInputText;
    [SerializeField] private GameObject setNamePanel;

    public void EnableCanvas() {
        int previousPersonalBest = (PlayerPrefs.HasKey("PersonalBestCatnip") ? PlayerPrefs.GetInt("PersonalBestCatnip") : 0);
        int currentScore = GameManager.Instance.catnip;

        if (currentScore > previousPersonalBest) {
            personalBestText.text = "You beat your personal best!";
            submitButton.SetActive(true);

            PlayerPrefs.SetInt("PersonalBestCatnip", currentScore);
        } else {
            personalBestText.text = "Personal best: " + previousPersonalBest;
        }

        GetComponent<Canvas>().enabled = true;

        if (MusicManager.Instance) MusicManager.Instance.StartGameOverMusic();
    }

    public void RestartGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void SubmitScore() {
        LootLockerSDKManager.GetPlayerName((response) => {
            if (response.success) {
                if (response.name != "") {
                    SendScoreToLeaderboard(response.name);
                } else {
                    setNamePanel.SetActive(true);
                }
            } else {
                Debug.Log("Failed to get player info: " + response.errorData.message);
                infoText.text = "Failed to get player info. Are you connected to services?";
            }
        });
    }

    public void SetName() {
        setNamePanel.SetActive(false);

        LootLockerSDKManager.SetPlayerName(nameInputText.text, (response) => {
            if (response.success) {
                SendScoreToLeaderboard(response.name);
            } else {
                Debug.Log("Failed to set player name: " + response.errorData.message);
                infoText.text = "Failed to set player name. Maybe the name is already taken? Try another name.";
            }
        });
    }

    public void SendScoreToLeaderboard(string playerName) {
        LootLockerSDKManager.SubmitScore(PlayerPrefs.GetInt("PlayerID").ToString(), GameManager.Instance.catnip, "Catnip", (response) =>
        {
            if (response.statusCode == 200) {
                infoText.text = $"Score has been submitted as {playerName}!";
            } else {
                Debug.Log("Failed to submit score: " + response.errorData.message);
                infoText.text = "Failed to submit score.";
            }
        });
    }
}
