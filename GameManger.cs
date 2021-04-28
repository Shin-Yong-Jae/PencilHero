using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.EventSystems;

public class GameManger : SingleTon<GameManger>
{

    [SerializeField]
    private GameObject chapter1;
    [SerializeField]
    private GameObject chapter2;
    [SerializeField]
    private GameObject chapter3;

    private int pointerID;
    public GameObject joystick;
    public bool isCutScene = false;
    public int textcount = 0;
    public bool TimeLineing = false;
    public bool StoryOn = false;
    public bool ending = false;
    public GameObject player_bubble;

    [Header("Ch2_Bubble")]
    public GameObject enemy_bubble;
    public GameObject salmon_bubble;
    public GameObject shrimp_bubble;
    public GameObject Tamago_bubble;
    public GameObject Salmonegg_bubble;
    public GameObject Tofu_bubble;
    public GameObject boss_bubble;
    public GameObject SushiKing_bubble;

    [Header("Ch3_Bubble")]
    public GameObject MonaKing_bubble;
    public GameObject Knight_bubble;
    public GameObject Archer_bubble;
    public GameObject MonaKing2_bubble;
    public GameObject MonaKing3_bubble;
    public GameObject MonaKing4_bubble;
    public GameObject ending1_bubble;

    [Header("Ch2_View_Box")]
    public GameObject Ch2_View1;
    public GameObject Ch2_View2;
    public GameObject Ch2_View3;
    public GameObject Ch2_View4;
    public GameObject Ch2_View5;
    public GameObject Ch2_View6;
    public GameObject Ch2_View7;
    public GameObject Ch2_View8;

    [Header("Ch3_View_Box")]
    public GameObject Ch3_View1;
    public GameObject Ch3_View2;
    public GameObject Ch3_View3;
    public GameObject Ch3_View4;
    public GameObject Ch3_View5;
    public GameObject Ch3_F1Enemy;
    public GameObject Ch3_F2Enemy;
    public GameObject Ch3_F3Enemy;

    public GameObject Player;
    public Sprite PlayerIdle;
    public GameObject Boss;
    public GameObject Ch2_TextBox;
    public GameObject Ch3_TextBox;
    public bool boss_die = false;
    public GameObject Ch3_Knight;
    public GameObject Ch3_Big;

    [Header("UI_Control")]
    public GameObject SubUI;
    public GameObject JoystickUI;
    public GameObject AttackbtUI;
    public GameObject PlayeredgeUI;
    public GameObject HPUI;
    public GameObject SkillUI;
    public GameObject PlayerUI;
    public GameObject StartbtUI;
    public GameObject PausebtUI;
    public GameObject SkipbtUI;
    public GameObject EditUI;
    public GameObject CreditUI;

    [Header("TimeLine")]
    public GameObject Ch2_TimeLine1;
    public GameObject Ch3_TimeLine1;
    public GameObject Ch3_TimeLine2;
    public GameObject Ch3_TimeLine3;
    public GameObject Ch3_TimeLine4;
    public GameObject Ch3_TimeLine5;

    public int ChapterLevel;
    private bool enemystate_stern;
    public GameObject chapter1stroy;

    void Awake()
    {
        ChapterLevel = MainTitleManager.instance.TitleLevel;

        switch (ChapterLevel)
        {
            case 1: chapter1.SetActive(true); chapter2.SetActive(false); chapter3.SetActive(false); break;
            case 2: chapter1.SetActive(false); chapter2.SetActive(true); chapter3.SetActive(false); break;
            case 3: chapter1.SetActive(false); chapter2.SetActive(false); chapter3.SetActive(true); break;
        }
#if UNITY_EDITOR
        pointerID = -1; //PC나 유니티 상에서는 -1
#elif UNITY_IOS || UNITY_IPHONE
        pointerID = 0;  // 휴대폰이나 이외에서 터치 상에서는 0 
#endif
    }

