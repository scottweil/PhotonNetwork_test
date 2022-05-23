using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHP : MonoBehaviourPunCallbacks
{
    private Renderer[] renderers;

    private int initHp = 100;
    public int currHp = 100;

    private Animator anim;
    private CharacterController cc;

    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashrespawn = Animator.StringToHash("Respawn");

    private GameObject PlayerHP_GameScene;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        currHp = initHp;
        if (photonView.IsMine)
        {

            PlayerHP_GameScene = transform.Find("PlayerHP_GameScene").gameObject;
            PlayerHP_GameScene.SetActive(false);
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (currHp > 0 && other.collider.CompareTag("BULLET"))
        {
            Bullet bullet = other.collider.GetComponent<Bullet>();

            currHp -= bullet.damage;
            if (currHp <= 0)
            {
                StartCoroutine("IEPlayerDie");
            }

        }
    }
    IEnumerator IEPlayerDie()
    {
        cc.enabled = false;
        anim.SetBool(hashrespawn, false);
        anim.SetTrigger(hashDie);

        SetPlayerVisible(false);

        yield return new WaitForSeconds(1.5f);

        Vector3 randomPos = UnityEngine.Random.insideUnitCircle;
        transform.position = new Vector3(randomPos.x, 0, randomPos.z);

        currHp = 100;
        SetPlayerVisible(true);

        cc.enabled = true;
    }

    private void SetPlayerVisible(bool isVisible)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = isVisible;
        }
    }
}

