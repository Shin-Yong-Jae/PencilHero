using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShineController : MonoBehaviour
{
    BoxCollider2D Box2d;
    private void Start()
    {
        Box2d = GetComponent<BoxCollider2D>();
    }

    public void ShineJudgetrue()
    {
        Box2d.enabled = true;
    }

    public void ShineJudgefalse()
    {
        Box2d.enabled = false;

    }
}
