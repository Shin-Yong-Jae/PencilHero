using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyControl : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;

    public Transform Pos;
    public Transform Pos2;

    public Vector2 BoxSize;
    private float curTime;
    Vector2 startPosition;
    GameObject EnemyHpBar;
    Animator anim;
    bool EnemyAttack_First = true;
    bool EnemyMove = false;
    private float initHp = 100.0f;
    public GameObject DamageTxt;
    [Header("Status")]
    public float currHp;
    public float attkDistance; //적 공격범위
    public float findRange; //플레이어 찾는 거리
    public int EnemySpeed; //적 속도

    private GameObject MainCamera;
    public bool InCamera = false;

    private void Awake()
    {
    }
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
        currHp = initHp;
        EnemyHpBar = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject;
        //Invoke("StartPositionValue", 1f);
        anim.SetTrigger("Enemy_Idle");

        Invoke("NewEnemyAi", 2f);
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void StartPositionValue()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.right * findRange, Color.yellow); //플레이어 발견 범위


        if (EnemyMove)
        {
            anim.SetTrigger("Enemy_Move");
            EnemyMove = false;
        }
    }

    public void TakeDamage(int hit)
    {
        currHp -= hit;
        GameObject hitDamage = Instantiate(DamageTxt, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), Quaternion.identity);
        hitDamage.GetComponent<HitText>().hit = hit;
        anim.SetTrigger("Enemy_Attacked");
        anim.ResetTrigger("Enemy_Move");
        StartCoroutine(Damage());
        if (currHp <= 0)
        {
            //사망
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

    void NewEnemyAi()
    {
        Debug.Log("AI 시작한다");



        if (AiAttack()) //공격 사정거리에 있느가? 있으면 공격
        {
            Debug.Log("공격 사정거리에 있다 공격한다");
        }
        else //공격 사정거리에 없다 -> 플레이어를 찾아서 사정거리까지 걸어가자
        {
            Debug.Log("공격 사정거리에 없다 -> 캐릭터를 찾아라");
            RaycastHit2D rayRange = Physics2D.Raycast(transform.position, Vector3.right, findRange, LayerMask.GetMask("Player")); //왼쪽
            RaycastHit2D rayRange2 = Physics2D.Raycast(transform.position, Vector3.left, findRange, LayerMask.GetMask("Player")); //오른쪽
            if (rayRange.collider != null)
            {
                if (Vector2.Distance(transform.position, rayRange.collider.transform.position) <= attkDistance)
                {
                    AiAttack();
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
                    AiAttack();
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
        Invoke("NewEnemyAi", 5f); //5초마다 계속 ai 재생
    }

    IEnumerator EnemyMoveToards(Collider2D vec)
    {

        while (true)
        {

          transform.position = Vector2.MoveTowards(transform.position, vec.transform.position, Time.deltaTime * EnemySpeed);

            if (AiAttack())
            {
                Debug.Log("공격 사거리 안에 들어옴");
                Invoke("NewEnemyAi", 5f); //5초마다 계속 ai 재생
                break;
            }
            yield return null;

        }
        

    }

    bool AiAttack()
    {
        //왼쪽 공격

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(1, transform.position);
                anim.SetTrigger("Enemy_Attack2");
                anim.ResetTrigger("Enemy_Move");
                spriteRenderer.flipX = false;

                return true;
            }

        }
        //오른쪽 공격
        collider2Ds = Physics2D.OverlapBoxAll(Pos2.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(1, transform.position);
                anim.SetTrigger("Enemy_Attack2");
                anim.ResetTrigger("Enemy_Move");
                spriteRenderer.flipX = true;

                return true;
            }
        }

        return false; // 범위안에 플레이어 없음
    }

    void AiAttackT() 
    {
        //왼쪽 공격

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(1, transform.position);
                anim.SetTrigger("Enemy_Attack2");
                anim.ResetTrigger("Enemy_Move");
            }

        }
        //오른쪽 공격
        collider2Ds = Physics2D.OverlapBoxAll(Pos2.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(1, transform.position);
                anim.SetTrigger("Enemy_Attack2");
                anim.ResetTrigger("Enemy_Move");

            }
        }


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(Pos.position, BoxSize);
    }

    void Update()
    {
        EnemyHpBar.GetComponent<Image>().fillAmount = (currHp / initHp);
    }
    public void CheckIn()//카메라 뷰 안에 적이 있는지 없는지
    {
            Vector3 screenPoint = MainCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
            if (screenPoint.x > 0.0f && screenPoint.x < 1.0f && screenPoint.y < 1.0f && screenPoint.y > 0.0f)
            {
                InCamera = false;
            }
            else InCamera = true;
        }
    }

