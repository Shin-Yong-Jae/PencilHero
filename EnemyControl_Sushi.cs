using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public enum SushiType
{
    NormalType,
    LongType
};
public class EnemyControl_Sushi : Enemy
{
    SpriteRenderer spriteRenderer;

    bool EnemyAttack_First = true;
    bool EnemyMove = false;
    public bool OnScreen = false;

    public Collider2D AttackRange;
    public GameObject RiceBall;
    bool Birthinvin;
    [Header("Status")]
    public float hp;
    public float EnemySpeed; //적 속도

    bool IsSalmonEggAttack = true;
    public GameObject SalmonEggBall;

    private GameObject playerPosY;
    public SushiType sushitype;
    public int damage;
    public int movecount;
    void Start()
    {
        //레이어 검색해서 sortingOrder ++하기   
        //EnemyHpBar.transform.parent.gameObject.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        int OtherPrefabsCount = GameObject.FindGameObjectsWithTag("Enemy_Sushi").Length;
        spriteRenderer.sortingOrder = OtherPrefabsCount;
        playerPosY = GameObject.Find("Player");
        hp = base.CurHp;

        if (gameObject.tag == "Enemy_Sushi_RiceBall")
        {
            RiceBallMove();
            AttackRange.enabled = true;
        }
        
    }