    private void Start()
    {
        StartCutScene();
        StartSet();

    }
    private void Update()
    {
        #region Ch2
        //Ch2_StotyMode
        if (ChapterLevel == 2)
        {
            if (Player.GetComponent<Warp>().CheckPage != 6 && isCutScene == false && StoryOn == false && Player.GetComponent<Warp>().CheckPage != 0) //Ch2&&필드2
            {
                StartCoroutine(NextCutScene());
                textcount = 0; //텍스트 초기화.
                StoryOn = true;
            }
            if (isCutScene == true && TimeLineing == false && Player.GetComponent<Warp>().CheckPage == 0) //Ch2 && 컷신스타트 && 타임라인 실행 중 아닐때&&필드1
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false) //클릭 시 UI제외
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 16) //ch2 필드 1 종료시점.
                    {
                        Ch2_View1.SetActive(false);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    //Ch2 필드 1 플레이어 대사.
                    else if (textcount == 1 | textcount == 2 | textcount == 5 | textcount == 6 | textcount == 9 | textcount == 10 | textcount == 11)
                    {
                        player_bubble.SetActive(true);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch2_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }
                    //ch2 필드 2 보스 대사.
                    else
                    {
                        player_bubble.SetActive(false);
                        enemy_bubble.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 2).gameObject.SetActive(false);

                        if (textcount == 12 && Ch2_TimeLine1.activeSelf == false) //첫타임라인 재생.
                        {
                            Ch2_TimeLine1.SetActive(true);
                            TimeLineing = true;
                            SkipbtUI.SetActive(false);
                        }

                        if (textcount == 13) //첫 타임라인 종료.
                        {
                            Ch2_TimeLine1.SetActive(false);
                            SkipbtUI.SetActive(true);
                        }

                        if (textcount == 15) //적 보여주기.
                        {
                            Ch2_View1.SetActive(true);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch2_TextBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        enemy_bubble.SetActive(false);
                        Ch2_View1.SetActive(false);
                        TimeLineing = false;
                    }
                }
            }
            if (Player.GetComponent<Warp>().CheckPage == 1 && isCutScene == true && TimeLineing == false)
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 6)//ch2_Field2 종료시점
                    {
                        Ch2_View2.SetActive(false);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    //Ch2 필드 2 플레이어 대사.
                    else if (textcount == 5)
                    {
                        player_bubble.SetActive(true);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else if (textcount == 3) //연어 대사
                    {
                        salmon_bubble.SetActive(true);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else
                    {
                        enemy_bubble.SetActive(true);
                        if (textcount == 4)
                        {
                            salmon_bubble.SetActive(false);
                        }
                        else
                        {
                            player_bubble.SetActive(false);
                        }
                        Ch2_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch2_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 2).gameObject.SetActive(false);
                        }

                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch2_TextBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        enemy_bubble.SetActive(false);
                        salmon_bubble.SetActive(false);
                        Ch2_View2.SetActive(false);
                    }
                }
            }
            if (Player.GetComponent<Warp>().CheckPage == 2 && isCutScene == true && TimeLineing == false)
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 7)
                    {
                        Ch2_View3.SetActive(false);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    else if (textcount == 2 || textcount == 5) // 플레이어 대사
                    {
                        player_bubble.SetActive(true);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else if (textcount == 3)//새우대
                    {
                        shrimp_bubble.SetActive(true);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else
                    {
                        enemy_bubble.SetActive(true);
                        if (textcount == 4)
                        {
                            shrimp_bubble.SetActive(false);
                        }
                        else
                        {
                            player_bubble.SetActive(false);
                        }
                        Ch2_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch2_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch2_TextBox.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        enemy_bubble.SetActive(false);
                        shrimp_bubble.SetActive(false);
                        Ch2_View3.SetActive(false);
                    }
                }
            }
            if (Player.GetComponent<Warp>().CheckPage == 3 && isCutScene == true && TimeLineing == false)
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 6)
                    {
                        Ch2_View4.SetActive(false);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    else if (textcount == 5) // 플레이어 대사
                    {
                        player_bubble.SetActive(true);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else if (textcount == 3)//달걀대사.
                    {
                        Tamago_bubble.SetActive(true);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else
                    {
                        enemy_bubble.SetActive(true);
                        if (textcount == 4)
                        {
                            Tamago_bubble.SetActive(false);
                        }
                        else
                        {
                            player_bubble.SetActive(false);
                        }
                        Ch2_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch2_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch2_TextBox.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        enemy_bubble.SetActive(false);
                        Tamago_bubble.SetActive(false);
                        Ch2_View4.SetActive(false);
                    }
                }
            }
            if (Player.GetComponent<Warp>().CheckPage == 4 && isCutScene == true && TimeLineing == false)
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 5)
                    {
                        Ch2_View5.SetActive(false);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    else if (textcount == 4) // 플레이어 대사
                    {
                        player_bubble.SetActive(true);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else if (textcount == 2)//연어알 대사.
                    {
                        Salmonegg_bubble.SetActive(true);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else
                    {
                        enemy_bubble.SetActive(true);
                        if (textcount == 3)
                        {
                            Salmonegg_bubble.SetActive(false);
                        }
                        else
                        {
                            player_bubble.SetActive(false);
                        }
                        Ch2_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch2_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch2_TextBox.gameObject.transform.GetChild(4).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        enemy_bubble.SetActive(false);
                        Salmonegg_bubble.SetActive(false);
                        Ch2_View5.SetActive(false);
                    }
                }
            }
            if (Player.GetComponent<Warp>().CheckPage == 5 && isCutScene == true && TimeLineing == false)
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 6)
                    {
                        Ch2_View6.SetActive(false);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(5).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    else if (textcount == 3 || textcount == 5) // 플레이어 대사
                    {
                        player_bubble.SetActive(true);
                        if (textcount == 3)
                        {
                            Tofu_bubble.SetActive(false);
                        }
                        else
                        {
                            enemy_bubble.SetActive(false);
                        }
                        Ch2_TextBox.gameObject.transform.GetChild(5).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(5).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else if (textcount == 2)//유부 대사.
                    {
                        Tofu_bubble.SetActive(true);
                        enemy_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(5).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(5).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else
                    {
                        enemy_bubble.SetActive(true);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(5).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch2_TextBox.gameObject.transform.GetChild(5).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch2_TextBox.gameObject.transform.GetChild(5).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        enemy_bubble.SetActive(false);
                        Ch2_View6.SetActive(false);
                        Tofu_bubble.SetActive(false);
                    }
                }
            }

            if (Player.GetComponent<Warp>().CheckPage == 7 && isCutScene == true && TimeLineing == false && boss_die == false) //보스 오프닝.
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 9)
                    {
                        Ch2_View7.SetActive(false);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(6).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    else if (textcount == 1 || textcount == 4 || textcount == 8) // 플레이어 대사
                    {
                        player_bubble.SetActive(true);
                        boss_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(6).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch2_TextBox.gameObject.transform.GetChild(6).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }
                    else// 보스 대사.
                    {
                        boss_bubble.SetActive(true);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(6).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(6).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch2_TextBox.gameObject.transform.GetChild(6).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        boss_bubble.SetActive(false);
                        Ch2_View7.SetActive(false);
                        textcount = -1;
                    }
                }
            }

            if (Player.GetComponent<Warp>().CheckPage == 7 && boss_die == true && TimeLineing == false && isCutScene == true) //보스 엔딩.
            {
                Player.GetComponent<SpriteRenderer>().sprite = PlayerIdle;
                Player.transform.position = new Vector2(164f, -0.5f);
                Player.transform.localScale = new Vector3(1, 1, 1);
                Ch2_View7.SetActive(true);
                ending = true;
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 19)
                    {
                        Ch2_View7.SetActive(false);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(7).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                        PlayerData.Instance.chapterCount = 3;
                        PlayerData.Instance.SaveData();
                        Debug.Log("CHAPTERCOUNT : " + PlayerData.Instance.chapterCount);
                        StartCoroutine(GoMain());
                    }
                    else if (textcount == 2 || textcount == 4 || textcount == 10 | textcount == 14 | textcount == 15 | textcount == 18) // 플레이어 대사
                    {
                        if (textcount == 10)
                        {
                            Ch2_View8.SetActive(false);
                        }
                        player_bubble.SetActive(true);
                        SushiKing_bubble.SetActive(false);
                        boss_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(7).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(7).GetChild(textcount - 2).gameObject.SetActive(false);

                    }
                    else if (textcount == 3 | textcount == 5 | textcount == 7 | textcount == 9)//스시킹 대사
                    {
                        if (textcount == 3)
                        {
                            Ch2_View8.SetActive(true);
                        }
                        SushiKing_bubble.SetActive(true);
                        boss_bubble.SetActive(false);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(7).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch2_TextBox.gameObject.transform.GetChild(7).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else// 보스 대사.
                    {
                        boss_bubble.SetActive(true);
                        SushiKing_bubble.SetActive(false);
                        player_bubble.SetActive(false);
                        Ch2_TextBox.gameObject.transform.GetChild(7).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch2_TextBox.gameObject.transform.GetChild(7).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch2_TextBox.gameObject.transform.GetChild(7).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        SushiKing_bubble.SetActive(false);
                        boss_bubble.SetActive(false);
                        Ch2_View7.SetActive(false);
                        PlayerData.Instance.chapterCount = 3;
                        PlayerData.Instance.SaveData();
                        Debug.Log("CHAPTERCOUNT : " + PlayerData.Instance.chapterCount);
                        StartCoroutine(GoMain());
                    }
                }
            }
        }

        #endregion
        #region Ch3
        //Ch3_StoryMode
        if (ChapterLevel == 3)
        {
            //스토리 모드 진행 되는 구간이 아니고 컷신이 false라면.
            if (Player.GetComponent<Warp>().CheckPage != 3 && isCutScene == false && StoryOn == false && Player.GetComponent<Warp>().CheckPage != 0
                && Player.GetComponent<Warp>().CheckPage != 4 && Player.GetComponent<Warp>().CheckPage != 5 && Player.GetComponent<Warp>().CheckPage != 6)
            {
                StartCoroutine(NextCutScene());
                textcount = 0; //텍스트 초기화.
                StoryOn = true;
            }
            if (isCutScene == true && TimeLineing == false && Player.GetComponent<Warp>().CheckPage == 0) //Ch3 && 컷신스타트 && 타임라인 실행 중 아닐때&&필드1
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 31) //ch3 필드 1 종료시점.
                    {
                        Ch3_F1Enemy.SetActive(true);
                        player_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    //Ch3 필드 1 플레이어 대사.
                    else if (textcount == 1 | textcount == 2 | textcount == 7 | textcount == 8 | textcount == 13 | textcount == 14 | textcount == 15 |
                        textcount == 22 | textcount == 23 | textcount == 25 | textcount == 26 | textcount == 28 | textcount == 29 | textcount == 30)
                    {
                        player_bubble.SetActive(true);
                        MonaKing_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch3_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                        if (textcount == 28)
                        {
                            Knight_bubble.SetActive(false);
                        }
                        if (textcount == 25)
                        {
                            Ch3_Knight.SetActive(true);
                            SkipbtUI.SetActive(true);
                        }
                    }
                    else if (textcount == 24)
                    {
                        Ch3_View1.SetActive(false);
                        Ch3_View2.SetActive(false);
                        player_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 2).gameObject.SetActive(false);
                        Ch3_TimeLine1.SetActive(true);
                        SkipbtUI.SetActive(false);
                        TimeLineing = true;
                    }
                    //ch3 필드 1 Knight 대사.
                    else if (textcount == 27)
                    {
                        player_bubble.SetActive(false);
                        Knight_bubble.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    //ch3 필드 1 보스 대사.
                    else
                    {
                        player_bubble.SetActive(false);
                        MonaKing_bubble.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(0).GetChild(textcount - 2).gameObject.SetActive(false);
                        if (textcount == 16) //적들 소환.
                        {
                            Ch3_View2.SetActive(true);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch3_TextBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        MonaKing_bubble.SetActive(false);
                        Knight_bubble.SetActive(false);
                        Ch3_View1.SetActive(false);
                        Ch3_View2.SetActive(false);
                        Ch3_Knight.SetActive(true);
                        Ch3_F1Enemy.SetActive(true);
                    }
                }
            }
            if (isCutScene == true && TimeLineing == false && Player.GetComponent<Warp>().CheckPage == 1) //Ch3 && 컷신스타트 && 타임라인 실행 중 아닐때&&필드2
            {
                Ch3_TimeLine1.SetActive(false);//후에 재사용.
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 7) //ch3 필드 2 종료시점.
                    {
                        player_bubble.SetActive(false);
                        Ch3_F2Enemy.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    //Ch3 필드 2 플레이어 대사.
                    else if (textcount == 1 | textcount == 5 | textcount == 6)
                    {
                        player_bubble.SetActive(true);
                        Archer_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch3_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }

                    //ch3 필드 2 아처 대사.
                    else
                    {
                        player_bubble.SetActive(false);
                        Archer_bubble.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(1).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch3_TextBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        Ch3_F2Enemy.SetActive(true);
                        player_bubble.SetActive(false);
                        Archer_bubble.SetActive(false);
                    }
                }
            }
            if (isCutScene == true && TimeLineing == false && Player.GetComponent<Warp>().CheckPage == 2) //Ch3 && 컷신스타트 && 타임라인 실행 중 아닐때&&필드3
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 7) //ch3 필드 2 종료시점.
                    {
                        player_bubble.SetActive(false);
                        Ch3_F3Enemy.SetActive(true);
                        Ch3_Big.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    //Ch3 필드 2 플레이어 대사.
                    else if (textcount == 2 | textcount == 4 | textcount == 6)
                    {
                        player_bubble.SetActive(true);
                        MonaKing2_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 2).gameObject.SetActive(false);
                        if (textcount == 6)
                        {
                            Ch3_View3.SetActive(false);
                        }
                    }
                    //ch3 필드 2 보스 대사.
                    else
                    {
                        player_bubble.SetActive(false);
                        MonaKing2_bubble.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch3_TextBox.gameObject.transform.GetChild(2).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch3_TextBox.gameObject.transform.GetChild(2).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        MonaKing2_bubble.SetActive(false);
                        Ch3_View3.SetActive(false);
                        Ch3_Big.SetActive(true);
                        Ch3_F3Enemy.SetActive(true);
                    }
                }
            }
            if (isCutScene == true && TimeLineing == false && Player.GetComponent<Warp>().CheckPage == 7 && boss_die == false) //Ch3 && 컷신스타트 && 타임라인 실행 중 아닐때&&필드3
            {
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 10) //ch3 필드 2 종료시점.
                    {
                        MonaKing3_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 2).gameObject.SetActive(false);
                        SkipCutScene();
                    }
                    //Ch3 필드 2 플레이어 대사.
                    else if (textcount == 1 | textcount == 4 | textcount == 7 | textcount == 8)
                    {
                        player_bubble.SetActive(true);
                        MonaKing3_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch3_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                    }
                    //ch3 필드 2 보스 대사.
                    else
                    {
                        player_bubble.SetActive(false);
                        MonaKing3_bubble.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(3).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        Ch3_TextBox.gameObject.transform.GetChild(3).gameObject.SetActive(false);
                        player_bubble.SetActive(false);
                        MonaKing3_bubble.SetActive(false);
                        textcount = -1;
                    }
                }
            }
            if (isCutScene == true && TimeLineing == false && Player.GetComponent<Warp>().CheckPage == 7 && boss_die == true) //Ch3 && 컷신스타트 && 타임라인 실행 중 아닐때&&필드3
            {
                Player.transform.position = new Vector2(164f, -0.5f);
                Player.transform.localScale = new Vector3(1,1,1);
                ending = true;
                if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == false)
                {
                    textcount++; // 텍스트 카운트++
                    if (textcount == 44) //ch3 종료시점.
                    {
                        player_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).gameObject.SetActive(false);
                        SkipCutScene();
                        StartCoroutine(GoMain());
                    }
                    else if (textcount == 19)
                    {
                        player_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                        Ch3_TimeLine1.SetActive(true);
                        SkipbtUI.SetActive(false);
                        Ch3_View4.SetActive(false);
                        TimeLineing = true;
                    }
                    else if (textcount == 20)
                    {
                        //Player.transform.position = new Vector2(170f, -0.5f);
                        SkipbtUI.SetActive(true);
                        Ch3_TimeLine2.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(true);
                        player_bubble.SetActive(true);
                        TimeLineing = true;
                    }
                    else if (textcount == 21)
                    {
                        Ch3_TimeLine2.SetActive(false);
                        Ch3_TimeLine3.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 3).gameObject.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(true);
                        player_bubble.SetActive(true);
                        ending1_bubble.SetActive(false);
                        TimeLineing = true;
                    }
                    else if (textcount == 22)
                    {
                        Ch3_TimeLine3.SetActive(false);
                        Ch3_TimeLine4.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount).gameObject.SetActive(true);
                        player_bubble.SetActive(true);
                        ending1_bubble.SetActive(false);
                        TimeLineing = true;
                    }
                    else if (textcount == 23)
                    {
                        Ch3_TimeLine4.SetActive(false);
                        player_bubble.SetActive(false);
                        ending1_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount).gameObject.SetActive(false);
                        textcount = 24;
                    }
                    else if (textcount == 28)
                    {
                        player_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                        Ch3_View5.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(true);
                    }
                    else if (textcount == 29)
                    {
                        Ch3_View5.SetActive(false);
                        Ch3_View4.SetActive(true);
                        MonaKing4_bubble.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                    }
                    else if (textcount == 40)
                    {
                        MonaKing4_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                        Ch3_TimeLine5.SetActive(true);
                        TimeLineing = true;
                    }
                    //Ch3 필드 2 플레이어 대사.
                    else if (textcount == 2 | textcount == 3 | textcount == 5 | textcount == 6 | textcount == 9 | textcount == 14
                        | textcount == 15 | textcount == 17 | textcount == 18 | textcount == 25 | textcount == 26 | textcount == 27 | textcount == 31 | textcount == 32
                        | textcount == 35 | textcount == 36 | textcount == 38 | textcount == 41 | textcount == 42 | textcount == 43 | textcount == 44)
                    {
                        if (textcount == 41)
                        {
                            Ch3_View4.SetActive(false);
                        }
                        player_bubble.SetActive(true);
                        MonaKing4_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);

                    }
                    //ch3 필드 2 보스 대사.
                    else
                    {
                        player_bubble.SetActive(false);
                        MonaKing4_bubble.SetActive(true);
                        Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 1).gameObject.SetActive(true);
                        if (textcount != 1)
                        {
                            Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(textcount - 2).gameObject.SetActive(false);
                        }
                        else Ch3_View4.SetActive(true);
                    }
                }
                else if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject(pointerID) == true) //UI 클릭시.
                {
                    if (EventSystem.current.currentSelectedGameObject.name == "Skipbt") //UI 중 스킵버튼 누를 시.
                    {
                        player_bubble.SetActive(false);
                        MonaKing4_bubble.SetActive(false);
                        Ch3_View4.SetActive(false);
                        Ch3_TimeLine5.SetActive(false);
                        Ch3_View5.SetActive(false);
                        ending1_bubble.SetActive(false);
                        Ch3_TextBox.gameObject.transform.GetChild(4).gameObject.SetActive(false);
                        StartCoroutine(GoMain());
                    }
                }
            }
        }
        #endregion
        if (boss_die == true && ending == false && textcount != 0)
        {
            StartCoroutine(NextScene());
        }
    }
    public void StartSet()
    {

        if (ChapterLevel == 1)
        {            
            chapter1stroy.GetComponent<chapter1stroy>().IsOpenning();
            //PlayerData.instance.chapter1count = 1;

            /* chapter1stroy.GetComponent<chapter1stroy>().chapter1story[PlayerData.Instance.chapter1count].SetActive(false);
             StartCoroutine(chapter1stroy.GetComponent<chapter1stroy>().chapter1_1());
             */
        }
        else
        {
            //PlayerData.instance.chapter1count = 5;
            chapter1stroy.SetActive(false);

        }
    }
    public void StartCutScene() //컷신 스타트
    {
        
        Time.timeScale = 0f;
        isCutScene = true;
        SubUI.SetActive(false);
        JoystickUI.SetActive(false);
        AttackbtUI.SetActive(false);
        PlayeredgeUI.SetActive(false);
        HPUI.SetActive(false);
        SkillUI.SetActive(false);
        PlayerUI.SetActive(false);
        StartbtUI.SetActive(false);
        PausebtUI.SetActive(false);
        SkipbtUI.SetActive(true);

        if (ChapterLevel == 1)
            chapter1stroy.GetComponent<chapter1stroy>().StartScene();
    }

    public void SkipCutScene()  //컷신 스킵
    {
        if (ChapterLevel == 1)
        {
            if (PlayerData.instance.chapter1count == 0)
            {
                PlayerData.instance.chapter1count++;
                StartCoroutine(chapter1stroy.GetComponent<chapter1stroy>().chapter1_1());
            }
            else if (PlayerData.instance.chapter1count == 9)
            {
                StartCoroutine(GoMain());
                //SceneManager.LoadScene("Main");
            }
            else
            { 
                Debug.Log("playerdata : " + PlayerData.instance.chapter1count);
                Time.timeScale = 1f;
                isCutScene = false;
                EditUI.SetActive(false);
                SubUI.SetActive(true);
                JoystickUI.SetActive(true);
                AttackbtUI.SetActive(true);
                PlayeredgeUI.SetActive(true);
                HPUI.SetActive(true);
                SkillUI.SetActive(true);
                PlayerUI.SetActive(true);
                StartbtUI.SetActive(true);
                PausebtUI.SetActive(true);
                CreditUI.SetActive(false);
                SkipbtUI.SetActive(false);
                chapter1stroy.GetComponent<chapter1stroy>().Fdout.gameObject.SetActive(false);
                chapter1stroy.GetComponent<chapter1stroy>().StopAllCoroutines();


                if (ChapterLevel == 1)
                    chapter1stroy.GetComponent<chapter1stroy>().Skip();

            }
        }
        else
        {
            Time.timeScale = 1f;
            isCutScene = false;
            EditUI.SetActive(false);
            SubUI.SetActive(true);
            JoystickUI.SetActive(true);
            AttackbtUI.SetActive(true);
            PlayeredgeUI.SetActive(true);
            HPUI.SetActive(true);
            SkillUI.SetActive(true);
            PlayerUI.SetActive(true);
            StartbtUI.SetActive(true);
            PausebtUI.SetActive(true);
            CreditUI.SetActive(false);
            SkipbtUI.SetActive(false);
        }
    }

    public bool EnemyState_Stern
    {
        get
        {
            return enemystate_stern;

        }
        set
        {
            enemystate_stern = value;
        }
    }

    IEnumerator NextCutScene()
    {
        joystick.GetComponent<Joystick2>().isTouch = false;// 컷신 전 이동 불가.
        joystick.GetComponent<Joystick2>().IsMove = false;
        yield return new WaitForSeconds(3.5f);
        StartCutScene();
    }
    IEnumerator NextScene()
    {
        textcount = 0;
        Player.GetComponent<Playercontrol>().IdleMode();
        yield return new WaitForSeconds(2f);
        StartCutScene();
    }
    IEnumerator GoMain()
    {
        yield return new WaitForSecondsRealtime(2f);
        PlayerData.instance.chapter1count = 0;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main");
    }

    IEnumerator GoMain2()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main");

    }
}
