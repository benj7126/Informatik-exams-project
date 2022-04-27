using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GunSpawner : MonoBehaviour
{
    public Gun[] possibleGuns;
    private Gun chosenGun;

    private float swayTimer = 0f;
    private Vector3 startPos;
    public GameObject textPrefab;
    
    void Start()
    {
        chosenGun = possibleGuns[Random.Range(0, possibleGuns.Length)];

        SpriteRenderer sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = chosenGun.theImageSprite;
        
        BoxCollider2D C = gameObject.AddComponent<BoxCollider2D>();
        C.isTrigger = true;

        startPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        swayTimer += Time.deltaTime;
        if (swayTimer > 2)
            swayTimer -= 2;
        
        float toGive = swayTimer;
        if (toGive > 1)
            toGive = 2 - toGive;

        gameObject.transform.position = new Vector3(startPos.x, startPos.y+easeInOutCubic(toGive)/4f, startPos.z);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey("e"))
            equip(other);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        textPrefab.GetComponent<TextMeshPro>().text = "(E) Pick Up "+chosenGun.name;
        textPrefab.SetActive(true);
        if (Input.GetKey("e"))
            equip(col);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        textPrefab.SetActive(false);
    }

    private void equip(Collider2D col)
    {   
        plrXGunInteraction outVar;
        if (col.gameObject.TryGetComponent<plrXGunInteraction>(out outVar))
        {
            outVar.playPickup();
            outVar.setGun(chosenGun);
            Destroy(gameObject);
        }
    }

    float easeInOutCubic(float x){
        return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;;
    }

}
