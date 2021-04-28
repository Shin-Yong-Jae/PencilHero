using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl_Big : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;
    public Transform Pos;
    public Transform Pos2;
    public Vector2 BoxSize;
    Animator anim;

    void Start()
    {
        r2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetBool("Enemy_Move", false);
    }
    public bool AiAttack()
    {
        //왼쪽 공격
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

               // collider.gameObject.GetComponent<Playercontrol>().TakeDamage(1, transform.position);               
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
               // collider.gameObject.GetComponent<Playercontrol>().TakeDamage(1, transform.position);                
                anim.SetTrigger("Enemy_Attack");
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

                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(20, transform.position);
                //anim.SetTrigger("Enemy_Attack");
            }

        }
        //오른쪽 공격
        collider2Ds = Physics2D.OverlapBoxAll(Pos2.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.tag == "Player")
            {

                collider.gameObject.GetComponent<Playercontrol>().TakeDamage(20, transform.position);
             //   anim.SetTrigger("Enemy_Attack");

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(Pos.position, BoxSize);
    }
}
