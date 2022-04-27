using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class areaManagerScript : MonoBehaviour
{
    public Area[] areas;
    public float curDifficulty = 0.2f;
    public int curDist = 18;
    public int maxDist = 50;
    private Area lastArea;

    public GameObject plr;

    private void Start()
    {
        lastArea = areas[0];
    }

    void Update()
    {
        if (plr.GetComponent<tempPlrController>().stopScreenMove)
            return;
            
        curDifficulty = 0.2f + Mathf.Floor(curDist / 100f)/10f;
        
        if (plr.transform.position.x * 4f + maxDist > curDist)
        {
            Area area = null;

            while (area is null)
            {
                int random = UnityEngine.Random.Range(0, areas.Length);
                if (areas[random].minDifficulty <= curDifficulty && areas[random] != lastArea)
                {
                    area = areas[random];
                    lastArea = area;
                }
            }
            
            GameObject instantiatedObj = Instantiate(area.gridToPaste, new Vector3(curDist/2f, 0f, 0f), quaternion.identity, transform);

            enemyAllInOneScript[] allToChange = instantiatedObj.GetComponentsInChildren<enemyAllInOneScript>();
            foreach (enemyAllInOneScript enemy in allToChange)
            {
                enemy.player = plr;
                enemy.difficulty = curDifficulty;
            }
            
            
            curDist += area.cellWidth;
        }
    }
}
