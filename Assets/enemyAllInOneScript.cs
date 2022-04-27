using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using UnityEngine;

public class enemyAllInOneScript : MonoBehaviour
{
    public Sprite[] possibleSprites;
    
    public Gun enemysGun;
    public float maxHP;
    public float HP;

    public float difficulty = 0.2f;

    public GameObject aim;
    public GameObject gunHolder;
    
    private float shootCooldown;

    public LayerMask thisCollisonBullet;
    public GameObject player;

    private void Start()
    {
        gunHolder.GetComponent<SpriteRenderer>().sprite = enemysGun.theImageSprite;

        gameObject.GetComponent<SpriteRenderer>().sprite = possibleSprites[UnityEngine.Random.Range(0, possibleSprites.Length)];
    }

    void Update()
    {
        shootCooldown = Mathf.Max(shootCooldown-Time.deltaTime, 0f);
        
        Vector3 targetPos = player.transform.position;
        Vector3 thisPos = gameObject.transform.position;
        float r = Mathf.Rad2Deg*Mathf.Atan2(thisPos.y-targetPos.y, thisPos.x-targetPos.x);

        float s = Math.Abs(gameObject.transform.localScale.x);
        if (90f > r && r > -90f)
        {
            gameObject.transform.localScale = new Vector3(s, s, s);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(-s, s, s);
            r = Mathf.Rad2Deg*Mathf.Atan2(targetPos.y-thisPos.y, targetPos.x-thisPos.x);
        }


        bool withinRange = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y),
            new Vector2(transform.position.x, transform.position.y)) < 10;
        
        if (shootCooldown == 0 && withinRange)
        {
            gunHolder.GetComponent<AudioSource>().clip = enemysGun.shootingAudio;
            gunHolder.GetComponent<AudioSource>().Play();
                
            GameObject bulletSystem = Instantiate(enemysGun.bullet, gunHolder.transform);
            bulletSystem.transform.localPosition = new Vector3(enemysGun.bulletOfsetEnemyScale.x, enemysGun.bulletOfsetEnemyScale.y, 0);
            bulletSystem.transform.parent = null;
            bulletSystem.transform.localScale = new Vector3(1, 1, 1);
            
            bulletSystem.GetComponent<genericBulletDamageScript>().damage = enemysGun.damage;

            ParticleSystem ps = bulletSystem.GetComponent<ParticleSystem>();
            var collision = ps.collision;
            collision.collidesWith = thisCollisonBullet;

            ps.startSpeed *= difficulty;

            shootCooldown = enemysGun.fireRate*(1/Mathf.Min(0.9f, difficulty));
        }
        
        aim.transform.rotation = Quaternion.Euler(0, 0, r);
    }

    public void takeDam(int dam)
    {
        HP -= dam;

        if (HP <= 0)
        {
            player.GetComponent<tempPlrController>().killCount += 1;
            Destroy(gameObject);
        }
    }
}
