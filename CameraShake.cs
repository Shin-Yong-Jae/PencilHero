using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //카메라 흔들기
    public float ShakeAmount;
    float ShakeTime;
    Vector3 initialPosition;
    public bool Attacked = false;
    
    // Start is called before the first frame update
    public void VibrateForTime(float time)
    {
        ShakeTime = time;
        //canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //canvas.renderMode = RenderMode.WorldSpace;
    }
    void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        initialPosition = GameObject.FindWithTag("MainCamera").transform.position;//카메라 흔들릴 위치값
        if (ShakeTime > 0)
        {
            Attacked = true;
            transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            if (Attacked&& GameObject.Find("Player").GetComponent<Warp>().CanWarping)//이동할땐 안 흔들리도록 조건 제어
            {
                transform.position = new Vector3(0 + GameObject.Find("Player").GetComponent<Warp>().CheckPage * 25, 0, -10);
                Attacked = false;
            }
            //canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }
}
