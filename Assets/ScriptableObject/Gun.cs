using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string name; // name
    public bool automatic; // hold or click?
    public float damage; // damage should be moved to the bullet
    public float fireRate; // delay between sots
    public GameObject bullet; // the bullet it spawns
    public Vector2 bulletOfsetPlrScale; // Offset of bullet for players
    public Vector2 bulletOfsetEnemyScale; // Offset of bullet for the enemys
    public Sprite theImageSprite; // as the name says
    public int startAmmo = 10; // Amoung of shots in this gun
    public AudioClip shootingAudio; // The shootin sound
}


