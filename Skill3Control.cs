using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Skill3Control : MonoBehaviour
{
    public GameObject samplePen;
    public int Pencilcount;
    public GameObject Player;
    Animator m_Anim;


    [SerializeField]
    private float pencilattackterm;
    [SerializeField]
    private GameObject leftpencil;
    [SerializeField]
    private GameObject rightpencil;
    [SerializeField]
    private GameObject entirepencil;

    private bool isskill3end = false;
    void Start()
    {
        m_Anim = GetComponent<Animator>();
        /* for (int i=0; i<Pencilcount; i++)
         {
             GameObject pen = Instantiate(samplePen);
             pen.transform.position = new Vector3(-100, 0, 0);
             pen.SetActive(false);
             Pencill.Enqueue(pen);

         }*/
    }

    void StartSkill()
    {
        leftpencil.SetActive(false);
        rightpencil.SetActive(false);
        entirepencil.SetActive(false);
        isskill3end = false;

        leftpencil.transform.position = new Vector3(Camera.main.transform.position.x, leftpencil.transform.position.y, 1);
        rightpencil.transform.position = new Vector3(Camera.main.transform.position.x, rightpencil.transform.position.y, 1);
        entirepencil.transform.position = new Vector3(Camera.main.transform.position.x, entirepencil.transform.position.y, 1);
        StartCoroutine(PencilMove());
        m_Anim.SetBool("IsSkill", true);
        if (Player.GetComponent<Playercontrol>().OnSubSound) Player.GetComponent<Playercontrol>().audi.PlayOneShot
                (Player.GetComponent<Playercontrol>().Skill3_Sound);

    }

    IEnumerator PencilMove()
    {
        while (!isskill3end)
        {
            yield return new WaitUntil(() => !isskill3end);
            StartCoroutine(LeftPencilAttack());
            yield return new WaitForSecondsRealtime(pencilattackterm);            
            StartCoroutine(SetPencil(leftpencil));

            yield return new WaitUntil(() => !isskill3end);
            StartCoroutine(RightPencilAttack());
            yield return new WaitForSecondsRealtime(pencilattackterm);
            StartCoroutine(SetPencil(rightpencil));

            yield return new WaitUntil(() => !isskill3end);
            StartCoroutine(EntirePencilAttack());
            yield return new WaitForSecondsRealtime(pencilattackterm);
            

            StartCoroutine(SetPencil(entirepencil));

        }
    }

    public IEnumerator SetPencil(GameObject pecils)
    {
        for (int i = 0; i < pecils.transform.childCount; i++)
        {
            pecils.transform.GetChild(i).gameObject.SetActive(true);
        }
        yield return null;
    }

    public IEnumerator LeftPencilAttack()
    {
        yield return new WaitUntil(() => !isskill3end);
        leftpencil.SetActive(true);

    }

    public IEnumerator RightPencilAttack()
    {
        yield return new WaitUntil(() => !isskill3end);
        rightpencil.SetActive(true);

    }
    public IEnumerator EntirePencilAttack()
    {
        yield return new WaitUntil(() => !isskill3end);
        entirepencil.SetActive(true);

    }

    IEnumerator EndSkill()
    {
        yield return new WaitForSeconds(1f);
        StopAllCoroutines();
        m_Anim.SetBool("IsSkill", false);
        gameObject.GetComponent<Playercontrol>().IsPlaySomething = false;
        gameObject.GetComponent<Playercontrol>().isskill3doing = false;
        gameObject.GetComponent<Playercontrol>().IsSkillfalse();
        gameObject.GetComponent<Playercontrol>().GetComponent<Rigidbody2D>().isKinematic = false;

        Enemy[] enemy = FindObjectsOfType<Enemy>();
        GameManger.Instance.EnemyState_Stern = false;
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].gameObject.tag != "Enemy_Sushi_Boss")
            {
                if (enemy[i].gameObject.layer == LayerMask.NameToLayer("Boss"))
                {
                    if (enemy[i].gameObject.tag == "Enemy_Sushi_KingSushi")
                    {
                        enemy[i].gameObject.GetComponent<EnemyControl_KingSushi>().anim.SetTrigger("Enemy_Attacked");
                        enemy[i].gameObject.GetComponent<EnemyControl_KingSushi>().anim.SetBool("sk3_IsAttacked", false);
                    }
                    else if (enemy[i].gameObject.tag == "Enemy_Boss")
                    {
                        enemy[i].gameObject.GetComponent<EnemyControl_Boss>().anim.SetTrigger("Enemy_Attacked");
                        enemy[i].gameObject.GetComponent<EnemyControl_Boss>().anim.SetBool("sk3_IsAttacked", false);
                    }
                    else
                    {
                        enemy[i].anim.SetTrigger("Enemy_Attacked");
                        enemy[i].anim.SetBool("sk3_IsAttacked", false);
                    }
                }
                else
                {
                    enemy[i].anim.SetTrigger("Enemy_Attacked");
                    enemy[i].anim.SetBool("sk3_IsAttacked", false);
                }
            }
            else
            {

                if (enemy[i].gameObject.GetComponent<EnemyControl_BossSushi>().fieldMove)
                {

                }
                else
                {
                    enemy[i].anim.SetTrigger("Enemy_Attacked");
                    enemy[i].anim.SetBool("sk3_IsAttacked", false);
                }
            }




        }
        isskill3end = true;
        yield return null;
    }

}
