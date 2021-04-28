using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class EnemyControl_KingSushi : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;

    public enum BossState { Idle, Jump, Shot, Attacked };
    public BossState _bossState = BossState.Idle;
    public GameObject EnemyHpBar;
    public Animator anim;
    public GameObject DamageTxt;
    public bool OnScreen = false;
    public Transform Pos;
    public Vector2 BoxSize2; //러쉬 충돌 범위

    private GameObject MainCamera;
    private GameObject Player;
    public bool InCamera = false;
    public GameObject[] Ink;
    public GameObject Inkbullet;
    public GameObject JumpEffect;
    public GameObject GM;
    public float Inkspeed;
    private int ShotOrJump = 0;
    Vector3 HpbarScale;

    public bool _isAttacking = false;

    [Header("Status")]
    public float initHp;
    public float currHp;
    public float vy = 0; //점프 중력값 
    public Vector2 BoxSize; // 점프 공격범위


    void Start()
    {
        //EnemyHpBar.transform.parent.gameObject.SetActive(false);
        r2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currHp = initHp;
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Player = GameObject.Find("Player");
        Jump();
        StartCoroutine(Attack(7));
        HpbarScale = EnemyHpBar.transform.localScale;
        StartCoroutine(CountTime(2));
    }
    void Update()
    {
        switch (_bossState)
        {
            case BossState.Idle: _idle(); break;
            case BossState.Attacked: _attacked(); break;
            case BossState.Jump: _jump(); break;
            case BossState.Shot: _shot(); break;
            default: _idle(); break;
        }
        EnemyHpBar.GetComponent<Image>().fillAmount = (currHp / initHp);
        Vector3 screenPoint = MainCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);//화면안에 있나

        if (screenPoint.x > 0.0f && screenPoint.x < 1.0f && screenPoint.y < 1.0f && screenPoint.y > 0.0f)
        {
            InCamera = true;
        }
        else InCamera = false;
    }
    //public void BirthEnemyHpBar()
    //{
    //    EnemyHpBar.transform.parent.gameObject.SetActive(true);
    //}
    public void ShotAttack()
    {
        if (Player.transform.position.x - gameObject.transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            EnemyHpBar.transform.localScale = new Vector3(-HpbarScale.x, HpbarScale.y, HpbarScale.z);
            StartCoroutine(ShotDelay());
        }
        else
        {
            StartCoroutine(ShotDelay());
        }
    }
    void Shot()
    {
        _isAttacking = true;
        if (Player.transform.position.x - gameObject.transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            EnemyHpBar.transform.localScale = new Vector3(-HpbarScale.x, HpbarScale.y, HpbarScale.z);
            StartCoroutine(ShotDelay());
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            EnemyHpBar.transform.localScale = HpbarScale;
            StartCoroutine(ShotDelay());
        }
    }
    void Jump()
    {
        _isAttacking = true;
        JumpEffect.SetActive(false);
        _bossState = BossState.Jump;
        r2d.AddForce(Vector2.up * vy, ForceMode2D.Impulse);
    }
    public void JumpDamage()  // 공격 범위안에 있으면 데미지
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(new Vector2(transform.position.x,0.0f), new Vector2(BoxSize.x, BoxSize.y), 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                Player.GetComponent<Playercontrol>().TakeDamage(300, Player.transform.position);
            }
        }
    }
    public void TouchPlayer()
    {
        //왼쪽 공격
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize2, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {
                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(20, transform.position);
            }
        }
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



        if (_isAttacking == false) 
        StartCoroutine(Damage());
        DeathCheck();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "sim")
        {
            Debug.Log("심에 맞았어");
            anim.SetTrigger("Enemy_Attacked");
            TakeDamage(20);
        }
        else if (collision.tag == "pen")
        {
            TakeDamage(20);
        }
    }
    public void KockTakeDamage(int hit, Vector2 Pos)
    {
        currHp -= hit;
        float offsetX = Random.Range(-0.5f, 2f);
        float offsetY = Random.Range(0.5f, 2f);
        GameObject hitDamage = Instantiate(DamageTxt, new Vector3(gameObject.transform.position.x + offsetX, gameObject.transform.position.y + 1 + offsetY, gameObject.transform.position.z), Quaternion.identity);
        hitDamage.GetComponent<HitText>().hit = hit;
        StartCoroutine(Damage());
        DeathCheck();
    }
    public void DeathCheck()
    {
        if (currHp <= 0)
        {
            StopAllCoroutines();
            anim.SetTrigger("Enemy_Die");
            transform.GetComponent<BoxCollider2D>().enabled = false;
            r2d.isKinematic = true;
            CancelInvoke();
            StartCoroutine(EnemyDeath());
            GM.GetComponent<GameManger>().boss_die = true;
        }
    }
    public void OnJumpEffect()
    {
        JumpEffect.SetActive(true);
        _bossState = BossState.Idle;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(Pos.position, BoxSize2);
    }
    #region 코루틴문 
    IEnumerator Attack(float DelayTime)
    {
        yield return new WaitForSeconds(DelayTime);
        ShotOrJump++;
        if (ShotOrJump % 3 != 0)
        {
            Shot();
        }
        else
        {
            Jump();
        }
        //_bossState = BossState.Idle;
        StartCoroutine(Attack(7));
    }
    IEnumerator CountTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        TouchPlayer();
        StartCoroutine(CountTime(2));
    }
    IEnumerator EnemyDeath()
    {        
        yield return new WaitForSeconds(1f);
        PlayerData.Instance.chapterCount = 3;
        PlayerData.Instance.SaveData();
        Destroy(gameObject);
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1f);
    }
    IEnumerator ShotDelay()
    {
        List<int> randomshot = new List<int> { 1, 2, 3, 4 };
        for (int i = 1; i < 5; i++)
        {
            int a = Random.Range(randomshot.Min(), randomshot.Max());
            if (randomshot.Contains(a))
            {
                randomshot.Remove(a);
                anim.SetTrigger("Enemy_ShotAttack" + a);
                yield return new WaitForSeconds(1.0f);
                GameObject Ins = Instantiate(Inkbullet, Ink[a - 1].transform.position, Quaternion.identity);
                Ins.name = "Ink" + a;
                anim.ResetTrigger("Enemy_ShotAttack" + a);
            }
            else i--;
        }
        _bossState = BossState.Idle;
        _isAttacking = false;
    }
    #endregion
    #region Ink 생성문
    public void OnInk1()
    {
        //Ink[0].SetActive(true);
    }
    public void DeleteInk()
    {
    //    for(int i=0; i<Ink.Length;i++)
    //    {
    //        Ink[i].SetActive(false);
    //    }
    }
    public void OnInk2()
    {
        //Ink[1].SetActive(true);
    }

    public void OnInk3()
    {
        //Ink[2].SetActive(true);
    }

    public void OnInk4()
    {
        //Ink[3].SetActive(true);
    }

    #endregion
    #region 상태함수 
    private void _idle() //기본상태함수
    {
        anim.SetTrigger("Enemy_Idle");
        anim.ResetTrigger("Enemy_Jump");

    }
    private void _attacked()
    {
        anim.ResetTrigger("Enemy_Jump");
        anim.ResetTrigger("Enemy_Idle");
        anim.SetTrigger("Enemy_Attacked");
    }

    private void _jump()
    {
        anim.SetTrigger("Enemy_Jump");
    }
    private void _shot()
    {
        anim.ResetTrigger("Enemy_Jump");
        anim.ResetTrigger("Enemy_Idle");
        anim.ResetTrigger("Enemy_Attacked");
    }
    #endregion
}
