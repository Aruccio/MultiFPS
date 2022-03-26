using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Gun", menuName ="Gun")]
public class Gun : ScriptableObject
{
    public string name;
    public int damage;
    
    public float firerate;
    public float distance;
    public float aimSpeed;
    public float bloom;
    public float aimbloom;
    public float recoil;
    public float kicback;
    
    public GameObject prefab;
}
