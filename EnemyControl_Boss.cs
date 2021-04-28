using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl_Boss : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;

    private float curTime;
    GameObject EnemyHpBar;
    public Animator anim;
    bool EnemyMove = false;
    public GameObject DamageTxt;
    public bool OnScreen = false;

    private GameObject MainCamera;
    public bool InCamera = false;

    public Transform Pos;
    public Transform Pos2;
    public Vector2 BoxSize;
    public GameObject Shine;
    public GameObject ShineRange;

    [Header("소환할 몬스터")]
    public GameObject Enemy_Big;
    public GameObject Enemy_Horse;
    public GameObject Enemy_Rose;


    [Header("Status")]
    public float initHp;
    public float currHp;
    public float attkDistance; //적 공격범위
    public float findRange; //플레이어 찾는 거리
    public int EnemySpeed; //적 속도
    public int BossState = 1;
    public bool IsBossSkill_Shine;
    public bool IsBossSkill_Anger;
    public GameObject enemygroup;
    public GameObject chater1story;

    public int bossskil3count = 0;
    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currHp = initHp;
        EnemyHpBar = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        anim.SetBool("Enemy_Move", false);
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(Pos.position, BoxSize);
    }

    public bool Attack1()
    {

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                EnemyHpBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Left;
                BossPattern(BossState);
                return true;
            }

        }
        //오른쪽 공격
        collider2Ds = Physics2D.OverlapBoxAll(Pos2.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                EnemyHpBar.GetComponent<Image>().fillOrigin = (int)Image.OriginHorizontal.Right;
                BossPattern(BossState);
                return true;
            }
        }

        return false; // 범위안에 플레이어 없음
    }

 
    public void BossPattern(int BossState)
    {
        anim.SetBool("IsBossSkill", true); //공격받는 애니메이션 방지


        switch (BossState)
        {
            case 1:
                BossAttack_Tear();
                break;
            case 2:
                BossAttack_Shine();
                break;
            case 3:
                BossAttack_Anger();
                break;
        }
        anim.ResetTrigger("Enemy_Attacked");

    }

    public void BossAttack_Tear()
    {
        anim.SetTrigger("Boss_Skill_Tear");
    }
    public void BossAttack_Shine()//3초마다
    {
        if (IsBossSkill_Shine)
        {//1초동안 제자리 정지
            ShineRange.SetActive(true);
            //사거리 보여주고 1초후 발동
            CancelInvoke();
           // StopAllCoroutines();
            StartCoroutine(BossAttak_Shine_true());
            IsBossSkill_Shine = false;

        }

    }
    IEnumerator BossAttak_Shine_true()
    {
        yield return new WaitForSeconds(1f);
        ShineRange.SetActive(false);
        Shine.SetActive(true); //나중에 애니메이션 추가
        anim.SetTrigger("Boss_Skill_Shine");
    }

    public void BossAttack_Shine_false()
    {
        Shine.SetActive(false);
        StartCoroutine(BossAttack_ShineColl(3f));
        Invoke("NewEnemyAi", 3f);

    }
    public void BossAttack_Anger() //10초마다
    {
        if (IsBossSkill_Anger)
        {
            anim.SetTrigger("Boss_Skill_Anger");

        }
        else if(IsBossSkill_Shine)
        {
            BossAttack_Shine();
        }
        else
        {
            BossAttack_Tear();
        }
        
    }
    public void BossAnger() //소환
    {
        IsBossSkill_Anger = false;
        StartCoroutine(BossAttack_AngerColl(25f));

        //int random = Random.Range(0, 3);
        int randomxy = Random.Range(0, 2);
        int offsetx = 0;

        if (randomxy == 0)
        {
            offsetx=10;
        }
        else
        {
            offsetx = -10;
        }
        
        switch (bossskil3count)
        {
      
            case 0:
               GameObject tmp1 =  Instantiate(Enemy_Horse,new Vector3(Camera.main.transform.position.x + offsetx, 0,0), Quaternion.identity);
                tmp1.transform.parent = enemygroup.transform;
                bossskil3count++;
                break;
            case 1:
                GameObject tmp2= Instantiate(Enemy_Rose, new Vector3(Camera.main.transform.position.x + offsetx, 0, 0), Quaternion.identity);
                tmp2.transform.parent = enemygroup.transform;
                bossskil3count++;
                break;
            case 2:
                GameObject tmp3=Instantiate(Enemy_Big, new Vector3(Camera.main.transform.position.x + offsetx, 0, 0), Quaternion.identity);
                tmp3.transform.parent = enemygroup.transform;
                bossskil3count = 0;
                break;
        }

    }

    IEnumerator BossAttack_AngerColl(float cool)
    {
            yield return new WaitForSeconds(cool);
            IsBossSkill_Anger = true;
     
    }
    IEnumerator BossAttack_ShineColl(float cool)
    {
        yield return new WaitForSeconds(cool);
        IsBossSkill_Shine = true;
    }
    
    public void IsBossSkillFalse()
    {
        anim.ResetTrigger("Enemy_Attacked");
        anim.SetBool("IsBossSkill", false);
    }
    public void CheckHp()
    {
        if (currHp / initHp * 100 > 60)
        {
            BossState = 1;
        }
        else if (currHp / initHp * 100 >= 40)
        {
            BossState = 2;
        }
        else
        {
            BossState = 3;
        }
    }
 
    private void OnBecameVisible()
    {
        Invoke("NewEnemyAi", 2f);
        
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.right * findRange, Color.yellow); //플레이어 발견 범위
        if (EnemyMove)
        {
            anim.SetBool("Enemy_Move", true);
            EnemyMove = false;
        }
        Vector3 screenPoint = MainCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);//화면안에 있낫
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

        //anim.SetTrigger("Enemy_Attacked");
        CheckHp();
        anim.SetBool("Enemy_Move", false);
        StartCoroutine(Damage());

        if (currHp <= 0)
        {
            //사망
            transform.GetComponent<BoxCollider2D>().enabled = false;
            r2d.isKinematic = true;
            anim.SetBool("IsBossSkill", true);
            anim.SetTrigger("Enemy_Die");
            CancelInvoke();
            StartCoroutine(EnemyDeath());
        }
    }

    IEnumerator EnemyDeath()
    {
        PlayerData.Instance.chapterCount = 2;
        PlayerData.Instance.SaveData();
        PlayerData.Instance.chapter1count= 9;


        gameObject.transform.parent = enemygroup.transform.parent.parent;
        enemygroup.SetActive(false);
        yield return new WaitForSecondsRealtime(5f);
        StartCoroutine(chater1story.GetComponent<chapter1stroy>().Chapter1_ending());
        gameObject.SetActive(false);

    }
    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1f);
    }

    void NewEnemyAi()
    {

        anim.SetBool("Enemy_Move", false);
        if (Attack1()) //공격 사정거리에 있느가? 있으면 공격
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
        Invoke("NewEnemyAi", 6f);
    }

    IEnumerator EnemyMoveToards(Collider2D vec)
    {

        while (true)
        {

            transform.position = Vector2.MoveTowards(transform.position, vec.transform.position, Time.deltaTime * EnemySpeed);
            anim.SetBool("Enemy_Move", true);

            if (Attack1())
            {
                Debug.Log("공격 사거리 안에 들어옴");
                anim.SetBool("Enemy_Move", false);
                anim.SetBool("IsSkill2Attack", false);

                //Invoke("NewEnemyAi", 5f); //5초마다 계속 ai 재생
                break;
            }
            yield return null;

        }


    }


    void Update()
    {
        EnemyHpBar.GetComponent<Image>().fillAmount = (currHp / initHp);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sim")
        {
            TakeDamage(20);
            if (currHp <= 0)
            {

            }
            else
            {
                anim.SetTrigger("Enemy_Attacked");
                anim.SetBool("IsSkill2Attack", true);
                CancelInvoke();
                StopAllCoroutines();
                Invoke("NewEnemyAi", 2.2f);
            }

        }
        else if (collision.tag == "pen")
        {
            TakeDamage(20);

        }

    }
    public void KockTakeDamage(float hit, Vector2 Pos)
    {
        currHp -= hit;
        float offsetX = Random.Range(-0.5f, 2f);
        float offsetY = Random.Range(0.5f, 2f);
        GameObject hitDamage = Instantiate(DamageTxt, new Vector3(gameObject.transform.position.x + offsetX, gameObject.transform.position.y + 1 + offsetY, gameObject.transform.position.z), Quaternion.identity);
        hitDamage.GetComponent<HitText>().hit = (int)hit;
        anim.SetTrigger("Enemy_Attacked");
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
}
