using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_RoseControl : MonoBehaviour
{
    public GameObject direction;
    public float Arrowspeed;
    GameObject Player;
    public int damage;
    
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ArrowDestroy());
    }   
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, direction.transform.position, Arrowspeed * Time.deltaTime);
    }
    
    IEnumerator ArrowDestroy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" != collision.gameObject.GetComponent<Playercontrol>().skil3invincibility)
        {
            collision.gameObject.GetComponent<Playercontrol>().TakeDamage(damage, gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    void DestroyGameOnJect()
    {
        Destroy(gameObject);
    }

    public void SalmonEggAttack(bool dir)
    {
        if (dir) //true 왼쪽 방향
        {
            direction.transform.position = new Vector3(-1, 0, 0);
        }
        else
        {
            direction.transform.position = new Vector3(1, 0, 0);
        }
        //  StartCoroutine(BallMove(position));
    }


    public IEnumerator BallMove(Vector3 position)
    {
        float count = 0;
            Vector3 firstpos = gameObject.transform.position;
            while (true)
            {
                count += Time.deltaTime;
                gameObject.transform.position = Vector3.Lerp(firstpos, position, count);
                if (count >= 2)
                {
                    gameObject.transform.position = position;
                    break;
                }
                yield return null;
            }
    }
}
