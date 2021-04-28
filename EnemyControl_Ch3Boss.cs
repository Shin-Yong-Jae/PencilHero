using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyControl_Ch3Boss : MonoBehaviour
{
    GameObject player;
    public GameObject sword;
    SpriteRenderer swordsprite;
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;
    Animator anim;
    public Transform Pos;
    public Transform Pos2;
    public Transform swordpos;
    public Transform swordpos2;
    public Vector2 BoxSize; //공격 범위 
    public Vector2 BoxSize2; //러쉬 충돌 범위
    bool EnemyMove = false;
    public bool isAttack2 = false;
    public GameObject Hpbar;
    public Transform Hppos;
    public Transform Hppos2;
    public Transform passivepos;
    public GameObject Inkpassive;
    private bool passivecool = false;
    private int passivelevel = 0;
    public GameObject GM;

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
        //sword.transform.position = sword.transform.position;
        swordsprite = sword.GetComponent<SpriteRenderer>();
        //StartCoroutine(AttackCool());
    }
    void FixedUpdate()
    {
        if (EnemyMove) // 이동 중 동작.
        {
            if (Vector2.Distance(player.transform.position, transform.position) < 10)
            {
                anim.SetBool("Enemy_Move", true);
                anim.ResetTrigger("Enemy_Idle");
            }
            else
            {
                anim.SetBool("Enemy_Move", false);
                anim.SetTrigger("Enemy_Idle");
            }
            EnemyMove = false;
        }
        if (isAttack2) // attack2 상호작용
        {
            //왼쪽 공격
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(swordpos.position, BoxSize2, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    swordsprite.flipX = false;
                    collider.gameObject.GetComponent<Playercontrol>().TakeDamage(100, transform.position);
                    isAttack2 = false;
                }
            }
            //오른족 공격
            collider2Ds = Physics2D.OverlapBoxAll(swordpos2.position, BoxSize2, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    swordsprite.flipX = true;
                    collider.gameObject.GetComponent<Playercontrol>().TakeDamage(100, transform.position);
                    isAttack2 = false;
                }
            }
        }
        if (gameObject.GetComponent<Enemy>().CurHp / gameObject.GetComponent<Enemy>().initHp <= 0.6
            && gameObject.GetComponent<Enemy>().CurHp / gameObject.GetComponent<Enemy>().initHp > 0.3)
        {
            passivelevel = 1;
            EnemySpeed = 4;
            if (passivecool == false) StartCoroutine(Inkbunsu());
        }
        else if (gameObject.GetComponent<Enemy>().CurHp / gameObject.GetComponent<Enemy>().initHp <= 0.3)
        {
            passivelevel = 2;
            EnemySpeed = 6;
            if (passivecool == false) StartCoroutine(Inkbunsu());
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
                spriteRenderer.flipX = false;
                anim.SetTrigger("Enemy_Attack");
                anim.SetBool("Enemy_Move", false);
                Hpbar.transform.position = new Vector2(Hppos.position.x, Hppos.position.y);
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
                Hpbar.transform.position = new Vector2(Hppos2.position.x, Hppos2.position.y);
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
                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(80, transform.position);
                //anim.SetTrigger("Enemy_Attack");
            }
        }
        //오른쪽 공격
        collider2Ds = Physics2D.OverlapBoxAll(Pos2.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(80, transform.position);
            }
        }
    }

    void NewEnemyAi()
    {
        anim.SetBool("Enemy_Move", false);
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
        Invoke("NewEnemyAi", 4f); //3초마다 계속 ai 재생
    }

    IEnumerator EnemyMoveToards(Collider2D vec)
    {
        if (Vector2.Distance(player.transform.position, transform.position) >= 10)
        {
            anim.SetBool("Enemy_Move", false);
            anim.SetTrigger("Enemy_Idle");
            anim.SetTrigger("Enemy_Attack2");
            StartCoroutine(Dashdelay());
        }
        else
        {
            while (Vector2.Distance(player.transform.position, transform.position) < 10)
            {
                transform.position = Vector2.MoveTowards(transform.position, vec.transform.position, Time.deltaTime * EnemySpeed);
                anim.SetBool("Enemy_Move", true);
                if (SearchAI())
                {
                    Debug.Log("공격 사거리 안에 들어옴");
                    anim.SetBool("Enemy_Move", false);
                    Invoke("NewEnemyAi", 4f); //5초마다 계속 ai 재생
                    break;
                }
                yield return null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sim")
        {
            //Debug.Log("심에 맞았어");
            //anim.SetBool("IsSkill2Attack", true);
            gameObject.GetComponent<Enemy>().TakeDamage(20);
        }
        else if (collision.tag == "pen")
        {
            gameObject.GetComponent<Enemy>().TakeDamage(20);
        }
    }
    IEnumerator Dashdelay()
    {
        yield return new WaitForSeconds(1f);
    }
    IEnumerator Inkbunsu()
    {
        //List<int> randomshot = new List<int> { 0, 1, 2, 3, 4 };
        if (passivelevel == 1)
        {
            int a = Random.Range(0, 5);
            GameObject Ins1 = Instantiate(Inkpassive, new Vector2(passivepos.transform.position.x + (8 * 0) + a, passivepos.transform.position.y),
                Quaternion.identity);
            Ins1.name = "Ink1";
            int b = Random.Range(0, 5);
            GameObject Ins2 = Instantiate(Inkpassive, new Vector2(passivepos.transform.position.x + (8 * 1) + b, passivepos.transform.position.y),
                Quaternion.identity);
            Ins2.name = "Ink2";
            int c = Random.Range(0, 5);
            GameObject Ins3 = Instantiate(Inkpassive, new Vector2(passivepos.transform.position.x + (8 * 2) + c, passivepos.transform.position.y),
                Quaternion.identity);
            Ins3.name = "Ink3";
        }
        else if(passivelevel==2)
        {
            int a = Random.Range(0, 4);
            GameObject Ins1 = Instantiate(Inkpassive, new Vector2(passivepos.transform.position.x + (4.5f * 0) + a, passivepos.transform.position.y),
                Quaternion.identity);
            Ins1.name = "Ink1";

            int b = Random.Range(0, 4);
            GameObject Ins2 = Instantiate(Inkpassive, new Vector2(passivepos.transform.position.x + (4.5f * 1) + b, passivepos.transform.position.y),
                Quaternion.identity);
            Ins2.name = "Ink2";

            int c = Random.Range(0, 4);
            GameObject Ins3 = Instantiate(Inkpassive, new Vector2(passivepos.transform.position.x + (4.5f * 2) + c, passivepos.transform.position.y),
                Quaternion.identity);
            Ins3.name = "Ink3";

            int d = Random.Range(0, 4);
            GameObject Ins4 = Instantiate(Inkpassive, new Vector2(passivepos.transform.position.x + (4.5f * 3) + c, passivepos.transform.position.y),
                Quaternion.identity);
            Ins4.name = "Ink4";

            int e = Random.Range(0, 4);
            GameObject Ins5 = Instantiate(Inkpassive, new Vector2(passivepos.transform.position.x + (4.5f * 4) + c, passivepos.transform.position.y),
                Quaternion.identity);
            Ins5.name = "Ink5";
        }

        passivecool = true;
        yield return new WaitForSeconds(7f);
        passivecool = false;
    }
    public void Rush()
    {
        isAttack2 = true;
    }
    public void RandomInk()
    {
    }
    public void FinishRush() // 애니메이션 안 이중 방지 코드.
    {
        isAttack2 = false;
    }
    public void boss_die()
    {
        Debug.Log("die");
        GM.GetComponent<GameManger>().boss_die = true;
    }
    public void Attack2()
    {
        var heading = player.transform.position - transform.position; // 대쉬중인 나이트와 플레이어의 방향 계산
        var distance = heading.magnitude;
        var direction = heading / distance; // 방향벡터
        sword.SetActive(true);
        if (!spriteRenderer.flipX) sword.transform.position = new Vector2(swordpos.position.x, swordpos.position.y);
        else sword.transform.position = new Vector2(swordpos2.position.x, swordpos2.position.y - 0.2f);
    }
    public void FinishAttack2()
    {
        sword.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(Pos.position, BoxSize);
        Gizmos.DrawCube(swordpos.position, BoxSize2);
    }
}

