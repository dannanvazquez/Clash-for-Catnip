using UnityEngine;

public class Weapon : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Settings")]
    [SerializeField] private float fireForce;

    public void Fire() {
        GameObject projectile = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);
    }
}
