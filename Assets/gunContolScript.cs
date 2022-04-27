using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunContolScript : MonoBehaviour
{
    public Gun curGun;
    public GameObject gunHolder;
    public Gun foldbackGun;

    public LayerMask ground;

    public plrXGunInteraction pXGI;
    private AudioSource AS;

    public LayerMask thisCollisonBullet;
    
    public int ammo;
    public float cooldown = 0;

    private void Start()
    {
        AS = gunHolder.GetComponent<AudioSource>();
    }

    private void Update()
    {
        // if the gun is automatic, use the getMouseButtonDown, else you need to press to shoot
        bool doShoot = curGun.automatic ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        
        // reduce cooldown with delta time. To a minimum of 0
        cooldown = Mathf.Max(cooldown - Time.deltaTime, 0);

        // Check if the gun is within terrain, so that it dose not shoot thru the ground or the walls.
        if (Mathf.Abs(transform.localScale.x) == transform.localScale.x)
        {
            if (Physics2D.Raycast(gunHolder.transform.position, gunHolder.transform.right,
                    curGun.bulletOfsetPlrScale.x*2, ground).collider != null)
                doShoot = false;
        }
        else
        {
            if (Physics2D.Raycast(gunHolder.transform.position, gunHolder.transform.right*-1,
                    curGun.bulletOfsetPlrScale.x*2, ground).collider != null)
                doShoot = false;
        }
        
        // shoot
        if (doShoot && cooldown == 0)
        {
            // spawn a bullet
            GameObject bulletSystem = Instantiate(curGun.bullet, gunHolder.transform);
            bulletSystem.transform.localPosition = new Vector3(curGun.bulletOfsetPlrScale.x, curGun.bulletOfsetPlrScale.y, 0);
            bulletSystem.transform.parent = null;
            bulletSystem.transform.localScale = new Vector3(1, 1, 1);

            bulletSystem.GetComponent<genericBulletDamageScript>().damage = curGun.damage;

            // add collision layer from player
            var collision = bulletSystem.GetComponent<ParticleSystem>().collision;
            collision.collidesWith = thisCollisonBullet;

            // reset cooldown
            cooldown = curGun.fireRate;

            // if there is sound attatched, play it
            if (!(curGun.shootingAudio is null))
            {
                AS.clip = curGun.shootingAudio;
                AS.Play();
            }

            // only decrease if the current gun is not the foldback gun.
            if (curGun.name != foldbackGun.name)
            {
                ammo -= 1;
                if (ammo == 0)
                {
                    pXGI.setGun(foldbackGun);
                }
            }
        }
    }

    public void newGun(Gun g)
    {
        curGun = g;
        ammo = curGun.startAmmo;

        // if the gun is the foldback gun set ammo to 99, for visuals.
        if (curGun.name == foldbackGun.name)
            ammo = 99;
    }
}
