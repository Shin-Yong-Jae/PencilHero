using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl_Rose : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D r2d;
    public Transform Pos;
    public Transform Pos2;
    public Vector2 BoxSize;
    public GameObject GunRange;
    public GameObject GunRange2;
    Animator anim;
    public GameObject Bullet_Rose;
    public bool EnemyMove = false;
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
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(new Vector2(Pos.position.x-3f, Pos.position.y), BoxSize, 0);
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
        collider2Ds = Physics2D.OverlapBoxAll(new Vector2(Pos2.position.x + 3f, Pos.position.y), BoxSize, 0);
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
        return false;
    }

    public void AiAttackT()
    {

        if (spriteRenderer.flipX)
        {
            GameObject Bull = Instantiate(Bullet_Rose, GunRange2.transform.position, Quaternion.identity);
            Bull.GetComponent<Transform>().localScale = new Vector3(-2, 2, 2);
        }
        else
        {
            Instantiate(Bullet_Rose, GunRange.transform.position, Quaternion.identity);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(Pos.position, BoxSize);
    }
}
