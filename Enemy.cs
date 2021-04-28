using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public Rigidbody2D r2d;
    public Animator anim;
    public GameObject DamageTxt;
    public GameObject EnemyHpBar;
    public bool InCamera = false;
    private GameObject MainCamera;

    public float initHp;
    public float CurHp;
    public bool isQuake = false;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        SetMonster();

        if(gameObject.tag != "Enemy_Sushi_RiceBall")
        anim.SetBool("Enemy_Move", false);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetMonster()
    {
        CurHp = initHp;

    }
    void Update()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Boss")&&!gameObject.CompareTag("Enemy_Boss"))
            EnemyHpBar.GetComponent<Image>().fillAmount = (CurHp / initHp);

        Vector3 screenPoint = MainCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);//화면안에 있나
        if (screenPoint.x > 0.0f && screenPoint.x < 1.0f && screenPoint.y < 1.0f && screenPoint.y > 0.0f)
        {
            InCamera = true;
        }
        else InCamera = false;

    }
    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1f);
    }

    public virtual void TakeDamage(float hit)
    {
        CurHp -= hit;
        StopCoroutine(HitAlphaAnimation());
        StartCoroutine(HitAlphaAnimation());
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

        if (gameObject.tag == "Enemy_Archer") // 아처 피격반응 코드.
        {
            anim.SetTrigger("Enemy_Attacked");
            gameObject.GetComponent<EnemyControl_Archer>().TakeDamaged = true;
        }

        if (gameObject.tag != "Enemy_Sushi_RiceBall")
            anim.SetBool("Enemy_Move", false);

        StartCoroutine(Damage());
        DeathCheck(CurHp);

        if (CurHp <= 0 && gameObject.tag != "Enemy_Sushi_Boss")
        {
            //사망

            anim.SetTrigger("Enemy_Die");
        }
    }


    public void KockTakeDamage(float hit, Vector2 Pos)
    {
        CurHp -= hit;
        float offsetX = Random.Range(-0.5f, 2f);
        float offsetY = Random.Range(0.5f, 2f);
        GameObject hitDamage = Instantiate(DamageTxt, new Vector3(gameObject.transform.position.x + offsetX, gameObject.transform.position.y + 1 + offsetY, gameObject.transform.position.z), Quaternion.identity);
        hitDamage.GetComponent<HitText>().hit = (int)hit;

        anim.SetBool("Enemy_Move", false);
        StartCoroutine(Damage());

        DeathCheck(CurHp);

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

    public void DeathCheck(float currHp)
    {
        if (currHp <= 0 && gameObject.tag != "Enemy_Sushi_Boss")
        {
            StopAllCoroutines();
            anim.SetTrigger("Enemy_Die");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;// 죽으면서 맞는 일없게 콜라이더를 꺼주는 작업 추가.
            //r2d.velocity = Vector3.zero;
            r2d.isKinematic = true;
            
            StartCoroutine(EnemyDeath());
        }
    }

    public virtual IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    private IEnumerator HitAlphaAnimation()
    {
        //현재 적의 색상.
        Color color = spriteRenderer.color;

        //적의 투명도 40퍼센트.
        color.a = 0.5f;
        spriteRenderer.color = color;

        //0.05초 대기
        yield return new WaitForSeconds(0.1f);

        //적의 투명도 100프로 설정.
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
