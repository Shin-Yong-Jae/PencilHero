using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCtrl : MonoBehaviour
{
    public bool InCamera = false;
    public GameObject MainCamera;
    public Joystick2 player;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = MainCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);//화면안에 있나
        if (screenPoint.x > 0.0f && screenPoint.x < 1.0f && screenPoint.y < 1.0f && screenPoint.y > 0.0f)
        {
            InCamera = true;
        }
        else InCamera = false;

        if(InCamera==true && Time.timeScale ==1&& player.MoveSpeed >0)
        {
            gameObject.SetActive(false);
        }
    }
}
