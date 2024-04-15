using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int selectedWeapon = 0;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        // Check if the 'E' key is pressed to cycle through weapons
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedWeapon++;
            if (selectedWeapon >= transform.childCount) // If we exceed the number of weapons, loop back to the first
            {
                selectedWeapon = 0;
            }
        }

        // Apply the weapon selection if it has changed
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            // Activate the selected weapon and deactivate others
            weapon.gameObject.SetActive(i == selectedWeapon);
            i++;
        }
    }
}
