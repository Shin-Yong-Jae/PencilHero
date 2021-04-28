using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl_Knight : MonoBehaviour
{
    GameObject player;
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;
    Animator anim;
    public Transform Pos;
    public Transform Pos2;
    public Transform CenterPos;
    public Vector2 BoxSize; //공격 범위 
    public Vector2 BoxSize2; //러쉬 충돌 범위
    bool EnemyMove = false;
    public bool isRushing = false;
    [Header("Status")]
    public float attkDistance; //적 공격범위
    public float findRange; //플레이어 찾는 거리
    public int EnemySpeed; //적 속도
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        r2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetBool("Enemy_Move", false);
        //StartCoroutine(AttackCool());
    }
    void FixedUpdate()
    {
        if (EnemyMove) // 이동 중 동작.
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 10.5f)
            {
                anim.SetBool("Enemy_Move", true);
                anim.ResetTrigger("Enemy_Idle");
            }
            else
            {
                anim.SetBool("Enemy_Move", false);
                //anim.SetTrigger("Enemy_Idle");
            }
            EnemyMove = false;
        }
        if (isRushing) // 대쉬 중 동작.
        {
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(CenterPos.position, BoxSize2, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    collider.gameObject.GetComponent<Playercontrol>().TakeDamage(50, transform.position);
                    var heading = player.transform.position - transform.position; // 대쉬중인 나이트와 플레이어의 방향 계산
                    var distance = heading.magnitude;
                    var direction = heading / distance; // 방향벡터
                    if (collider.gameObject.GetComponent<Playercontrol>().isQuake == false
                        && collider.gameObject.GetComponent<Playercontrol>().DashCoolDone==true)
                    {
                        collider.gameObject.GetComponent<Playercontrol>().isQuake = true;
                        collider.gameObject.GetComponent<Playercontrol>().r2d.AddForce(direction * 1600f);
                        r2d.AddForce(-direction * 1600f);
                        StartCoroutine(CanQuake());
                    }
                    isRushing = false;
                }
            }
        }

    }
    private void OnBecameVisible()
    {
        Invoke("NewEnemyAi", 2f);
    }

    public bool SearchAI()
    {
        //왼쪽 공격
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                //collider.gameObject.GetComponent<Playercontrol>().TakeDamage(15, transform.position);               
                spriteRenderer.flipX = false;
                anim.SetTrigger("Enemy_Attack");
                anim.SetBool("Enemy_Move", false);
                return true;
            }
        }
        //오른쪽 공격
        collider2Ds = Physics2D.OverlapBoxAll(Pos2.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                //collider.gameObject.GetComponent<Playercontrol>().TakeDamage(15, transform.position);                
                anim.SetTrigger("Enemy_Attack");
                anim.SetBool("Enemy_Move", false);
                spriteRenderer.flipX = true;
                return true;
            }
        }
        return false; // 범위안에 플레이어 없음
    }
    public void AiAttackT()
    {
        //왼쪽 공격
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(15, transform.position);
                //anim.SetTrigger("Enemy_Attack");
            }
        }
        //오른쪽 공격
        collider2Ds = Physics2D.OverlapBoxAll(Pos2.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(15, transform.position);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sim")
        {
            anim.SetTrigger("Enemy_Attacked");
            anim.SetBool("IsSkill2Attack", true);
            gameObject.GetComponent<Enemy>().TakeDamage(20);
            Invoke("NewEnemyAi", 3f);
        }
        else if (collision.tag == "pen")
        {
            gameObject.GetComponent<Enemy>().TakeDamage(20);
        }
    }

    void NewEnemyAi()
    {
        anim.SetBool("Enemy_Move", false);
        anim.SetBool("IsSkill2Attack", false);

        if (SearchAI()) //공격 사정거리에 있느가? 있으면 공격
        {
        }
        else //공격 사정거리에 없다 -> 플레이어를 찾아서 사정거리까지 걸어가자
        {

            RaycastHit2D rayRange = Physics2D.Raycast(transform.position, Vector3.right, findRange, LayerMask.GetMask("Player")); //왼쪽
            RaycastHit2D rayRange2 = Physics2D.Raycast(transform.position, Vector3.left, findRange, LayerMask.GetMask("Player")); //오른쪽

            if (rayRange.collider != null)
            {
                if (Vector2.Distance(transform.position, rayRange.collider.transform.position) <= attkDistance) //사거리 안
                {
                    SearchAI();
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
                    SearchAI();
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
        Invoke("NewEnemyAi", 3f); //3초마다 계속 ai 재생
    }

    IEnumerator EnemyMoveToards(Collider2D vec)
    {
        if (Vector2.Distance(player.transform.position, transform.position) >= 10.5f)
        {
            anim.SetBool("Enemy_Move", false);
            anim.SetTrigger("Enemy_Idle");
            anim.SetTrigger("Enemy_Rush");
            StartCoroutine(Dashdelay());
        }
        else
        {
            while (Vector2.Distance(player.transform.position, transform.position) < 10.5f)
            {
                EnemyMove=true;
                transform.position = Vector2.MoveTowards(transform.position, vec.transform.position, Time.deltaTime * EnemySpeed);
                anim.SetBool("Enemy_Move", true);
                if (SearchAI())
                {
                    anim.SetBool("Enemy_Move", false);
                    Invoke("NewEnemyAi", 3f); //5초마다 계속 ai 재생
                    break;
                }
                yield return null;
            }
        }
    }
    IEnumerator Dashdelay()
    {
        var heading = player.transform.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance; // 방향벡터
        yield return new WaitForSeconds(1f);
    }
    IEnumerator CanQuake()
    {
        yield return new WaitForSeconds(1.0f);
        player.GetComponent<Playercontrol>().isQuake = false;
    }
    public void Rush()
    {
        isRushing = true;
        var heading = player.transform.position - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance; // 방향벡터
        r2d.AddForce(direction * 1600f);
    }
    public void FinishRush() // 애니메이션 안 이중 방지 코드.
    {
        isRushing = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(Pos.position, BoxSize);
        Gizmos.DrawCube(CenterPos.position, BoxSize2);
    }
}
