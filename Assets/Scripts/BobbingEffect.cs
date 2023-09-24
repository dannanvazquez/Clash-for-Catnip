using UnityEngine;

public class BobbingEffect : MonoBehaviour {
    [Header("Settings")]
    [Tooltip("The speed of the bobbing.")]
    [SerializeField] private float speed;
    [Tooltip("The max height the bobbing will go.")]
    [SerializeField] private float height;

    [HideInInspector] public bool canBob = false;

    private float initialY;

    void Start() {
        initialY = transform.position.y;
    }

    void Update() {
        if (!canBob) return;

        transform.position = new Vector3(transform.position.x, initialY + Mathf.Cos(Time.time * speed) * height, transform.position.z);
    }
}
