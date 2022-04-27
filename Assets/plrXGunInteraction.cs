using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plrXGunInteraction : MonoBehaviour
{
    public Gun startGun;
    
    public GameObject gunHolder;
    public Gun curGun;

    public gunContolScript contolScript;

    // Start is called before the first frame update
    void Start()
    {
        setGun(startGun);
    }

    public void playPickup()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void setGun(Gun g)
    {
        curGun = g;
        contolScript.newGun(curGun);

        gunHolder.GetComponent<SpriteRenderer>().sprite = curGun.theImageSprite;
    }
}
