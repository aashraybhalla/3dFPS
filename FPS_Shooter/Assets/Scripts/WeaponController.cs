using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private int currentWeaponIndex = 0;
    public List<Transform> weapons = new List<Transform>();

    void Start()
    {
        // Find and store all child weapons
        foreach (Transform child in transform)
        {
            weapons.Add(child);
            child.gameObject.SetActive(false);
        }

        // Activate the first weapon
        if (weapons.Count > 0)
        {
            currentWeaponIndex = 0;
            weapons[currentWeaponIndex].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        // Use the mouse scroll wheel to switch weapons
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheel != 0)
        {
            // Deactivate the current weapon
            weapons[currentWeaponIndex].gameObject.SetActive(false);

            // Change the weapon index based on scroll direction
            if (scrollWheel > 0)
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
            }
            else if (scrollWheel < 0)
            {
                currentWeaponIndex = (currentWeaponIndex - 1 + weapons.Count) % weapons.Count;
            }

            // Activate the new weapon
            weapons[currentWeaponIndex].gameObject.SetActive(true);
        }
    }
}
