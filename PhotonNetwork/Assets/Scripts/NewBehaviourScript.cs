using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NewBehaviourScript : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        string gamever = "0.1";
        PhotonNetwork.GameVersion = gamever;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }
    public override void OnJoinedRoom()
    {
        Vector2 pos = Random.insideUnitCircle * 3f;

        // PhotonNetwork.Instantiate("Player", new Vector3.back(pos.x.5.pos.y).quaternion.identity)
    }
}
