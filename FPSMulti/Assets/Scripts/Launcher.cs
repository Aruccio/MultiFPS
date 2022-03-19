using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace FPSMulti
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public void Awake() //wlacza siê ta funkcja i wywo³uje Connect()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }

        public override void OnConnectedToMaster() //wywo³uje join
        {
            Debug.Log("Connected.");
            Join();
            base.OnConnectedToMaster();
            
        }

        public override void OnJoinedRoom() //gdy juz istnieje room i gracz siê ³¹czy, wywo³uje siê StartGame()
        {
            StartGame();
            base.OnJoinedRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Create();
            base.OnJoinRandomFailed(returnCode, message);
        }
        public void Connect()
        {
            Debug.Log("Trying to connect...");
            PhotonNetwork.GameVersion = "0.0.0";
            PhotonNetwork.ConnectUsingSettings();
            //w³¹cza siê OnConnecterToMaster
        }
        
        public void Join()
        {
            PhotonNetwork.JoinRandomRoom(); //do³¹cza, a jesli nie, wywo³uje OnJoinRandomFailed
        }

        public void Create()
        {
            PhotonNetwork.CreateRoom(""); //tworzy room, bo go nie by³o
        }

        public void StartGame()
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount==1)
            {
                PhotonNetwork.LoadLevel(1); //to jest za³adowanie sceny o takim indeksie w build settings
            }
        }
    }
}