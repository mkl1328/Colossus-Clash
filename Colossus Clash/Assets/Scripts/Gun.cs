using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject pistolBulletPrefab;
    public GameObject shotgunBulletPrefab;
    public float shootingRate = 2f;
    private float shootingCooldown = 0f;
    //start with the pistol
    private string selectedWeapon = "pistol";

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
            if(selectedWeapon == "pistol")
            {
                Instantiate(pistolBulletPrefab, firePoint.position, firePoint.rotation);
                shootingCooldown = 1f / shootingRate;
            }
            if(selectedWeapon == "shotgun")
            {
                Instantiate(shotgunBulletPrefab, firePoint.position, firePoint.rotation);
                Instantiate(shotgunBulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, 20)); // Rotate by 20 degrees around Z axis
                Instantiate(shotgunBulletPrefab, firePoint.position, firePoint.rotation * Quaternion.Euler(0, 0, -20)); // Rotate by -20 degrees around Z axis
                shootingCooldown = 1f / shootingRate;
            }
        }
    }

    public void SwitchWeapon()
    {
        //switch which gun is selected
        if(selectedWeapon == "pistol")
        {
            selectedWeapon = "shotgun";
            shootingRate = 1.5f;
        }
        else
        {
            selectedWeapon = "pistol";
            shootingRate= 2f;
        }
    }
}
