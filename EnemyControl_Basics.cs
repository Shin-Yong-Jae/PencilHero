using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl_Basics : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;

    private float curTime;
    Vector2 startPosition;
    public GameObject EnemyHpBar;
    Animator anim;
    bool EnemyAttack_First = true;
    bool EnemyMove = false;
    public GameObject DamageTxt;
    public bool OnScreen = false;

    private GameObject MainCamera;
    public bool InCamera = false;


    [Header("Status")]
    public float initHp;
    public float currHp;
    public float attkDistance; //적 공격범위
    public float findRange; //플레이어 찾는 거리
    public int EnemySpeed; //적 속도
    public float attackcolltime;
    private void Awake()
    {
    }
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currHp = initHp;
        anim.SetBool("Enemy_Move", false);
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");


    }
    public bool Attack1()
    {
        if (gameObject.tag == "Enemy_Horse")
        {
           return gameObject.GetComponent<EnemyControl_Horse>().AiAttack();
        }else if (gameObject.tag == "Enemy_Big")
        {
            return gameObject.GetComponent<EnemyControl_Big>().AiAttack();
        }
        else if (gameObject.tag == "Enemy_Rose")
        {
            return gameObject.GetComponent<EnemyControl_Rose>().AiAttack();
        }

        return false;
    }

    void StartPositionValue()
    {
        startPosition = transform.position;
    }

    private void OnBecameVisible()
    {
        StartCoroutine(NewEnemyAi(2f));
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.right * findRange, Color.yellow); //플레이어 발견 범위
        if (EnemyMove)
        {
            anim.SetBool("Enemy_Move", true);
            EnemyMove = false;
        }
        Vector3 screenPoint = MainCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);//화면안에 있나
        if (screenPoint.x > 0.0f && screenPoint.x < 1.0f && screenPoint.y < 1.0f && screenPoint.y > 0.0f)
        {
            InCamera = true;
        }
        else InCamera = false;
    }

    public void TakeDamage(float hit)
    {
        currHp -= hit;
        float offsetX = Random.Range(-0.5f, 2f);
        float offsetY = Random.Range(0.5f, 2f);
        if (hit == 60)
        {
            GameObject hitDamage = Instantiate(DamageTxt, new Vector3(gameObject.transform.position.x + offsetX, gameObject.transform.position.y + 1 + offsetY, gameObject.transform.position.z), Quaternion.identity);
            hitDamage.GetComponent<MeshRenderer>().material = hitDamage.GetComponent<HitText>().material;
            hitDamage.GetComponent<HitText>().hit = (int)hit;
        }
        else
        {
            GameObject hitDamage = Instantiate(DamageTxt, new Vector3(gameObject.transform.position.x + offsetX, gameObject.transform.position.y + 1 + offsetY, gameObject.transform.position.z), Quaternion.identity);
            hitDamage.GetComponent<HitText>().hit = (int)hit;
        }

        if (gameObject.tag == "Enemy_Big")
        {
            int rnd = Random.Range(0, 2);
            if(rnd >= 1)
            {
                anim.SetTrigger("Enemy_Attacked1");

            }
            else
            {
                anim.SetTrigger("Enemy_Attacked2");

            }
        }
        else
        {
            anim.SetTrigger("Enemy_Attacked");
        }
        
        anim.SetBool("Enemy_Move", false);
        StartCoroutine(Damage());

        if (currHp <= 0)
        {
            //사망
            anim.SetTrigger("Enemy_Die");
            transform.GetComponent<BoxCollider2D>().enabled = false;
            r2d.isKinematic = true;
            CancelInvoke();
            StartCoroutine(EnemyDeath());
        }
    }

    IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1f);
    }

    IEnumerator NewEnemyAi(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("Enemy_Move", false);
        anim.SetBool("IsSkill2Attack", false);
        
        if (Attack1() && !GameManger.Instance.EnemyState_Stern) //공격 사정거리에 있느가? 있으면 공격
        {

        }
        else //공격 사정거리에 없다 -> 플레이어를 찾아서 사정거리까지 걸어가자
        {

                RaycastHit2D rayRange = Physics2D.Raycast(transform.position, Vector3.right, findRange, LayerMask.GetMask("Player")); //왼쪽
                RaycastHit2D rayRange2 = Physics2D.Raycast(transform.position, Vector3.left, findRange, LayerMask.GetMask("Player")); //오른쪽

            if (gameObject.tag == "Enemy_Big")
            {
                rayRange = Physics2D.Raycast(new Vector3(transform.position.x, -1, transform.position.z), Vector3.right, findRange, LayerMask.GetMask("Player")); //왼쪽
                rayRange2 = Physics2D.Raycast(new Vector3(transform.position.x, -1, transform.position.z), Vector3.left, findRange, LayerMask.GetMask("Player")); //오른쪽
            }


            if (rayRange.collider != null)
            {
                if (Vector2.Distance(transform.position, rayRange.collider.transform.position) <= attkDistance) //사거리 안
                {
                    Attack1();

                }
                else
                {
                    spriteRenderer.flipX = false;
                    CancelInvoke();
                    StartCoroutine(EnemyMoveToards(rayRange.collider));
                    EnemyMove = true;
                }
            }
            else if (rayRange2.collider != null)
            {
                if (Vector2.Distance(transform.position, rayRange2.collider.transform.position) <= attkDistance)
                {
                    Attack1();
                }
                else
                {
                    spriteRenderer.flipX = true;
                    CancelInvoke();
                    StartCoroutine(EnemyMoveToards(rayRange2.collider));
                    EnemyMove = true;
                }
            }
        }

      
            CancelInvoke();        
        StartCoroutine(NewEnemyAi(attackcolltime));
       
    }

    IEnumerator EnemyMoveToards(Collider2D vec)
    {

        while (!GameManger.Instance.EnemyState_Stern)
        {
            transform.position = Vector2.MoveTowards(transform.position, vec.transform.position, Time.deltaTime * EnemySpeed);
            anim.SetBool("Enemy_Move", true);
            if (Attack1())
            {
                Debug.Log("공격 사거리 안에 들어옴");
                anim.SetBool("Enemy_Move", false);
                anim.SetBool("IsSkill2Attack", false);
               // StartCoroutine(NewEnemyAi(5f));

                //Invoke("NewEnemyAi", 5f); //5초마다 계속 ai 재생
                break;
            }
            yield return null;
        }
    }



    void Update()
    {
        //EnemyHpBar.GetComponent<Image>().fillAmount = (currHp / initHp);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sim")
        {
            GetComponent<Enemy>().TakeDamage(20);
            if (GetComponent<Enemy>().CurHp <= 0)
            {

            }
            else
            {
                if (gameObject.tag == "Enemy_Big")
                {
                    anim.SetTrigger("Enemy_Attacked");

                }
                else
                {
                    anim.SetTrigger("Enemy_Attacked");

                }
                anim.SetBool("IsSkill2Attack", true);
                CancelInvoke();
                StopAllCoroutines();
                StartCoroutine(NewEnemyAi(2f));
                //Invoke("NewEnemyAi", 2.2f);
            }

        }
        else if (collision.tag == "pen")
        {
            GetComponent<Enemy>().TakeDamage(20);

        }

    }
    public void KockTakeDamage(float hit, Vector2 Pos)
    {
        currHp -= hit;
        float offsetX = Random.Range(-0.5f, 2f);
        float offsetY = Random.Range(0.5f, 2f);
        GameObject hitDamage = Instantiate(DamageTxt, new Vector3(gameObject.transform.position.x + offsetX, gameObject.transform.position.y + 1 + offsetY, gameObject.transform.position.z), Quaternion.identity);
        hitDamage.GetComponent<HitText>().hit = (int)hit;
        if (gameObject.tag == "Enemy_Big")
        {
            anim.SetTrigger("Enemy_Attacked1");
        }
        else
        {
            anim.SetTrigger("Enemy_Attacked");
        }
        anim.SetBool("Enemy_Move", false);
        StartCoroutine(Damage());

        if (currHp <= 0)
        {
            //사망
            anim.SetTrigger("Enemy_Die");
            transform.GetComponent<BoxCollider2D>().enabled = false;
            r2d.isKinematic = true;
            CancelInvoke();
            StartCoroutine(EnemyDeath());
        }

        float x = transform.position.x - Pos.x;
        if (x < 0)
        {
            x = 1;
        }
        else
        {
            x = -1;
        }
        StartCoroutine(Kockback(x));
    }
    IEnumerator Kockback(float dir)
    {
        float KnockTime = 0;
        while (KnockTime < 0.2f)
        {

            transform.Translate(Vector2.left * 5f * Time.deltaTime * dir);
            KnockTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator EnemyDie()
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        r2d.isKinematic = true;
        CancelInvoke();
        StartCoroutine(EnemyDeath());
        yield return null;
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
        CancelInvoke();
        StopAllCoroutines();
        StartCoroutine(NewEnemyAi(0));
    }
}
