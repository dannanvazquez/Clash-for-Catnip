using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Joystick movementJoystick;
    [SerializeField] private Joystick shootingJoystick;
    [SerializeField] private Weapon weapon;

    [Header("Settings")]
    [Tooltip("The speed of the player movement.")]
    [SerializeField] private float moveSpeed;
    [Tooltip("The range from the player auto aim is triggered.")]
    [SerializeField] private float autoAimRange;

    private Rigidbody2D rb;

    private Vector2 moveDirection;
    private Vector2 aimDirection;

    private bool isAutoAiming = true;

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

        moveDirection = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.C)) {
            isAutoAiming = !isAutoAiming;
            shootingJoystick.gameObject.SetActive(!isAutoAiming);
        }

        if (isAutoAiming) {
            Vector3 closestEnemyPosition = GetClosestEnemyPosition();
            if (closestEnemyPosition != transform.position) {
                aimDirection = (closestEnemyPosition - weapon.transform.position).normalized;
            }
        } else {
            if (!GameManager.Instance.isMobile()) {
                Vector3 mousePosition = playerCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = transform.position.z;
                if (Vector3.Distance(transform.position, mousePosition) > 1f) {
                    aimDirection = mousePosition;
                    aimDirection -= new Vector2(weapon.transform.position.x, weapon.transform.position.y);
                } else {
                    aimDirection = mousePosition;
                    aimDirection -= rb.position;
                }
            } else {
                aimDirection = shootingJoystick.Direction;
            }
        }
    }

    private void Move() {
        rb.velocity = moveDirection * moveSpeed;

        // Rotate player to follow mouse
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    private Vector3 GetClosestEnemyPosition() {
        Vector3 closestPosition = transform.position;
        float closestDistance = float.PositiveInfinity;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, autoAimRange);
        foreach (var hitCollider in hitColliders) {
            if (hitCollider.CompareTag("Enemy")) {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance) {
                    closestPosition = hitCollider.transform.position;
                    closestDistance = distance;
                }
            }
        }

        return closestPosition;
    }
}
