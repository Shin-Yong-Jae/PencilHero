using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch3Boss_Passive : MonoBehaviour
{
    GameObject Player;
    public BoxCollider2D boxCollider;
    // Start is called before the first frame update
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        StartCoroutine(InkDestroy());
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player.GetComponent<Playercontrol>().TakeDamage(80, Player.transform.position);
            boxCollider.enabled = false;
        }
    }
    IEnumerator InkDestroy()
    { 
        yield return new WaitForSeconds(1f);
        if (gameObject != null) Destroy(gameObject);
    }
}
