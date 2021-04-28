using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl_Distant : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;



    public GameObject Bullet;
    public GameObject BulletPosiiton;

    public Transform Pos;
    public Vector2 BoxSize;
    private float curTime;
    Vector2 startPosition;
    GameObject EnemyHpBar;


    private float initHp = 100.0f;
    [Header("Status")]
    public float currHp;
    public float attkDistance; //적 공격범위
    public float findRange; //플레이어 찾는 거리
    public int EnemySpeed; //적 속도
    public float CoolTime = 0.5f; //공격 속도

    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        currHp = initHp;
        EnemyHpBar = transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject;
    }


    void FixedUpdate()
    {
        EnemyAi();
    }

    public void TakeDamage(int hit)
    {
        currHp -= hit;
        if (currHp <= 0)
        {
            //사망
        }
    }

    void EnemyAi()
    {
        /*
        RaycastHit2D rayRange = Physics2D.Raycast(transform.position, Vector3.right, findRange, LayerMask.GetMask("Player"));

        Debug.DrawRay(transform.position, Vector3.right * findRange, Color.yellow); //플레이어 발견 범위

        if (rayRange.collider != null)
        {
          */
        if (curTime <= 0)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
            if (collider2Ds != null)
            {
                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.tag == "Player")
                    {
                        Debug.Log("표창 공격");
                        Instantiate(Bullet, BulletPosiiton.transform.position, Quaternion.identity);
                    }
                }
            }
            curTime = CoolTime;


        }
        curTime -= Time.deltaTime;
    }
    /*
        else //플레이어 찾는 범위 밖이면 처음 위치로 이동
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(startPosition.x, transform.position.y), Time.deltaTime * EnemySpeed);

        }
        */


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(Pos.position, BoxSize);
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    void Update()
    {
        EnemyHpBar.GetComponent<Image>().fillAmount = (currHp / initHp);

        if (currHp <= 0)
        {
            Die();
        }
    }

}

