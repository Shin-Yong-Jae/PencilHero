using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject[] InkDestination;
    public GameObject Player;

    public float Inkspeed;
    public int a;
    public GameObject InkEffect;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < 4; i++)
        {
            InkDestination[i] = GameObject.Find("InkDestination" + (i + 1));
        }
    }
    void Update()
    {
        if (gameObject.name == "Ink1")
        {
            transform.position = Vector2.MoveTowards(transform.position, InkDestination[0].transform.position, Inkspeed * Time.deltaTime);
            Vector3 vectorToTarget = InkDestination[0].transform.position - transform.position;

            if (vectorToTarget.x < 0)
            {
                float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 180;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Inkspeed);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Inkspeed);
            }
            StartCoroutine(BulletDestroy());
        }

        if (gameObject.name == "Ink2")
        {
            transform.position = Vector2.MoveTowards(transform.position, InkDestination[1].transform.position, Inkspeed * Time.deltaTime);
            Vector3 vectorToTarget = InkDestination[1].transform.position - transform.position;

            if (vectorToTarget.x < 0)
            {
                float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 180;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Inkspeed);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Inkspeed);
            }
            StartCoroutine(BulletDestroy());
        }
        if (gameObject.name == "Ink3")
        {
            transform.position = Vector2.MoveTowards(transform.position, InkDestination[2].transform.position, Inkspeed * Time.deltaTime);
            Vector3 vectorToTarget = InkDestination[2].transform.position - transform.position;

            if (vectorToTarget.x < 0)
            {
                float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 180;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Inkspeed);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Inkspeed);
            }
            StartCoroutine(BulletDestroy());
        }
        if (gameObject.name == "Ink4")
        {
            transform.position = Vector2.MoveTowards(transform.position, InkDestination[3].transform.position, Inkspeed * Time.deltaTime);
            Vector3 vectorToTarget = InkDestination[3].transform.position - transform.position;

            if (vectorToTarget.x < 0)
            {
                float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 180;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Inkspeed);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * Inkspeed);
            }
            StartCoroutine(BulletDestroy());
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Player.transform.position.x - gameObject.transform.position.x < 0)
            {
                GameObject Inkeff = Instantiate(InkEffect, new Vector3(gameObject.transform.position.x - 1.0f, gameObject.transform.position.y - 1.0f, gameObject.transform.position.z), transform.rotation);
            }
            else
            {
                GameObject Inkeff = Instantiate(InkEffect, new Vector3(gameObject.transform.position.x + 1.0f, gameObject.transform.position.y - 1.0f, gameObject.transform.position.z), transform.rotation);
                Inkeff.transform.localScale = new Vector3(-1, 1, 1);
            }
            Destroy(gameObject);
            Player.GetComponent<Playercontrol>().TakeDamage(100, Player.transform.position);
        }
        //else if (collision.tag == "Floor"|| collision.tag == "Warp")
        //{
        //    if (Player.transform.position.x - gameObject.transform.position.x < 0)
        //    {
        //        GameObject Inkeff = Instantiate(InkEffect, new Vector3(gameObject.transform.position.x - 1.0f, gameObject.transform.position.y - 1.0f, gameObject.transform.position.z), transform.rotation);
        //    }
        //    else
        //    {
        //        GameObject Inkeff = Instantiate(InkEffect, new Vector3(gameObject.transform.position.x + 1.0f, gameObject.transform.position.y - 1.0f, gameObject.transform.position.z), transform.rotation);
        //        Inkeff.transform.localScale = new Vector3(-1, 1, 1);
        //    }
        //    Destroy(gameObject);
        //}
    }

    IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(2f);
        if (gameObject != null)  Destroy(gameObject);
    }
}
