using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//네트워크 기능을 사용하겠음
using Photon.Pun; //포톤 펀 영역에 있는 클래스, 메서드, 자료형을 사용하겠음.
using Photon.Realtime; //포톤의 코어기능을 기반으로 만들겠음.
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks //MonoBehaviour + photonnetwork를 다 쓰고 싶다. 다양한 함수, 이벤트 발생 시 자동으로 함수가 호출됨.
{
    //접속 상태를 체크할 수 있는 상태변수를 준비하자 - 열거타입으로 상태를 정의함.
    enum NetworkState { none, Connect, Disconnect, MakeRoom, InRoom }

    //UI 객체들 : 버튼 3개, text 1개
    public Button Login; //접속
    public Button Logout; //접속해제
    public Button Join; //방 생성 혹은 참여(방이 있을 때)
    public Text info_txt;

    //PhotonNetwork

    string gameVer = "0.1";

    //일반 변수들
    NetworkState NetState = NetworkState.none;

    void Start()
    {
        //버튼이 처음부터 다 활성화 되는 것이 아니라 접속만 활성화시킨다.
        Login.interactable = true;
        Logout.interactable = false;
        Join.interactable = false;

        PhotonNetwork.GameVersion = gameVer;
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    //버튼 이벤트별 함수------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Connect_Server()
    {
        info_txt.text = "마스터 서버에 접속 중";
        PhotonNetwork.ConnectUsingSettings(); //포톤서버(마스터) 접속시도 -> 세팅한 정보 기반, 접속여부에 따라 다양한 함수가 호출된다(콜백함수)
    }

    public void DisConnect_Server()
    {
        PhotonNetwork.Disconnect();

        //버튼이 초기화가 이루어져야 함 -> log 활성 / logout, join 비활성
        Login.interactable = true;
        Logout.interactable = false;
        Join.interactable = false;

        NetState = NetworkState.Disconnect;
    }

    public void Connect_Room()
    {
        if (NetState == NetworkState.Connect)
        {
            info_txt.text = "방 생성 혹은 참여 중";
            PhotonNetwork.JoinRandomRoom();
        }
    }
    //------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public override void OnConnectedToMaster() //서버 접속시 호출되는 함수의 재정의
    {
        NetState = NetworkState.Connect;

        info_txt.text = "접속성공";
        Login.interactable = false;
        Logout.interactable = true;
        Join.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause) //서버가 접속 실패시 호출되는 함수의 재정의
    {
        if (NetState == NetworkState.none)
        {
            info_txt.text = "접속실패";
            PhotonNetwork.ConnectUsingSettings(); //포톤서버(마스터) 접속시도 -> 세팅한 정보 기반, 접속여부에 따라 다양한 함수가 호출된다(콜백함수)
        }
        else if (NetState == NetworkState.Disconnect)
        {
            info_txt.text = "접속해지 상황입니다.";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        info_txt.text = "방이 없음 새로 방을 만들겠음.";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 8 });
    }

    public override void OnJoinedRoom()
    {
        info_txt.text = "방이 만들어졌음. \n" + PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.LoadLevel("2.Game");
    }

    void Update()
    {

    }
}
