using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacMove : MonoBehaviour
{

    void Start()
    {
        Go();
    }
    private void Go()
    {
        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);//2d 좌우 반전 위해
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(3.5f, -3.9f, 0),
            "time", 3, "easeType", iTween.EaseType.linear,"oncomplete", "Comeback"));
    }
    private void Comeback()
    {
        transform.localScale = new Vector3(-0.6f, 0.6f, 0.6f);//2d 좌우 반전 위해
        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(-0.5f, -3.9f, 0),
            "time", 3, "easeType", iTween.EaseType.linear,"oncomplete" ,"Go"));
    }
}
