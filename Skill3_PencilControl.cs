using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3_PencilControl : MonoBehaviour
{
    Animator ani;
    BoxCollider2D AttackBox;

    void Start()
    {
        ani = GetComponent<Animator>();
        AttackBox = GetComponent<BoxCollider2D>();
    }

    void Attack()
    {
        AttackBox.enabled = true;
    }

    void NotAttack()
    {
        AttackBox.enabled = false;

    }

    void InQueue()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 15, 0);
        gameObject.SetActive(false);
    }
}