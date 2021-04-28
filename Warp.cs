using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warp : MonoBehaviour
{
    CameraFunc cam;
    public GameObject player;
    public GameObject MainCamera;
    public Vector3 playertr;
    public Vector3 Cameratr;
    public bool CanWarping = true;
    public int wallcheck;
    public int CheckPage = 0;
    public GameObject Ch2_Boss;

    public GameObject GM;
    public chapter1stroy chapter1Stroy;
    public GameObject monsterGroup;
    private void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<CameraFunc>();
    }
    private void Update()
    {
        playertr = player.transform.position;
        Cameratr = MainCamera.transform.position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Warp") && cam.CanWarp && CanWarping && !player.GetComponent<Playercontrol>().Heromode)//필드 내 적 다 잡았을 시 이동가능 조건
        {
            GameObject.Find("JoystickBackground").GetComponent<Joystick2>().MoveSpeed = 0;
            CanWarping = false;
            StartCoroutine(CanWarp());
            iTween.MoveTo(MainCamera, iTween.Hash("x", Cameratr.x + 12.5f,
                "time", 0.7f, "easeType", iTween.EaseType.linear));
            iTween.MoveTo(gameObject, iTween.Hash("x", playertr.x + 0.1f,
            "time", 0.5f, "easeType", iTween.EaseType.linear, "oncomplete", "BreakWall", "delay", 1.5f));
            CheckPage++;
            PlayerData.Instance.chapter1count++;

            monsterGroup.SetActive(false);
            if (GM.GetComponent<GameManger>().ChapterLevel == 1)
            {
                StartCoroutine(chapter1Stroy.Startchapter(PlayerData.Instance.chapter1count));
            }

            GM.GetComponent<GameManger>().StoryOn = false;

            if (CheckPage == 7 && GM.GetComponent<GameManger>().ChapterLevel == 2)
            {
                StartCoroutine(Ch2BossBirth());
            }
        }
    }
    private void OnWarp()
    {
        CanWarping = true;
        gameObject.GetComponent<Playercontrol>().Dashbtn();
        CanWarping = false;
        StartCoroutine(CanDash());
    }
    private void OnJump()
    {
        gameObject.GetComponent<Playercontrol>().Jump();
    }
    private void BreakWall()
    {
        iTween.MoveTo(gameObject, iTween.Hash("x", playertr.x + 3,
            "time", 0.3f, "easeType", iTween.EaseType.linear, "onstart", "OnWarp"));
        wallcheck++;
        GameObject.Find("BreakWall" + wallcheck).SetActive(false);
        GameObject.Find("wall1-" + wallcheck + "Right").SetActive(false);

        iTween.MoveTo(MainCamera, iTween.Hash("x", Cameratr.x + 12.5f,
            "time", 0.7f, "easeType", iTween.EaseType.linear, "delay", 0.7f));

        if (GM.GetComponent<GameManger>().ChapterLevel == 2)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Enemy_Sushi_Boss");

            if (boss != null)
                StartCoroutine(ResetBoss());
            
        }

        //monsterGroup.SetActive(true);
        //Invoke("NextFeild", 2f);
    }

    public void NextFeild()
    {

    }
    IEnumerator ResetBoss()
    {
        yield return new WaitForSeconds(2f);
        GameObject boss = GameObject.FindGameObjectWithTag("Enemy_Sushi_Boss");
        boss.GetComponent<EnemyControl_BossSushi>().StartNewEnemyAi();
        boss.GetComponent<EnemyControl_BossSushi>().Summons = true;
        boss.GetComponent<EnemyControl_BossSushi>().EnemySpeed = 0.5f;


        boss.GetComponent<BoxCollider2D>().enabled = true;
        boss.GetComponent<Rigidbody2D>().gravityScale = 1;
    }
    IEnumerator CanWarp()
    {
        yield return new WaitForSeconds(3.7f);
        CanWarping = true;
        GameObject.Find("JoystickBackground").GetComponent<Joystick2>().MoveSpeed = 5;
    }
    IEnumerator CanDash()
    {
        yield return new WaitForSeconds(1.5f);
        CanWarping = true;
    }
    IEnumerator Ch2BossBirth()
    {
        yield return new WaitForSeconds(3.0f);
        Ch2_Boss.SetActive(true);
    }
}
