using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyWeapon : MonoBehaviour {
    [Header("References")]
    [SerializeField] private RangedEnemy rangedEnemy;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioClip[] fireClips;

    private AudioSource audioSource;

    [Header("Settings")]
    [Tooltip("The force applied to the bullet shooting outwards.")]
    [SerializeField] private float fireForce;
    [Tooltip("How many seconds in between shots.")]
    [SerializeField] private float fireRate;
    [Tooltip("The base amount of damage this enemy will do against the player per hit.")]
    [SerializeField] private int damageBase;
    [Tooltip("The range at which the enemy will shoot at from the player.")]
    public int shootRange;
    [Tooltip("The range at which the enemy will start walking towards the player after shooting.")]
    public int deaggroRange;

    private bool canShoot = true;

    [HideInInspector] public bool isInShootingState = false;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire() {
        if (!canShoot) return;

        isInShootingState = true;

        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        projectile.GetComponent<Bullet>().damage = damageBase;
        projectile.GetComponent<Bullet>().belongsToPlayer = false;

        audioSource.clip = fireClips[Random.Range(0, fireClips.Length)];
        audioSource.Play();

        StartCoroutine(FireCooldown());
    }

    IEnumerator FireCooldown() {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
