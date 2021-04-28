using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondAnimator : MonoBehaviour
{

    private double lastTime;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        lastTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {

        float deltaTime = Time.realtimeSinceStartup - (float)lastTime;

        //animator.Simulate(deltaTime, true, false); //last must be false!!

        lastTime = Time.realtimeSinceStartup;
    }

}
