using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl_Archer : MonoBehaviour
{
    GameObject player;
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;
    Animator anim;
    public BoxCollider2D boxCollider;
    public Transform Pos;
    public Vector2 BoxSize; //공격 범위
    public bool EnemyMove = false;
    public bool TakeDamaged = false;
    public bool Right = true;
    public GameObject Arrow;
    public Transform ArrowPos;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        r2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetBool("Enemy_Move", false);
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        //StartCoroutine(AttackCool());
    }
    void Update()
    {
        if(TakeDamaged&&!player.GetComponent<Playercontrol>().isskill3doing)
        {
           boxCollider.enabled = false;
        }
    }
    void FixedUpdate()
    {
        if (EnemyMove)
        {
            anim.SetBool("Enemy_Move", true);
            EnemyMove = false;
        }
    }
    private void OnBecameVisible()
    {
        Invoke("SearchAI", 1f);
    }

    public void SearchAI()
    {
        anim.SetBool("IsSkill2Attack", false);

        //왼쪽 공격
        Collider2D[] collider2Ds = Physics2D.OverlapAreaAll(Pos.position, gameObject.transform.position);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {              
                anim.SetTrigger("Enemy_Attack");
                //anim.ResetTrigger("Enemy_Idle");
            }
        }
        StartCoroutine(AttackCoolTime());
    }
    public void OnWarp()
    {
        if (TakeDamaged &&!player.GetComponent<Playercontrol>().isskill3doing)
        {
            float currhp;
            currhp = gameObject.GetComponent<Enemy>().CurHp;
            anim.SetTrigger("Enemy_InWarp");
            StartCoroutine(Warping());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sim")
        {
            anim.SetTrigger("Enemy_Attacked");
            anim.SetBool("IsSkill2Attack", true);
            gameObject.GetComponent<Enemy>().TakeDamage(20);
            Invoke("SearchAI", 2f);
        }
        else if (collision.tag == "pen")
        {
            gameObject.GetComponent<Enemy>().TakeDamage(20);
        }

      
    }
    public void AttackFinish()
    {
        anim.SetTrigger("Enemy_Idle");
        anim.ResetTrigger("Enemy_Attack");
    }
    public void InstantiateArrow()
    {
        Instantiate(Arrow, ArrowPos);
    }

    IEnumerator AttackCoolTime()
    {
        yield return new WaitForSeconds(5f);
        SearchAI();
    }
    IEnumerator Warping()
    {
        yield return new WaitForSeconds(2.2f);

        if (Right)
        {
            TakeDamaged = false;
            Right = false;
            boxCollider.enabled = true;
            transform.localScale = new Vector3(-1, 1, 1);
            gameObject.transform.position = new Vector2(gameObject.transform.position.x - 20.0f, gameObject.transform.position.y);
            anim.ResetTrigger("Enemy_InWarp");
            anim.ResetTrigger("Enemy_Attack");
            anim.SetTrigger("Enemy_OutWarp");
            anim.SetTrigger("Enemy_Idle");
        }
        else
        {
            TakeDamaged = false;
            Right = true;
            boxCollider.enabled = true;
            transform.localScale = new Vector3(1, 1, 1);
            gameObject.transform.position = new Vector2(gameObject.transform.position.x + 20.0f, gameObject.transform.position.y);
            anim.ResetTrigger("Enemy_InWarp");
            anim.ResetTrigger("Enemy_Attack");
            anim.SetTrigger("Enemy_OutWarp");
            anim.SetTrigger("Enemy_Idle");
        }
    }
}
