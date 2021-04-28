using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFunc : MonoBehaviour
{
    public Camera selectedCamera;
    public bool CanWarp;
    public int _chapter = 0;
    public GameObject GM;
    //public bool CanFinish;
    public Enemy[] Enemy;
    public bool MeetBoss = false;


    void Update()
    {
        Enemy = FindObjectsOfType<Enemy>();//적정보 배열로 (중간에 생성될경우도 있을수있으니 Update문에서) Search
        CheckEnemy();



    }
    public void CheckEnemy()//카메라 뷰 안에 적이 있는지 없는지
    {
        if (Enemy.Length == 0)
        {
            CanWarp = true;
        }

        for (int i = 0; i < Enemy.Length; i++)
        {
            Vector3 screenPoint = selectedCamera.WorldToViewportPoint(Enemy[i].transform.position);
            if (screenPoint.x > 0.0f && screenPoint.x < 1.0f && screenPoint.y < 1.0f && screenPoint.y > 0.0f)
            {
                CanWarp = false;
                break;
            }
            else CanWarp = true;
        }
    }
}
