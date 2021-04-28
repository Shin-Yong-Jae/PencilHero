using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyControl_Horse : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;
    public Transform Pos;
    public Transform Pos2;
    public Vector2 BoxSize;
    Animator anim;
    GameObject AttackCollider;

    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        AttackCollider = transform.GetChild(4).gameObject;
        anim.SetBool("Enemy_Move", false);
    }
   public bool AiAttack()
    {
        //왼쪽 공격
        //r2d.velocity.Normalize();
        //gameObject.GetComponent<Rigidbody2D>().velocity.no        
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

                //collider.gameObject.GetComponent<Playercontrol>().TakeDamage(1, transform.position);
                r2d.velocity = Vector2.zero;
                AttackCollider.SetActive(true);
                r2d.AddForce(Vector2.left * 800f);
                spriteRenderer.flipX = false;
                anim.SetTrigger("Enemy_Attack");

                return true;
            }

        }
        //오른쪽 공격
        collider2Ds = Physics2D.OverlapBoxAll(Pos2.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

                //collider.gameObject.GetComponent<Playercontrol>().TakeDamage(1, transform.position);
                //r2d.velocity = 0;
                r2d.AddForce(Vector2.right * 800f);
                anim.SetTrigger("Enemy_Attack");
                spriteRenderer.flipX = true;
                AttackCollider.SetActive(true);
                return true;
            }
        }

        return false; // 범위안에 플레이어 없음
    }

    void AiAttackT()
    {
        //왼쪽 공격
        r2d.velocity = Vector2.zero;
        

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(30, transform.position);
                anim.SetTrigger("Enemy_Attack");
            }

        }
        //오른쪽 공격
        collider2Ds = Physics2D.OverlapBoxAll(Pos2.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(30, transform.position);
                anim.SetTrigger("Enemy_Attack");

            }
        }
    }

    public void NotAttack()
    {
        AttackCollider.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(Pos.position, BoxSize);
    }
}
