using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityStandardAssets.Utility;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerMove : MonoBehaviourPunCallbacks
{
    public float speed = 5f;
    CharacterController cc;
    float ry;
    public float rotSpeed = 10f;

    PlayerInput playerInput; //내 이동관련 입력객체

    Animator anim;
    int speedHash;

    Vector3 velocity;

    Cinemachine.CinemachineFreeLook CM_FreeLook;
    CinemachineFreeLook CM_freeLook;

    public Transform CM_LookAt;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        speedHash = Animator.StringToHash("Speed");

        CM_freeLook = Camera.main.transform.GetComponentInChildren<CinemachineFreeLook>();

        if (photonView.IsMine)
        {
            CM_freeLook.Follow = this.transform;
            CM_freeLook.LookAt = CM_LookAt;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) { return; }

        MoveChar();
        RotChar();
    }

    Vector3 dir;
    void MoveChar()
    {
        dir = new Vector3(0, 0, playerInput.move);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.Normalize();

        anim.SetFloat(speedHash, playerInput.move);
        velocity = dir * speed;

        cc.SimpleMove(velocity);
    }
    void RotChar()
    {
        dir = Camera.main.transform.forward;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed);
    }
}
