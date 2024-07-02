using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private RectTransform experienceBarRectTransform;
    [SerializeField] private TMP_Text levelText;

    public static HUDController Instance { get; private set; }

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public void UpdateLevelUI(int level) {
        levelText.text = level.ToString();
    }

    public void UpdateExperienceBarUI(float percentage) {
        Vector3 updatedLocalScale = experienceBarRectTransform.localScale;
        updatedLocalScale.x = percentage;
        experienceBarRectTransform.localScale = updatedLocalScale;
    }
}
