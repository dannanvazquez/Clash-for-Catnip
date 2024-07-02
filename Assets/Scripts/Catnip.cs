using UnityEngine;

public class Catnip : MonoBehaviour {
    [HideInInspector] public float exp;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GameManager.Instance.PickedUpCatnip(exp);
            Destroy(gameObject);
        }
    }
}
