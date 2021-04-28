using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitText : MonoBehaviour
{
    public float MoveSpeed;
    public float AlphaSpeed;
    public int hit;
    TextMeshPro textpro;
    Color alpha;
    public Material material;
    void Start()
    {
        textpro = GetComponent<TextMeshPro>();
        alpha = textpro.color;
        Invoke("DestroyText", 2f);
        textpro.text = hit.ToString();
    }

    void Update()
    {
        transform.Translate(Vector3.up *Time.deltaTime *MoveSpeed);
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * AlphaSpeed);
        textpro.color = alpha;
    }

    void DestroyText() //파괴 -> 나중에 오브젝트풀?
    {
        Destroy(gameObject);
    }
}
