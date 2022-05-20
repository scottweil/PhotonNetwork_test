using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    void Start()
    {
        //좌표값
        Vector3 randomPos = Random.insideUnitCircle * 3f;
        PhotonNetwork.Instantiate("Player", new Vector3(randomPos.x, 0, randomPos.z), Quaternion.identity);
    }
}
