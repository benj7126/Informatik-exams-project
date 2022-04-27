using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class aimScript : MonoBehaviour
{
    public GameObject parent;
    void Update()
    {
        Vector3 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 thisPos = gameObject.transform.position;
        float r = Mathf.Rad2Deg*Mathf.Atan2(mp.y-thisPos.y, mp.x-thisPos.x);

        if (90f > r && r > -90f)
        {
            parent.transform.localScale = new Vector3(3, 3, 3);
        }
        else
        {
            parent.transform.localScale = new Vector3(-3, 3, 3);
            r = Mathf.Rad2Deg*Mathf.Atan2(thisPos.y-mp.y, thisPos.x-mp.x);
        }

        gameObject.transform.rotation = Quaternion.Euler(0, 0, r);
    }
}
