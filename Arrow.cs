using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject direction;
    public float Arrowspeed;
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ArrowDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, direction.transform.position, Arrowspeed * Time.deltaTime);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" != collision.gameObject.GetComponent<Playercontrol>().skil3invincibility)
        {
            Destroy(gameObject);
            Player.GetComponent<Playercontrol>().TakeDamage(50, Player.transform.position);
        }
    }
    IEnumerator ArrowDestroy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
    
