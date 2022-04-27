using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class destoyOnNoBullets : MonoBehaviour
{
    void Update()
    {
        if (!gameObject.GetComponent<ParticleSystem>().IsAlive(true))
            Destroy(gameObject);
    }
}
