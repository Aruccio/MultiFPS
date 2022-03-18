using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Blade", menuName = "Blade")]
public class Blade : ScriptableObject
{
    public string name;
    public float firerate;
    public float distance;
    public GameObject prefab;
}
