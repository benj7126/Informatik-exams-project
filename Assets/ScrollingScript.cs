using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingScript : MonoBehaviour
{
    public tempPlrController controller;
    public float moveSpeed;
    void FixedUpdate()
    {
        if (controller.stopScreenMove)
            return;
        
        gameObject.transform.position += new Vector3(moveSpeed, 0, 0);
    }
}
