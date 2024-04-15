using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float shootingRate = 2f;
    private float shootingCooldown = 0f;

    void Update()
    {
        if (shootingCooldown > 0)
        {
            shootingCooldown -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && shootingCooldown <= 0)
        {
            Shoot();
            shootingCooldown = 1f / shootingRate;
        }
    }

    public void Shoot()
    {
        if (shootingCooldown <= 0f)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            shootingCooldown = 1f / shootingRate;
        }
    }
}
