using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;

    private Vector2 moveDirection;
    private Vector2 mousePosition;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        ProcessInputs();
    }

    private void FixedUpdate() {
        Move();
    }

    private void ProcessInputs() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void Move() {
        rb.velocity = moveDirection * moveSpeed;
    }
}
