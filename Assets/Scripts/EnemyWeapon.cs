using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {
    [Header("References")]
    [SerializeField] private RangedEnemy rangedEnemy;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Settings")]
    [Tooltip("The force applied to the bullet shooting outwards.")]
    [SerializeField] private float fireForce;
    [Tooltip("How many seconds in between shots.")]
    [SerializeField] private float fireRate;
    [Tooltip("The base amount of damage this enemy will do against the player per hit.")]
    [SerializeField] private int damageBase;
    [Tooltip("The increase amount of damage this enemy will gain per wave.")]
    [SerializeField] private int damageIncrease;
    [Tooltip("The range at which the enemy will shoot at from the player.")]
    public int shootRange;
    [Tooltip("The range at which the enemy will start walking towards the player after shooting.")]
    public int deaggroRange;

    private int damage;
    private bool canShoot = true;

    [HideInInspector] public bool isInShootingState = false;

    private void Awake() {
        damage = damageBase + (damageIncrease * GameManager.Instance.wave);
    }

    public void Fire() {
        if (!canShoot) return;

        isInShootingState = true;

        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        projectile.GetComponent<Bullet>().damage = damage;
        projectile.GetComponent<Bullet>().belongsToPlayer = false;

        StartCoroutine(FireCooldown());
    }

    IEnumerator FireCooldown() {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
