using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityStandardAssets.Utility;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable
{
    public float speed = 5f;
    CharacterController cc;
    float ry;
    public float rotSpeed = 10f;

    PlayerInput playerInput; //내 이동관련 입력객체

    Animator anim;
    int speedHash;

    Vector3 velocity;
    CinemachineFreeLook CM_freeLook;

    public Transform CM_LookAt;

    //수신된 위치와 회전값을 저장할 변수
    private Vector3 receivePos;
    private Quaternion receiveRot;
    public float damping = 10.0f;

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

        CM_freeLook = GameObject.FindObjectOfType<CinemachineFreeLook>();

        if (photonView.IsMine)
        {
            CM_freeLook.Follow = this.transform;
            CM_freeLook.LookAt = CM_LookAt;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            MoveChar();

        }
        else
        {
            //수신된 좌표로 보간한 이동 처리
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping);

            //수신된 회전값으로 보간한 회전 처리
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping);
        }


        // RotChar();
    }

    Vector3 dir;
    void MoveChar()
    {
        dir = new Vector3(playerInput.moveH, 0, playerInput.moveV);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        anim.SetFloat(speedHash, Vector3.ClampMagnitude(dir, 1).magnitude);
        velocity = dir * speed;

        transform.forward += Vector3.Lerp(transform.forward, dir, rotSpeed);
        cc.SimpleMove(velocity);
    }
    // [PunRPC]
    // void RotChar()
    // {
    //     dir = Camera.main.transform.forward;
    //     dir.y = 0;
    //     transform.forward += Vector3.Lerp(transform.forward, dir, rotSpeed);
    // }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //IsWriting이 true이면 데이터를 전송
        //false면 수신
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
