using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Control : MonoBehaviour
{
    public GameObject Skill2_sim1;
    public GameObject Skill2_sim2;
    public GameObject Skill2_sim3;

    private void Start()
    {
        Skill2_sim1.SetActive(true);
    }

    public void sim1End()
    {
        Skill2_sim2.SetActive(true);
    }

    public void sim2End()
    {
        Skill2_sim3.SetActive(true);
    }

    public void sim3End()
    {
        Skill2_sim1.SetActive(false);
        Skill2_sim2.SetActive(false);
        Skill2_sim3.SetActive(false);
        //역재생
    }



}
