using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Joystick shootingJoystick;

    [Header("Settings")]
    [Tooltip("The speed of the player movement.")]
    [SerializeField] private float moveSpeed;

    private Rigidbody2D rb;

    private Vector2 moveDirection;
    private Vector2 aimDirection;

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
        float moveX;
        float moveY;
        if (!GameManager.Instance.isMobile()) {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        } else {
            moveX = movementJoystick.Horizontal;
            moveY = movementJoystick.Vertical;
        }

        if ((Input.GetButtonDown("Fire1") || GameManager.Instance.isMobile()) && Time.timeScale != 0) {
            weapon.Fire();
        }

        moveDirection = new Vector2(moveX, moveY).normalized;
        if (!GameManager.Instance.isMobile()) {
            aimDirection = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            aimDirection -= rb.position;
        } else {
            aimDirection = shootingJoystick.Direction;
        }
    }

    private void Move() {
        rb.velocity = moveDirection * moveSpeed;

        // Rotate player to follow mouse
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
}
