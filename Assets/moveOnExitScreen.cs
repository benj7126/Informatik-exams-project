using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveOnExitScreen : MonoBehaviour
{
    public float offsetAmount = 0;

    private Transform mainCamTransform;

    private void Start()
    {
        mainCamTransform = Camera.main.transform;
    }

    void Update()
    {
        // if game object is not rendered and is behind screen, move it.
        if (!gameObject.GetComponent<SpriteRenderer>().isVisible && mainCamTransform.position.x > gameObject.transform.position.x)
            gameObject.transform.position += new Vector3(offsetAmount*2f, 0, 0);
    }
}
