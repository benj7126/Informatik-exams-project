using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startOnShot : MonoBehaviour
{
    public tempPlrController controlScript;
    
    private void OnParticleCollision(GameObject other)
    {
        controlScript.stopScreenMove = false;
        Destroy(gameObject);
    }
}
