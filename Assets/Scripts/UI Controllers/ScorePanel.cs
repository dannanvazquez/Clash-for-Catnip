using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetScoreInfo(string name, int score) {
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
