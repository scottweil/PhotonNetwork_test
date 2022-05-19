using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    AudioSource fireSoundSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    LineRenderer bulletLineRenderer;
    float fireDist = 50f;

    public float damage = 25;
    public int armor_remain = 100;
    public int mag_capacity = 25;
    public int magArmo; // 현재 탄창에 있는 총알 갯수

    public enum State
    {
        Ready, Empty, Reloading
    }
    public State state { get; private set; }
    public Transform fireTransform;

    // Start is called before the first frame update
    void Awake()
    {
        magArmo = mag_capacity;
        fireSoundSource = GetComponent<AudioSource>();
        bulletLineRenderer = fireTransform.GetComponent<LineRenderer>();
        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
        state = State.Ready;


    }

    public void Fire()
    {
        if (state == State.Ready)
        {
            Shot();
        }
    }

    bool IsShooting = false;
    void Shot()
    {
        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        RaycastHit hitInfo;
        bool canShot = Physics.Raycast(ray, out hitInfo, fireDist);

        bulletLineRenderer.SetPosition(0, ray.origin);

        if (canShot)
        {
            bulletLineRenderer.SetPosition(1, hitInfo.point);
            if (!IsShooting)
            {
                StartCoroutine(IEShotEffect());
            }
        }
        else
        {
            bulletLineRenderer.SetPosition(1, ray.origin + ray.direction * 50);
            if (!IsShooting)
            {
                StartCoroutine(IEShotEffect());
            }
        }
    }

    IEnumerator IEShotEffect()
    {
        magArmo--;
        IsShooting = true;
        fireSoundSource.PlayOneShot(shootSound);
        bulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(0.5f);
        bulletLineRenderer.enabled = false;
        IsShooting = false;
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

        // public int armor_remain = 100; // 총 탄환
        // public int mag_capacity = 25; // 탄창당 탄환
        // 15               //25        10
        int fill_armo = mag_capacity - magArmo; // 탄창개수 - 현재 총알갯수 : 보충할 개수
        if (armor_remain < fill_armo)
        {
            fill_armo = armor_remain;
        }
        magArmo += fill_armo; // 갯수만큼 보충하고
        armor_remain -= fill_armo; // 총갯수에서 뺀다

        state = State.Ready;
        // 탄창을 채우는 부분
    }
}
