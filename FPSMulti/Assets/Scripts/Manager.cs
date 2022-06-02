using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace FPSMulti
{
    public class Manager : MonoBehaviour
    {
        public string playerPrefab;
        public Transform[] spawnPoints;

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            PhotonNetwork.Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}