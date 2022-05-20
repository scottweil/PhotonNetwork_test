using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20.0f;
    public int damage = 2;

    void Update()
    {
        Vector3 dir = transform.forward;
        transform.position += dir * Time.deltaTime * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        ObjectPool.instance.AddInactiveList(gameObject);

        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            enemy.AttackedDisplay();
            enemy.ENEMY_HP -= damage;
        }
    }
}
