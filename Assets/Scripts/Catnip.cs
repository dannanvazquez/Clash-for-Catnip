using UnityEngine;

public class Catnip : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The amount of health picking this up will heal.")]
    public int heal;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GameManager.Instance.PickedUpCatnip(heal);
            Destroy(gameObject);
        }
    }
}
