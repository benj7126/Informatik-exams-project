using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Area")]
public class Area : ScriptableObject
{
    public int cellWidth = 0;
    public GameObject gridToPaste;
    public float minDifficulty = 0.2f; // if higher than or equals, can spawn
}