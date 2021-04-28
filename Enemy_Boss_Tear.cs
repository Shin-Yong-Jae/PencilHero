using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss_Tear : MonoBehaviour
{
    public Queue<GameObject> Tear = new Queue<GameObject>();

    public GameObject SampleTear;
    public int Tearcount;
    void Start()
    {
        CreatedTear();
    }
    public void CreatedTear()
    {
        for (int i = 0; i < Tearcount; i++)
        {
            GameObject tear = Instantiate(SampleTear);
            tear.transform.position = new Vector3(-100, 0, 0);
            tear.SetActive(false);
            Tear.Enqueue(tear);
            //Tear.Add(tear);
        }
    }
    void StartSkill()
    {
        StartCoroutine(TearMove());
    }
    void EndSkill()
    {
        StopCoroutine(TearMove());
    }
    IEnumerator TearMove()
    {
        for (int i = 1; i <= Tearcount; i++)
        {

            yield return new WaitForSecondsRealtime(0.5f);
            GameObject tear = Tear.Dequeue(); //Tear[0];
            //Tear.RemoveAt(0);
            
            float offsetX = Random.Range(-10f, 10f);
            tear.transform.position = new Vector3(Camera.main.transform.position.x + offsetX, 6.5f, 0);
            tear.SetActive(true);

            yield return null;

        }
    }
}
