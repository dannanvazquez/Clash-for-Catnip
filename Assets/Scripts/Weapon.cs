using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Weapon : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioClip[] fireClips;

    private AudioSource audioSource;

    [Header("Settings")]
    [Tooltip("The force applied to the bullet shooting outwards.")]
    [SerializeField] private float fireForce;
    [Tooltip("How many seconds in between shots.")]
    [SerializeField] private float fireRate;
    [Tooltip("How much damage the bullet will do.")]
    [SerializeField] private int damage;

    private bool canShoot = true;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        Fire();
    }

    private void Fire() {
        if (!canShoot) return;

        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
        projectile.GetComponent<Bullet>().damage = damage;
        projectile.GetComponent<Bullet>().belongsToPlayer = true;

        audioSource.clip = fireClips[Random.Range(0,fireClips.Length)];
        audioSource.Play();

        StartCoroutine(FireCooldown());
    }

    IEnumerator FireCooldown() {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
