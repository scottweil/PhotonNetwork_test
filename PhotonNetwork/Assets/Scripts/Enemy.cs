using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    bool isAttacked;

    Material mt;
    public Material attacked_mt;
    public Slider enemyHP_bar_front;
    public Slider enemyHP_bar_back;

    public int enemyHP = 5;

    public int ENEMY_HP
    {
        get;
        set;
    }

    void Start()
    {
        mt = GetComponent<MeshRenderer>().material;
        ENEMY_HP = enemyHP;
        enemyHP_bar_front.maxValue = enemyHP_bar_back.maxValue = ENEMY_HP;
        enemyHP_bar_back.value = ENEMY_HP;
    }

    public void AttackedDisplay()
    {
        StartCoroutine(ChangeColor());
        Invoke("IsAttakced", 0.5f);
    }

    IEnumerator ChangeColor()
    {
        Color color = mt.color;
        mt.color = attacked_mt.color;
        yield return new WaitForSeconds(0.2f);
        mt.color = color;
    }

    private void Update()
    {
        enemyHP_bar_front.value = Mathf.Lerp(enemyHP_bar_front.value, ENEMY_HP, Time.deltaTime * 5f);
        EnemyHP_Back_Slider();

        if (ENEMY_HP < 0)
        {
            Destroy(gameObject, 0.5f);
        }
    }

    void IsAttakced()
    {
        isAttacked = true;
    }

    void EnemyHP_Back_Slider()
    {
        if (isAttacked)
        {
            enemyHP_bar_back.value = Mathf.Lerp(enemyHP_bar_back.value, enemyHP_bar_front.value, Time.deltaTime * 10f);
            if (enemyHP_bar_back.value < enemyHP_bar_front.value * 1.01f)
            {
                isAttacked = false;
                enemyHP_bar_back.value = ENEMY_HP;
            }
        }
    }
}
