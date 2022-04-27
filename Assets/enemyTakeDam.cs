using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTakeDam : MonoBehaviour
{
    public float multiplier = 1f; // for headshot or the like.
    private enemyAllInOneScript eAIOS;

    private void Start()
    {
        Debug.Log("startUp");
        eAIOS = transform.parent.GetComponent<enemyAllInOneScript>();
    }

    private void OnParticleCollision(GameObject other)
    {
        eAIOS.takeDam((int)(other.GetComponent<genericBulletDamageScript>().damage * multiplier));
    }
}
