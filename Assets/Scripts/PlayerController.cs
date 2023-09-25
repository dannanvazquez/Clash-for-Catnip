using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Weapon weapon;

    [Header("Settings")]
    [Tooltip("The speed of the player movement.")]
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
        playerCamera.transform.position = transform.position + new Vector3(0, 0, -10f);
    }

    private void ProcessInputs() {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Fire1") && Time.timeScale != 0) {
            weapon.Fire();
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Move() {
        rb.velocity = moveDirection * moveSpeed;

        // Rotate player to follow mouse
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
}