    public void Update()
    {
        //ShowHpbar();
    }
    // 몬스터 hp 표시 안하기로해서 주석처리
    /*public IEnumerator BirthEnemyHpBar()
    {
        EnemyHpBar.transform.parent.gameObject.SetActive(true);
        Birthinvin = false;
        Debug.Log("hp bar 보여줘");
        yield return new WaitForSeconds(0.5f);
        transform.GetComponent<BoxCollider2D>().enabled = true;
        Birthinvin = true;
        //r2d.gravityScale = 1;
        yield return null;
    }*/
    void RiceBallMove()
    {
        //왼쪽 오른쪽 랜덤으로 카메라 끝까지 이동 후 반대 방향으로 움직임
        Vector3 currentY = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        Vector3 Left = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, currentY.y, 1));
        Vector3 Right = Camera.main.ViewportToWorldPoint(new Vector3(0.9f, currentY.y, 1));

        if (gameObject.transform.position == Left)
        {
            StartCoroutine(RiceBallMovePosition(Right));
            spriteRenderer.flipX = true;

        }
        else if (gameObject.transform.position == Right)
        {
            StartCoroutine(RiceBallMovePosition(Left));
            spriteRenderer.flipX = false;

        }
        else
        {
            int RanDir = Random.Range(0, 2);
            if (RanDir > 0) //오른쪽으로 시작
            {
                StartCoroutine(RiceBallMovePosition(Right));
                spriteRenderer.flipX = true;


            }
            else //왼쪽 시작
            {
                StartCoroutine(RiceBallMovePosition(Left));
                spriteRenderer.flipX = false;

            }
        }
    }

    IEnumerator NewEnemyAi(float time) //지정한 지점으로 이동 
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("IsSkill2Attack", false);
        
        Vector3 currentY = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        Vector3 playerX = Camera.main.WorldToViewportPoint(playerPosY.transform.position);
        Vector3 p = Camera.main.ViewportToWorldPoint(new Vector3(playerX.x, currentY.y, 1));
        float PositionX = p.x - gameObject.transform.position.x;

        if (PositionX >= 0f)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;

        }

        if (IsSalmonEggAttack && gameObject.tag == "Enemy_Sushi_SalmonEgg") //slamonegg만 공격
        {
            IsSalmonEggAttack = false;
            anim.SetTrigger("Enemy_Attack");
            StartCoroutine(SalmonEggAttackCool(4));
        }
        else
        {
            StartCoroutine(MovePosition(p));
        }
    }

    IEnumerator RiceBallMovePosition(Vector3 position) //지정한 좌표로 이동할떄 시간에 맞춰서 이동해야하는지?
    {
        float count = 0;
        Vector3 firstpos = gameObject.transform.position;

        while (true)
        {
            count += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(firstpos, position, count * EnemySpeed);
            if (count >= 2)
            {
                gameObject.transform.position = position;
                RiceBallMove();
                break;
            }
            yield return null;
        }

    }

    IEnumerator MovePosition(Vector3 position) //지정한 좌표로 이동할떄 시간에 맞춰서 이동해야하는지?
    {
        float count = 0;
        Vector3 firstpos = gameObject.transform.position;
        while (true)
        {
            count += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(firstpos, position, count * EnemySpeed);            
            anim.SetBool("Enemy_Move", true);
           
            if (sushitype != SushiType.LongType)
            {
                yield return new WaitUntil(() => AttackRange.enabled == true);
            }

            

            if (gameObject.transform.position == position)
            {
                StartCoroutine(NewEnemyAi(2f));
                
                break;

            }
          
            yield return null;
        }

    }
    IEnumerator SalmonEggAttackCool(float cooltime)
    {

        yield return new WaitForSeconds(cooltime);
        IsSalmonEggAttack = true;
    }

    IEnumerator SalmonEggAttack()
    {
        RaycastHit2D rayRange = Physics2D.Raycast(transform.position, Vector3.right, 20, LayerMask.GetMask("Player")); //왼쪽
        RaycastHit2D rayRange2 = Physics2D.Raycast(transform.position, Vector3.left, 20, LayerMask.GetMask("Player")); //오른쪽

        
        if (rayRange.collider != null)
        {
            Vector3 right = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
            GameObject Ball = Instantiate(SalmonEggBall, gameObject.transform.position, Quaternion.identity);
            spriteRenderer.flipX = true;
            Ball.transform.localScale = new Vector3(-1, 1, 1);
            //Ball.GetComponent<Bullet_RoseControl>().SalmonEggAttack(false);
        }
        else if (rayRange2.collider != null)
        {
            //Vector3 left = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
           Instantiate(SalmonEggBall, gameObject.transform.position, Quaternion.identity);
            spriteRenderer.flipX = false;

            //Ball.GetComponent<Bullet_RoseControl>().SalmonEggAttack(true);

            //Ball.GetComponent<Bullet_RoseControl>().SalmonEggAttack(new Vector3(left.x, transform.position.y, 1));

        }


        yield return null;

    }

    public IEnumerator SalmonEggDeath() //살몬에그 사망  -> 양쪽에 공격
    {
        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        GameObject LeftBall = Instantiate(SalmonEggBall, gameObject.transform.position, Quaternion.identity);
        //LeftBall.GetComponent<Bullet_RoseControl>().SalmonEggAttack(true);
        // LeftBall.GetComponent<Bullet_RoseControl>().SalmonEggAttack(new Vector3(left.x, transform.position.y, left.z));

        GameObject RightBall = Instantiate(SalmonEggBall, gameObject.transform.position, Quaternion.identity);
        RightBall.transform.localScale = new Vector3(-1, 1, 1);
        //RightBall.GetComponent<Bullet_RoseControl>().SalmonEggAttack(false);

        // RightBall.GetComponent<Bullet_RoseControl>().SalmonEggAttack(new Vector3(right.x, transform.position.y, left.z));

        yield return null;
    }


    public IEnumerator MoveAttack_true() //초밥이 점프할때만 움직여야함
    {
        AttackRange.enabled = true;
        //StopCoroutine(NewEnemyAi(0));
        //StopAllCoroutines();
        //corou//
        //EnemySpeed = 0;

        yield return null;
    }

    public IEnumerator MoveAttack_False()
    {
        //EnemySpeed = 1;
        AttackRange.enabled = false;
        yield return null;
    }

    public IEnumerator BirthStart()
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        r2d.gravityScale = 0;
        yield return null;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sim")
        {
            if (Birthinvin)
            {

                GetComponent<Enemy>().TakeDamage(20);
            }
            if (GetComponent<Enemy>().CurHp <= 0)
            {

            }
            else
            {
                CancelInvoke();
                StopAllCoroutines();
                anim.SetTrigger("Enemy_Attacked");
                anim.SetBool("IsSkill2Attack", true);
                Invoke("StarteNewEnemyAi", 2f);
            }

        }
        else if (collision.tag == "pen")
        {
            GetComponent<Enemy>().TakeDamage(20);

        }
        else if (collision.tag == "player" && gameObject.tag == "Enemy_Sushi_RiceBall") //맨밥이 플레이어와 충돌하였을 경우
        {
            collision.GetComponent<Playercontrol>().TakeDamage(10, gameObject.transform.position);
            GetComponent<Enemy>().TakeDamage(10);
        }

    }

    public void StarteNewEnemyAi()
    {
        anim.SetBool("IsSkill2Attack", false);
        StartCoroutine(NewEnemyAi(0));
    }

    public IEnumerator TofuDeath() //유부초밥 죽음 -> 맨밥 생성
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(RiceBall, new Vector3(gameObject.transform.position.x, -2.2f, gameObject.transform.position.z), Quaternion.identity);

    }
    public void DieAni()
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        CancelInvoke();
        StopAllCoroutines();

    }
    public void AttackedEnd()
    {
        anim.SetBool("Enemy_Move", true);       
    }

    private void Sk3_Attacked()
    {
        if (GameManger.Instance.EnemyState_Stern)
        {
            StopAllCoroutines();
            CancelInvoke();
        }


    }
    private void Attacked_StopMove()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        StopAllCoroutines();        
    }
    private void Sk3_end() //스킬3 맞고 피없으면 버그 생김
    {
        CancelInvoke();
        StopAllCoroutines();
        StartCoroutine(NewEnemyAi(0));
    }
}
