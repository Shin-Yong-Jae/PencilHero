using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions.Must;

using UnityEngine.Timeline;

public class EnemyControl_BossSushi : Enemy
{

    //Rigidbody2D r2d;
    SpriteRenderer spriteRenderer;
    //Animator anim;
    //private GameObject MainCamera;
    //public bool InCamera = false;
    //public GameObject EnemyHpBar;

    //    GameObject EnemyHpBar;
    public bool Player_Field = false;


    [Header("소환할 초밥몬스터")]
    public GameObject Enemy_Sushi_Egg;
    public GameObject Enemy_Sushi_Salmon;
    public GameObject Enemy_Sushi_Shrimp;
    public GameObject Enemy_Sushi_Tofu;
    public GameObject Enemy_Sushi_SalmonEgg;
    public GameObject Enemy_Sushi_KingSushi;

    //public GameObject DamageTxt;

    //public GameObject TimeLineTest;

    [Header("Status")]
    //public float initHp;
    //public float currHp;
    public float findRange; //플레이어 찾는 거리
    public float EnemySpeed; //적 속도
    public float SummonsCool; //소환 쿨타임
    public bool Summons = true;
    public int MaxSushi;
    public int CurrentSushi;
    public int CurrentField;

    [SerializeField]
    private int nextFieldCount;
    [SerializeField]
    private float playerBetween;
    private GameObject playerPos;
    Enemy enemy;
    Vector3 SumonsPos;
    public bool fieldMove = false;
    void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>();
        playerPos = GameObject.Find("Player");
        StartNewEnemyAi();
    }

    void Update()
    {
        /* EnemyHpBar.GetComponent<Image>().fillAmount = (currHp / initHp);
         Vector3 screenPoint = MainCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);//화면안에 있나
         if (screenPoint.x > 0.0f && screenPoint.x < 1.0f && screenPoint.y < 1.0f && screenPoint.y > 0.0f)
         {
             InCamera = true;
         }
         else InCamera = false;*/

        CurrentSushi = FindObjectsOfType<EnemyControl_Sushi>().Length;



    }

    public void StartNewEnemyAi()
    {
        StartCoroutine(NewEnemyAi());
    }
    IEnumerator NewEnemyAi() //지정한 지점으로 이동 
    {
        Vector3 pos = Vector3.zero;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        fieldMove = false;
        if (Vector2.Distance(transform.position, playerPos.transform.position) <= playerBetween) //플레이어와 보스스시의 거리가 지정한 값보다 작다면 반대방향으로 이동
        {


            if (Camera.main.WorldToViewportPoint(playerPos.transform.position).x > 0.4f) //오른쪽
            {
                pos = new Vector3(transform.position.x - playerBetween, transform.position.y, transform.position.z);
            }
            else
            {
                pos = new Vector3(transform.position.x + playerBetween, transform.position.y, transform.position.z);
            }

            StartCoroutine(MovePosition(pos));

        }
        else
        {
            float RandomX = Random.Range(0.2f, 0.8f);
            Vector3 currentY = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            Vector3 p = Camera.main.ViewportToWorldPoint(new Vector3(RandomX, currentY.y, 1));

            float PositionX = p.x - gameObject.transform.position.x;

            if (PositionX >= 0f)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;

            }

            StartCoroutine(MovePosition(p));

            if (Summons && CurrentSushi < MaxSushi) //소환가능 상태
            {
                Summons = false;
                StopAllCoroutines();
                anim.SetTrigger("Attack");
                //StartCoroutine(SummonsCoolTime());  // 소환 쿨타임 주기
                Invoke("SummonsCoolTime2", SummonsCool);
                Invoke("StartNewEnemyAi", 1f);
            }
        }
        yield return null;
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

            if (gameObject.transform.position == position)
            {
                anim.SetBool("Enemy_Move", false);

                if (enemy.InCamera)
                {
                    Invoke("StartNewEnemyAi", 3f);

                }
                break;
            }
                /*
                if (count >= 7)
            {
                gameObject.transform.position = position;
                
                anim.SetBool("Enemy_Move", false);

                if (enemy.InCamera)
                {
                    Invoke("StartNewEnemyAi", 3f);
                }

                break;
            }*/
            yield return null;
        }
    }

    IEnumerator BossMovePosition(Vector3 position) //지정한 좌표로 이동할떄 시간에 맞춰서 이동해야하는지?
    {
        float count = 0;
        Vector3 firstpos = gameObject.transform.position;
        enemy.CurHp = enemy.initHp;

        while (true)
        {
            count += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(firstpos, position, count * EnemySpeed);
            anim.SetBool("Enemy_Move", true);

            if (count >= 3)
            {
                gameObject.transform.position = position;
                anim.SetBool("Enemy_Move", false);
                if (enemy.InCamera)
                {
                    Invoke("StartNewEnemyAi", 3f);
                    fieldMove = false;
                }

                break;
            }
            yield return null;
        }
    }


    IEnumerator SummonsCoolTime()
    {
        yield return new WaitForSeconds(SummonsCool);
        Summons = true;
    }
    public void SummonsCoolTime2()
    {
        Summons = true;
    }
    public void IsSummonsAni() //소환 애니메이션에서 작동
    {
        StartCoroutine(IsSummons(CurrentField));
    }
    IEnumerator IsSummons(int field) //몬스터 소환함수(참치, 새우, 계란, 알, 유부)
    {
        //int RandomCount = 1;
        SumonsPos = new Vector3(transform.position.x, -2.2f, transform.position.z);
        /*if (field > 2) //필드 2이상부터 몬스터 3가지 랜덤으로
        {
            RandomCount = Random.Range(1, 4);
        }
        else
        {
            RandomCount = Random.Range(1, 3);
        }*/
        float randomtmp = Random.Range(0f, 1f);
        switch (field)
        {

            case 1:
                Instantiate(Enemy_Sushi_Salmon, SumonsPos, Quaternion.identity);
                break;

            case 2:
                if (randomtmp <= 0.3f)
                {
                    Instantiate(Enemy_Sushi_Salmon, SumonsPos, Quaternion.identity);

                }
                else
                {
                    Instantiate(Enemy_Sushi_Shrimp, SumonsPos, Quaternion.identity);
                }

                break;


            case 3:
                if (randomtmp <= 0.3f)
                {
                    Instantiate(Enemy_Sushi_Shrimp, SumonsPos, Quaternion.identity);

                }
                else
                {
                    Instantiate(Enemy_Sushi_Egg, SumonsPos, Quaternion.identity);
                }
                break;

            case 4:

                if (randomtmp <= 0.3f)
                {
                    Instantiate(Enemy_Sushi_Egg, SumonsPos, Quaternion.identity);

                }
                else
                {
                    Instantiate(Enemy_Sushi_SalmonEgg, SumonsPos, Quaternion.identity);
                }
                break;
            case 5:

                if (randomtmp <= 0.3f)
                {
                    Instantiate(Enemy_Sushi_SalmonEgg, SumonsPos, Quaternion.identity);

                }
                else
                {
                    Instantiate(Enemy_Sushi_Tofu, SumonsPos, Quaternion.identity);
                }
                break;
            case 6:
            case 7:
                if (randomtmp <= 0.2f)
                {
                    Instantiate(Enemy_Sushi_Salmon, SumonsPos, Quaternion.identity);
                }
                else if (randomtmp <= 0.4f)
                {
                    Instantiate(Enemy_Sushi_Shrimp, SumonsPos, Quaternion.identity);
                }
                else if (randomtmp <= 0.6f)
                {
                    Instantiate(Enemy_Sushi_Egg, SumonsPos, Quaternion.identity);
                }
                else if (randomtmp <= 0.8f)
                {
                    Instantiate(Enemy_Sushi_SalmonEgg, SumonsPos, Quaternion.identity);
                }
                else
                {
                    Instantiate(Enemy_Sushi_Tofu, SumonsPos, Quaternion.identity);
                }
                break;
        }

        yield return null;
    }



    public void CheckHp() //HP 50% 미만 -> 기본 이동 속도 * 2  / HP 25% 미만 -> 기본 이동 속도 * 3 
    {
        if (enemy.CurHp / enemy.initHp * 100 <= 10)
        {
            EnemySpeed = 2f;            
        }
        else if (enemy.CurHp / enemy.initHp * 100 <= 40)
        {
            EnemySpeed = 1.5f;
        }
        else if (enemy.CurHp / enemy.initHp * 100 <= 70)
        {
            EnemySpeed = 1f;
        }

    }

    public override void TakeDamage(float hit)
    {
        GetComponent<Enemy>().TakeDamage(hit);
        CheckHp();

        if (enemy.CurHp / enemy.initHp * 100 <= 1)
        {
            StartCoroutine(NextField(CurrentField));
            r2d.isKinematic = true;
        }
    }

    IEnumerator NextField(int filed)
    {
        if (filed >= nextFieldCount)  //필드7단계면 보스가 바뀜
        {
            transform.GetComponent<BoxCollider2D>().enabled = false;
            r2d.isKinematic = true;
            anim.SetBool("IsBossSkill", true);
            spriteRenderer.flipX = false;
            anim.SetTrigger("Enemy_Die");
            //playerPos.GetComponent<Warp>().wallcheck++;           
            GameObject.Find("Main Camera").GetComponent<CameraFunc>().CanWarp = true;
            CancelInvoke();
        }
        else //다음필드로 이동하도록 만들면 됨
        {
            StopAllCoroutines();
            CancelInvoke();
            anim.SetBool("sk3_IsAttacked", false);
            CurrentField++;
            enemy.InCamera = false;
            transform.GetComponent<BoxCollider2D>().enabled = false;
            r2d.gravityScale = 0;            
            Invoke("MoveNextField", 3f);
        }
        yield return null;
    }
    public void MoveNextField()
    { 
        Vector3 currentY = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        Vector3 p = Camera.main.ViewportToWorldPoint(new Vector3(1.9f, currentY.y, 1));
        r2d.isKinematic = false;
        fieldMove = true;
        StartCoroutine(BossMovePosition(p));
        spriteRenderer.flipX = false;// 이동했을떄 왼쪽만바라보게
    }

    public override IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sim")
        {
            enemy.TakeDamage(20);
            if (enemy.CurHp <= 0)
            {
                StartCoroutine(NextField(CurrentField));
                r2d.isKinematic = true;
            }
            else
            {
                anim.SetTrigger("Enemy_Attacked");
                anim.SetBool("IsSkill2Attack", true);
                CancelInvoke();
                StopAllCoroutines();
                //StartCoroutine(SummonsCoolTime());
                Invoke("SummonsCoolTime2", SummonsCool);
                Invoke("StartNewEnemyAi", 2.2f);

            }

        }
        else if (collision.tag == "pen")
        {
            //enemy.TakeDamage(20);
            if (enemy.CurHp <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(NextField(CurrentField));
                r2d.isKinematic = true;
            }
        }
    }

    private void Sk3_Attacked()
    {
        if (GameManger.Instance.EnemyState_Stern)
        {
            StopAllCoroutines();
            CancelInvoke();
        }


    }
    private void Sk3_end() //스킬3 맞고 피없으면 버그 생김
    {
        if (enemy.CurHp <= 0)
        {
           /* StopAllCoroutines();
            StartCoroutine(NextField(CurrentField));
            r2d.isKinematic = true;*/
        }
        else
        {            
            if(enemy.InCamera)
            StartCoroutine(NewEnemyAi());

            Invoke("SummonsCoolTime2", 3f);
        }
    }

}
