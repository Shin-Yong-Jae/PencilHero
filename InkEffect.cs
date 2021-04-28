using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(EffectDestroy());
    }
    IEnumerator EffectDestroy()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
