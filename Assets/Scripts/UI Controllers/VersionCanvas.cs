using TMPro;
using UnityEngine;

public class VersionCanvas : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text versionText;

    private void Awake() {
        versionText.text = $"v{Application.version}";
    }
}
