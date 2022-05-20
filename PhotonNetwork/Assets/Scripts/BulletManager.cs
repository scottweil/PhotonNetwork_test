using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;
    private void Awake()
    {
        BulletManager.instance = this;
    }

    
    public int total_remain = 100;
    public int mag_capacity = 25;
    public int magArmo; // 현재 탄창에 있는 총알 갯수

    public int BULLET
    {
        get { return magArmo; }
        set
        {
            magArmo = value;
            bulletText.text = magArmo + " / " + total_remain;
        }
    }

    public TMP_Text bulletText;

    void Start()
    {
        BULLET = mag_capacity;
    }
}
