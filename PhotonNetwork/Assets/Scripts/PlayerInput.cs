#define PC_Platform
#define VR_Platform

using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInput : MonoBehaviourPunCallbacks
{
    public float moveV { get; private set; }
    public float moveH { get; private set; }
    public float rotate { get; private set; }
    public float fire { get; private set; }
    public float reload { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) { return; }
#if PC_Platform

        moveV = Input.GetAxis("Vertical");
        moveH = Input.GetAxis("Horizontal");
        rotate = Input.GetAxis("Mouse X");
        fire = Input.GetAxisRaw("Fire1");
        reload = Input.GetAxisRaw("Fire2");

#endif
    }
}
