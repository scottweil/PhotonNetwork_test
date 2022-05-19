using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerShoot : MonoBehaviourPun
{
    PlayerInput playerInput;
    Animator anim;

    bool firePress, reloadPress;

    void Start()
    {
        firePress = reloadPress = false;
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.fire == 1)
        {
            if (firePress == false)
            {
                firePress = true;

                anim.SetTrigger("Shoot");
            }
        }
        else if (playerInput.reload == 1)
        {
            if (reloadPress == false)
            {
                reloadPress = true;
                anim.SetTrigger("Reload");
            }
        }
        else
        {
            firePress = false;
            reloadPress = false;
        }
    }
}
