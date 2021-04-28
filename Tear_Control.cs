using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tear_Control : MonoBehaviour
{
    public Queue<GameObject>Tearqueue;
    Animator ani;
    Rigidbody2D ri2d;
    void Start()
    {
        //ani = GetComponent<Animator>();
        //  AttackBox = GetComponent<BoxCollider2D>();
        Tearqueue = GameObject.FindWithTag("Enemy_Boss").GetComponent<Enemy_Boss_Tear>().Tear;
        ani = GetComponent<Animator>();
        ri2d = GetComponent<Rigidbody2D>();
    }

    public void EnqueueTear()
    {
        Tearqueue.Enqueue(gameObject);
        //Tearqueue.Tear.Add(gameObject);
        ri2d.gravityScale = 0.3f;
        gameObject.SetActive(false);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Floor")
        {
            ani.SetTrigger("TearA");
            ri2d.gravityScale = 0;
            ri2d.velocity=new Vector2(0,0);
        }

    }
}
