using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKill2_simControl : MonoBehaviour
{

    Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void StartReverse()
    {
        ani.SetTrigger("sim_re");
    }
    public void End()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
