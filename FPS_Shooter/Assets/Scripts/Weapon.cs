using System;
using System.Diagnostics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData weaponData;
    private float damage;
    private float range;
    private float fireRate;

    public AudioSource shootSound;

    public Camera shootCamera;
    public ParticleSystem muzzleflash;

    private float nextTimeToFire = 0f;

    private void Start()
    {
        damage = weaponData.damage;
        range = weaponData.range;
        fireRate = weaponData.fireRate;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();

            if(shootSound != null)
            {
                shootSound.Play();
            }
        }

    }

    void Shoot()
    {
        muzzleflash.Play();
        

        RaycastHit hit;
        if (Physics.Raycast(shootCamera.transform.position, shootCamera.transform.forward, out hit, range))
        {
            

            // Check if the hit object is a zombie (you should use a proper tag or a specific component to identify zombies).
            ZombieStateMachine zombie = hit.transform.GetComponent<ZombieStateMachine>();
            if (zombie != null)
            {
                // Apply damage to the zombie.
                zombie.TakeDamage(damage);
            }
        }
    }

}