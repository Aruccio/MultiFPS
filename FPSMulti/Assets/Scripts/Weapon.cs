using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace FPSMulti
{
    public class Weapon : MonoBehaviourPunCallbacks
    {
        #region Variables

        public Gun[] loadout;
        public Transform weaponParent;
        public GameObject bulletholePrefab;
        public LayerMask canBeShot;

        private GameObject currentWeapon;
        private int currentIndex;
        private float bloomF;
        private float currentCooldown;

        #endregion Variables

        #region Monobehaviour Callbacks

        // Start is called before the first frame update
        private void Start()
        {
            foreach (Gun g in loadout)
            {
                g.Initialize();
            }
            //    Equip(0);
        }

        // Update is called once per frame
        private void Update()
        {
            if (photonView.IsMine && Input.GetKeyDown(KeyCode.Alpha1))
                photonView.RPC("Equip", RpcTarget.All, 0); //nazwa funkcji z string, do kogo, argument funkcji

            if (currentWeapon != null)
            {
                if (photonView.IsMine)
                {
                    Aim((Input.GetKey(KeyCode.Z) || Input.GetMouseButton(1)));
                    if (Input.GetMouseButtonDown(0))//LPM
                    {
                        if (loadout[currentIndex].CanFire())
                        {
                            photonView.RPC("Shoot", RpcTarget.All, Input.GetKey(KeyCode.Z));
                        }
                        else loadout[currentIndex].Reload();
                    }

                    //                    if (currentCooldown > 0) currentCooldown = -Time.deltaTime;
                }

                //weapon position elasticity
                if (currentWeapon != null)
                    currentWeapon.transform.localPosition = Vector3.Lerp(currentWeapon.transform.localPosition, Vector3.zero, Time.deltaTime * 4f);
            }
        }

        #endregion Monobehaviour Callbacks

        #region Private Methods

        [PunRPC]
        private void Equip(int pInd)
        {
            if (currentWeapon != null) Destroy(currentWeapon);

            GameObject newEquipment = Instantiate(loadout[pInd].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localEulerAngles = Vector3.zero;
            newEquipment.GetComponent<Sway>().isMine = photonView.IsMine;
            currentWeapon = newEquipment;
            currentIndex = pInd;
        }

        [PunRPC]
        public void Aim(bool isAiming)
        {
            Transform anchor = currentWeapon.transform.Find("Anchor");
            Transform ads = currentWeapon.transform.Find("States/ADS");
            Transform hip = currentWeapon.transform.Find("States/Hip");

            if (isAiming)
            {
                //aim
                anchor.position = Vector3.Lerp(anchor.position, ads.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            }
            else
            {
                //hip
                anchor.position = Vector3.Lerp(anchor.position, hip.position, Time.deltaTime * loadout[currentIndex].aimSpeed);
            }
        }

        [PunRPC]
        private void Shoot(bool aiming)
        {
            Transform spawn = transform.Find("Cameras/PlayerCamera");
            //setup bloom
            Vector3 bloom = spawn.position + spawn.forward * 1000f;
            if (aiming)
            {
                bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * spawn.up;
                bloom += Random.Range(-loadout[currentIndex].bloom, loadout[currentIndex].bloom) * spawn.right;
            }
            else
            {
                bloom += Random.Range(-loadout[currentIndex].aimbloom, loadout[currentIndex].aimbloom) * spawn.up;
                bloom += Random.Range(-loadout[currentIndex].aimbloom, loadout[currentIndex].aimbloom) * spawn.right;
            }

            bloom -= spawn.position;
            bloom.Normalize();

            //cooldown
            currentCooldown = loadout[currentIndex].firerate;

            //raycast
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(spawn.position, bloom, out hit, loadout[currentIndex].distance, canBeShot))
            {
                GameObject newBulletHole = Instantiate(bulletholePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                newBulletHole.transform.LookAt(hit.point + hit.normal);

                //jesli trafiamy w gracza
                if (hit.collider.gameObject.layer == 9)
                {
                    Destroy(newBulletHole, 1f);

                    if (photonView.IsMine)
                    {
                        //RPC Call zadaj¹cy dmg graczowi
                        hit.collider.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, loadout[currentIndex].damage);
                    }
                }
                else
                {
                    Destroy(newBulletHole, 5f);
                }
            }

            //gun fx (recoil)
            currentWeapon.transform.Rotate(-loadout[currentIndex].recoil, 0, 0);
            currentWeapon.transform.position -= currentWeapon.transform.forward * loadout[currentIndex].kicback;
        }

        [PunRPC]
        private void TakeDamage(int dmg)
        {
            GetComponent<Player>().TakeDamage(dmg);
        }

        #endregion Private Methods
    }
}