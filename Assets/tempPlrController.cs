using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class tempPlrController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform mcT;

    public float maxHP = 100;
    public float HP = 100;

    public float speed = 100f;

    public ParticleSystem ps;
    
    public Collider2D c2D;
    public LayerMask gridLayer; // grid and enemy, cuz then you can jump on them lol

    public float rocketBoostLeft = 100f;
    public float rocketBoostRegen = 50f;
    public float rocketBoostUse = 10f;
    public float rocketBoostPowerSides = 5f;
    public float rocketBoostPowerUp = 40f;

    public Material hp;
    public Material stam;

    public bool justJumped; // turn false when the player is at peak (aka begins falling from jump)

    [Space] public float scrollSpeedMult = 1f;
    
    public Animator ani;

    [Space] public int killCount = 0;
    public GameObject deathScreen;
    public bool stopScreenMove = false;
    public TextMeshPro tmpKK;
    public TextMeshPro tmpA;

    public gunContolScript gCS;
    
    public AudioSource AS;
    public Text scores;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        mcT = Camera.main.transform;
    } 

    // Update is called once per frame
    void Update()
    {
        tmpKK.text = "Kill count: " + killCount;
        tmpA.text = "Ammo: " + gCS.ammo;
        /*
        if ((!GetComponent<SpriteRenderer>().isVisible) && (transform.position.x < mcT.position.x || transform.position.y < mcT.position.y))
            die();
        */// fix this
        
        if (transform.position.y <= -7 || transform.position.x-mcT.position.x <= -7.5)
        {
            die();
        }

        float diff = transform.position.x - mcT.position.x;

        if (!stopScreenMove)
        {
            // move player with screen
            if ((!Input.GetKey("a")) && (!Input.GetKey("d")) && (!Input.GetKey("s")) && c2D.IsTouchingLayers())
                transform.position += new Vector3(Time.deltaTime * scrollSpeedMult, 0, 0);

            if (diff > 0) // if on right side of screen, add speed
                mcT.position += new Vector3(Time.deltaTime * scrollSpeedMult * Mathf.Pow(1.4f, diff), 0, 0);
            else
                mcT.position += new Vector3(Time.deltaTime * scrollSpeedMult, 0, 0);
        }

        // reset animation params
        ani.SetBool("IsSliding", false);
        ani.SetBool("IsAir", false);

        if (stopScreenMove)
            ani.SetBool("IsWalking", false);
        else
            ani.SetBool("IsWalking", true);

        if (c2D.IsTouchingLayers())
        {
            // regen rocket boost while standing
            rocketBoostLeft = Mathf.Min(100, rocketBoostLeft + Time.deltaTime * rocketBoostRegen);
            
            if (Input.GetKeyDown("s"))
            {
                rb.velocity = new Vector2(rb.velocity.x * 1.6f, rb.velocity.y);
            }
            else if (Input.GetKey("s"))
            {
                ani.SetBool("IsSliding", true);
                if (Input.GetKeyDown("w") || Input.GetKeyDown("space"))
                {
                    justJumped = true;
                    rb.velocity = new Vector2(rb.velocity.x * 1.3f, 4.5f);
                }
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                if (Input.GetKey("d"))
                    rb.velocity += new Vector2(speed, 0f);
                if (Input.GetKey("a"))
                    rb.velocity -= new Vector2(speed, 0f);

                // make it do walk animation if walking
                if (rb.velocity.x != 0)
                    ani.SetBool("IsWalking", true);

                // if you press bot a and d it will not go any way

                if (Input.GetKeyDown("w") || Input.GetKeyDown("space"))
                {
                    justJumped = true;
                    rb.velocity = new Vector2(rb.velocity.x, 6);
                }
            }
            // stop jetpack
            AS.Pause();
        }
        else
        {
            ani.SetBool("IsAir", true);
            if (justJumped && rb.velocity.y < 0)
                justJumped = false;

            if (Input.GetKey("w") || Input.GetKey("space"))
            {
                if (rocketBoostLeft != 0 && justJumped == false)
                {
                    //jetpack particles
                    ps.Emit(1);
                    //jetpack sounds
                    AS.UnPause();

                    rocketBoostLeft = Mathf.Max(0f, rocketBoostLeft - Time.deltaTime * rocketBoostUse);

                    // air control
                    if (Input.GetKey("d") && !Input.GetKey("a")) // if d is down and a is not, walk
                        rb.velocity += new Vector2(speed * 2 * Time.deltaTime, 0f);
                    if (Input.GetKey("a") && !Input.GetKey("d")) // if a is down and d is not, walk
                        rb.velocity -= new Vector2(speed * 2 * Time.deltaTime, 0f);

                    rb.velocity += new Vector2(0, Time.deltaTime * rocketBoostPowerUp);
                }
                else
                {
                    // stop jetpack
                    AS.Pause();
                }
            }
            else
            {
                // stop jetpack
                AS.Pause();
                
                // minor air control
                if (Input.GetKey("d") && !Input.GetKey("a")) // if d is down and a is not, walk
                    rb.velocity += new Vector2(speed * Time.deltaTime, 0f);
                if (Input.GetKey("a") && !Input.GetKey("d")) // if a is down and d is not, walk
                    rb.velocity -= new Vector2(speed * Time.deltaTime, 0f);
            }
        }

        stam.SetFloat("_p", Mathf.Abs(1 - rocketBoostLeft / 100));
        hp.SetFloat("_p", Mathf.Abs(1 - HP / maxHP));
    }
    
    private void OnParticleCollision(GameObject other)
    {
        HP = Mathf.Max(0f, HP - other.GetComponent<genericBulletDamageScript>().damage);

        if (HP == 0f)
        {
            die();
        }
    }

    private void die()
    {
        if (speed != 0)
        {
            stopScreenMove = true;
            speed = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;

            int HighScore = 0;
            if (PlayerPrefs.HasKey("HS"))
                HighScore = PlayerPrefs.GetInt("HS");
            if (killCount > HighScore)
                PlayerPrefs.SetInt("HS", killCount);
        
            scores.text = "Kills: "+ killCount +"\n Highscore: " + (killCount > HighScore ? (killCount + " (new)") : HighScore.ToString());
            deathScreen.SetActive(true);
        }
    }
}