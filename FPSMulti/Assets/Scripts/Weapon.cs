using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    #region Variables
    public Gun[] loadout;
    public Transform weaponParent;
    public GameObject bulletholePrefab;
    public LayerMask canBeShot;

    private GameObject currentWeapon;
    private int currIndex;
    private float bloomF;
    #endregion

    #region Monobehaviour Callbacks
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);
        if (currentWeapon != null)
        {
            Aim(Input.GetKey(KeyCode.Z) || Input.GetMouseButton(1));

            if (Input.GetMouseButtonDown(0))//LPM
            {
                Shoot(Input.GetKey(KeyCode.Z));
            }
            //weapon position elasticity
            currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
        }
    }

    #endregion

    #region Private Methods
    void Equip(int pInd)
    {
        if (currentWeapon != null) Destroy(currentWeapon);
        
        GameObject newEquipment = Instantiate(loadout[pInd].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        newEquipment.transform.localPosition = Vector3.zero;
        newEquipment.transform.localEulerAngles = Vector3.zero;
        currentWeapon = newEquipment;
        currIndex = pInd;

    }

    public void Aim(bool isAiming)
    {

        Transform anchor = currentWeapon.transform.Find("Anchor");
        Transform ads = currentWeapon.transform.Find("States/ADS");
        Transform hip = currentWeapon.transform.Find("States/Hip");

        if (isAiming)
        {
            //aim
            anchor.position = Vector3.Lerp(anchor.position, ads.position, Time.deltaTime * loadout[currIndex].aimSpeed);
        }
        else
        {
            //hip
            anchor.position = Vector3.Lerp(anchor.position, hip.position, Time.deltaTime * loadout[currIndex].aimSpeed);

        }
    }

    void Shoot(bool aiming)
    {
        Transform spawn = transform.Find("Cameras/PlayerCamera");

        //bloom

        Vector3 bloom = spawn.position + spawn.forward * 1000f;
        if (aiming)
        {
            bloom += Random.Range(-loadout[currIndex].bloom, loadout[currIndex].bloom) * spawn.up;
            bloom += Random.Range(-loadout[currIndex].bloom, loadout[currIndex].bloom) * spawn.right;
        }
        else
        {
            bloom += Random.Range(-loadout[currIndex].aimbloom, loadout[currIndex].aimbloom) * spawn.up;
            bloom += Random.Range(-loadout[currIndex].aimbloom, loadout[currIndex].aimbloom) * spawn.right;
        }
       
        bloom -= spawn.position;
        bloom.Normalize();

        //raycast
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(spawn.position, bloom, out hit, loadout[currIndex].distance, canBeShot))
        {
            GameObject newBulletHole = Instantiate(bulletholePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
            newBulletHole.transform.LookAt(hit.point + hit.normal);
            Destroy(newBulletHole, 5f);
        }

        //gun fx (recoil)

        currentWeapon.transform.Rotate(-loadout[currIndex].recoil, 0, 0);
        currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currIndex].kicback;
    }


    #endregion
}
