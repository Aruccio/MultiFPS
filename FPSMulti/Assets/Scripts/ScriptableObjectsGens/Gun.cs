using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string name;
    public int damage;
    public int ammo;
    public int clipsize;
    public float firerate;
    public float distance;
    public float aimSpeed;
    public float bloom;
    public float aimbloom;
    public float recoil;
    public float kicback;

    public GameObject prefab;

    private int stash; //current ammo
    private int clip; //now in clip

    public void Initialize()
    {
        stash = ammo;
        clip = clipsize;
    }

    public bool CanFire()
    {
        Debug.Log(clip);
        if (clip > 0)
        {
            clip -= 1;
            return true;
        }
        else return false;
    }

    public bool Reload()
    {
        if (stash > 0)
        {
            Debug.Log("Reload");
            //throws away remaining bullets
            stash -= clipsize;
            clip = clipsize;
            return true;

            //adds bullet to existing ones
            /*
             * stash += clip;
             * clip = Matchf.Min(clipsize,stash);
             * stash -= clip;
             */
        }
        else return false;
    }

    public int GetStash()
    { return stash; }

    public int GetClip()
    { return clip; }
}