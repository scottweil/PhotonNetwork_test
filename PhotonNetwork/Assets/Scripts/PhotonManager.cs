using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string version = "1.0";
    private string userId = "DoYoung";

    private void Awake()
    {
        //마스터 클라이언트의 씬 자동 동기화 옵션
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = version;
        PhotonNetwork.NickName = userId;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 2; //룸에 입장할 수 있는 최대 접속자 수
        ro.IsOpen = true; //룸의 오픈 여부
        ro.IsVisible = true; //로비에서 룸 목록에 노출시킬지 여부

        PhotonNetwork.CreateRoom("My Room", ro);
    }

    public override void OnCreatedRoom()
    {

    }

    public override void OnJoinedRoom()
    {
        Vector3 randomPos = Random.insideUnitCircle * 3f;

        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            print($"{player.Value.NickName}, {player.Value.ActorNumber}");
        }

        PhotonNetwork.Instantiate("Player", new Vector3(randomPos.x, 0, randomPos.z), Quaternion.identity);
    }
}
