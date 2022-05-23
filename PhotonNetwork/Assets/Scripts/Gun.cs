using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Gun : MonoBehaviour
{
    AudioSource fireSoundSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    Transform bulletParent;

    float fireDist = 50f;

    private PhotonView pv;

    public enum State
    {
        Ready, Empty, Reloading
    }
    public State state { get; private set; }
    public Transform fireTransform;

    void Awake()
    {
        // fireTransform = GameObject.Find("Gun").transform;s
        bulletParent = GameObject.Find("BulletParent").transform;
        fireSoundSource = GetComponent<AudioSource>();
        state = State.Ready;
    }

    private void Start()
    {
        pv = GetComponentInParent<PhotonView>();
        ObjectPool.instance.CreateInstance("Bullet", bulletParent, BulletManager.instance.BULLET);
    }

    public void Fire()
    {
        if (state == State.Ready)
        {
            Shot();
            pv.RPC("Shot", RpcTarget.All);
        }
    }

    bool IsShooting = false;

    [PunRPC]
    void Shot()
    {
        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        RaycastHit hitInfo;
        bool canShot = Physics.Raycast(ray, out hitInfo, fireDist);

        GameObject bullet = ObjectPool.instance.GetInactiveBulletNew("Bullet");
        bullet.transform.position = fireTransform.position;
        bullet.transform.forward = fireTransform.forward;

        if (canShot)
        {

        }
        else
        {

        }

        if (!IsShooting)
        {
            StartCoroutine(IEShotEffect(bullet));
        }
    }

    IEnumerator IEShotEffect(GameObject bullet)
    {
        BulletManager.instance.BULLET--;
        IsShooting = true;
        fireSoundSource.PlayOneShot(shootSound);
        yield return new WaitForSeconds(0.5f);
        IsShooting = false;
        yield return new WaitForSeconds(0.5f);
        ObjectPool.instance.AddInactiveList(bullet);
    }

    public void reload()
    {
        if (state == State.Reloading) // 리로드 중일때
        {// 리로드 상황일때 다양한 처리
            return;
        }// 그 외에는 다 리로딩이 실행
        // 키가 눌리면 다음 코루틴이 실행
        StartCoroutine(ReloadPrcess());
    }
    IEnumerator ReloadPrcess()
    {
        state = State.Reloading;
        this.fireSoundSource.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(0.05f); // 해당 초가 지나면
                                                // 탄창을 채운다.

        // public int total_remain = 100; // 총 탄환
        // public int mag_capacity = 25; // 탄창당 탄환
        // 15               //25        10
        int fill_armo = BulletManager.instance.mag_capacity - BulletManager.instance.magArmo; // 탄창개수 - 현재 총알갯수 : 보충할 개수
        if (BulletManager.instance.total_remain < fill_armo)
        {
            fill_armo = BulletManager.instance.total_remain;
        }

        BulletManager.instance.total_remain -= fill_armo; // 총갯수에서 뺀다
        BulletManager.instance.BULLET += fill_armo; // 갯수만큼 보충하고

        state = State.Ready;
        // 탄창을 채우는 부분
    }
}
