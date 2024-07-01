using UnityEngine;

public class GridSystem : MonoBehaviour {
    private Transform playerTransform;

    private void Start() {
        playerTransform = GameManager.Instance.player.transform;
    }

    private void Update() {
        if (playerTransform.position.x <= transform.position.x - 20) {
            transform.position += new Vector3(-40, 0, 0);
        } else if (playerTransform.position.x >= transform.position.x + 20) {
            transform.position += new Vector3(40, 0, 0);
        }

        if (playerTransform.position.y <= transform.position.y - 20) {
            transform.position += new Vector3(0, -40, 0);
        } else if (playerTransform.position.y >= transform.position.y + 20) {
            transform.position += new Vector3(0, 40, 0);
        }
    }
}
