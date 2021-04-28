using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class chapter1stroy : MonoBehaviour
{
    [SerializeField]
    private float typingSpeed;
    private int index;

    public float FadeTime = 0.3f; // Fade효과 재생시간
    public Image Fdout;

    public GameObject ch1_player;

    [SerializeField]
    private GameObject playerbuble;
    [SerializeField]
    private Text playerbuble_text;

    [SerializeField]
    private GameObject ch1_horse;
    [SerializeField]
    private Text ch1_horse_buble;
    [SerializeField]
    private GameObject ch1_rose;
    [SerializeField]
    private Text ch1_rose_buble;
    [SerializeField]
    private GameObject ch1_big;
    [SerializeField]
    private Text ch1_big_buble;

    [SerializeField]
    private GameObject ch1_boss;

    private bool isplayertext;
    private bool ismonstertext;
    private bool onclick = false;
    private bool istype = false;
    int playertextcount = 0;
    int monstertextcount = 0;
    private int pointerID;

    [SerializeField]
    private GameObject monsterGroup;
    [ReorderableList] [SerializeField] public List<GameObject> chapter1story = new List<GameObject>();
    [BoxGroup("챕터1_오프닝")] [SerializeField] private string[] playertext;
    [BoxGroup("챕터1_오프닝")] [SerializeField] private string[] monstertext;

    [BoxGroup("챕터1_1")] [SerializeField] private GameObject rose;
    [BoxGroup("챕터1_1")] [SerializeField] private Text rosebuble;
    [BoxGroup("챕터1_1")] [SerializeField] private string[] playertext1_1;
    [BoxGroup("챕터1_1")] [SerializeField] private string[] monstertext1_1;

    [BoxGroup("챕터1_2")] [SerializeField] private GameObject horse;
    [BoxGroup("챕터1_2")] [SerializeField] private Text horsebuble;
    [BoxGroup("챕터1_2")] [SerializeField] private string[] playertext1_2;
    [BoxGroup("챕터1_2")] [SerializeField] private string[] monstertext1_2;

    [BoxGroup("챕터1_3")] [SerializeField] private GameObject big;
    [BoxGroup("챕터1_3")] [SerializeField] private Text bigbuble;
    [BoxGroup("챕터1_3")] [SerializeField] private string[] playertext1_3;
    [BoxGroup("챕터1_3")] [SerializeField] private string[] monstertext1_3;

    [BoxGroup("챕터1_boss")] [SerializeField] private GameObject boss;
    [BoxGroup("챕터1_boss")] [SerializeField] private Text bosbuble;
    [BoxGroup("챕터1_boss")] [SerializeField] private string[] playertext1_boss;
    [BoxGroup("챕터1_boss")] [SerializeField] private string[] monstertext1_boss;








    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private GameObject end_horse;
    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private Text end_horse_buble;
    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private GameObject end_rose;
    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private Text end_rose_buble;
    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private GameObject end_big;
    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private Text end_big_buble;
    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private GameObject end_boss;
    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private Text end_boss_buble;

    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private GameObject end_bossL;
    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private Text end_bossL_buble;

    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private Sprite sprite_die;

    [BoxGroup("챕터1_bossending")]
    [SerializeField]
    private Sprite sprite_talk;

    [BoxGroup("챕터1_bossending")] [SerializeField] private string[] playertext1_bossend;
    [BoxGroup("챕터1_bossending")] [SerializeField] private string[] monstertext1_bossend;



    public GameManger gm;
    public void Start()
    {
#if UNITY_EDITOR
        pointerID = -1; //PC나 유니티 상에서는 -1
#elif UNITY_IOS || UNITY_IPHONE
        pointerID = 0;  // 휴대폰이나 이외에서 터치 상에서는 0 
#endif
    }
    public void Update()
    {

    }

    public void Skip()
    {
        monsterGroup.SetActive(true);
        chapter1story[PlayerData.Instance.chapter1count].SetActive(false);
        playerbuble.SetActive(false);
        //StopAllCoroutines();

    }
    public void IsOpenning()
    {
        StartCoroutine(Openning()); //오프닝 봤는지 값불러와서 확인

    }
    public void StartScene()
    {
        monsterGroup.SetActive(false);
        chapter1story[PlayerData.Instance.chapter1count].SetActive(true);
        //playerbuble.SetActive(true);
    }

    public IEnumerator Fadeout()
    {
        Fdout.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(FadeTime);
        Fdout.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);

    }
    public IEnumerator Openning()
    {

        chapter1story[0].SetActive(true);
        StartCoroutine(playertype(playertextcount));

        for (int i = 1; i < chapter1story.Count; i++)
        {
            chapter1story[i].SetActive(false);
        }
        return null;
    }
    public IEnumerator chapter1_1()
    {
        playertextcount = 0;
        monstertextcount = 0;
        chapter1story[0].SetActive(false);
        chapter1story[1].SetActive(true);
        StartCoroutine(monstertype1_1(monstertextcount));

        yield return null;
    }
    public IEnumerator Chapter1_2()
    {
        playertextcount = 0;
        monstertextcount = 0;
        monsterGroup.SetActive(false);
        chapter1story[0].SetActive(false);
        chapter1story[1].SetActive(false);
        chapter1story[2].SetActive(true);

        StartCoroutine(monstertype1_2(monstertextcount));

        yield return new WaitForSeconds(0.5f);
        gm.StartCutScene();
        yield return null;
    }
    public IEnumerator Chapter1_3()
    {
        playertextcount = 0;
        monstertextcount = 0;
        monsterGroup.SetActive(false);
        chapter1story[0].SetActive(false);
        chapter1story[1].SetActive(false);
        chapter1story[2].SetActive(false);
        chapter1story[3].SetActive(true);

        StartCoroutine(monstertype1_3(monstertextcount));

        yield return new WaitForSeconds(0.5f);
        gm.StartCutScene();
        yield return null;
    }
    public IEnumerator Chapter1_boss()
    {
        playertextcount = 0;
        monstertextcount = 0;
        monsterGroup.SetActive(false);
        chapter1story[0].SetActive(false);
        chapter1story[1].SetActive(false);
        chapter1story[2].SetActive(false);
        chapter1story[3].SetActive(false);
        chapter1story[4].SetActive(false);
        chapter1story[5].SetActive(false);
        chapter1story[6].SetActive(false);
        chapter1story[7].SetActive(true);
        chapter1story[8].SetActive(false);
        chapter1story[9].SetActive(false);


        StartCoroutine(monstertype1_boss(monstertextcount));

        yield return new WaitForSeconds(0.5f);
        gm.StartCutScene();
        yield return null;
    }

    public IEnumerator Chapter1_ending()
    {
        gm.StartCutScene();
        StartCoroutine(Fadeout());

        GameObject[] objByLayers = GameObject.FindGameObjectsWithTag("Skill");

        for (int i = 0; i < objByLayers.Length; i++)
        {
            objByLayers[i].SetActive(false);
        }


        //플레이어 위치 변경
        ch1_player.transform.position = new Vector3(165, ch1_player.transform.position.y, ch1_player.transform.position.z);
        monsterGroup.SetActive(false);
        playertextcount = 0;
        monstertextcount = 0;
        monsterGroup.SetActive(false);
        chapter1story[0].SetActive(false);
        chapter1story[1].SetActive(false);
        chapter1story[2].SetActive(false);
        chapter1story[3].SetActive(false);
        chapter1story[4].SetActive(false);
        chapter1story[5].SetActive(false);
        chapter1story[6].SetActive(false);
        chapter1story[7].SetActive(false);
        chapter1story[8].SetActive(false);
        chapter1story[9].SetActive(true);


        StartCoroutine(playertype1_ending(playertextcount));

        end_boss.transform.parent.parent.GetComponent<SpriteRenderer>().sprite = sprite_die;

        yield return new WaitForSecondsRealtime(0.5f);
        gm.StartCutScene();
        yield return null;
    }

    public IEnumerator Startchapter(int count)
    {
        yield return new WaitForSecondsRealtime(3.3f);

        switch (count)
        {
            case 2:
                StartCoroutine(Chapter1_2());
                yield return new WaitForSecondsRealtime(0.5f);

                break;
            case 3:
                StartCoroutine(Chapter1_3());
                yield return new WaitForSecondsRealtime(0.5f);

                break;
            case 8:
                StartCoroutine(Chapter1_boss());
                yield return new WaitForSeconds(1f);

                break;
            default:
                yield return new WaitForSecondsRealtime(0.2f);
                monsterGroup.SetActive(true);
                break;
        }

        //  monsterGroup.SetActive(true);

        yield return null;
    }

    public IEnumerator playertype(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(true);
        ch1_rose_buble.transform.parent.gameObject.SetActive(false);
        ch1_horse_buble.transform.parent.gameObject.SetActive(false);
        ch1_big_buble.transform.parent.gameObject.SetActive(false);

        if (playertext[count] == "end")
        {
            StartCoroutine(Fadeout());
            StartCoroutine(chapter1_1());
        }
        else if (playertext[count] != "next")
        {
            StartCoroutine(Type(playertext[count], playerbuble_text));
            playertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(playertype(playertextcount));
        }

        else
        {
            playertextcount++;
            StartCoroutine(monstertype(monstertextcount));
        }

    }
    public IEnumerator playertype1_1(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(true);
        rose.transform.parent.gameObject.SetActive(false);


        if (playertext1_1[count] == "end")
        {
            playerbuble.SetActive(false);
            chapter1story[1].gameObject.SetActive(false);
            StartCoroutine(Fadeout());
            PlayerData.Instance.chapter1count = 1;
            gm.SkipCutScene();
            //게임시작
        }
        else if (playertext1_1[count] != "next")
        {
            StartCoroutine(Type(playertext1_1[count], playerbuble_text));
            playertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);

            StartCoroutine(playertype1_1(playertextcount));
        }
        else
        {
            playertextcount++;
            StartCoroutine(monstertype1_1(monstertextcount));
        }

    }
    public IEnumerator playertype1_2(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(true);
        horse.transform.parent.gameObject.SetActive(false);


        if (playertext1_2[count] == "end")
        {
            playerbuble.SetActive(false);
            chapter1story[2].gameObject.SetActive(false);
            StartCoroutine(Fadeout());
            gm.SkipCutScene();
            //게임시작
        }
        else if (playertext1_2[count] != "next")
        {
            StartCoroutine(Type(playertext1_2[count], playerbuble_text));
            playertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(playertype1_2(playertextcount));
        }
        else
        {
            playertextcount++;
            StartCoroutine(monstertype1_2(monstertextcount));
        }

    }

    public IEnumerator playertype1_3(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(true);
        big.transform.parent.gameObject.SetActive(false);


        if (playertext1_3[count] == "end")
        {
            playerbuble.SetActive(false);
            chapter1story[3].gameObject.SetActive(false);
            StartCoroutine(Fadeout());
            gm.SkipCutScene();
            //게임시작
        }
        else if (playertext1_3[count] != "next")
        {
            StartCoroutine(Type(playertext1_3[count], playerbuble_text));
            playertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(playertype1_3(playertextcount));
        }
        else
        {
            playertextcount++;
            StartCoroutine(monstertype1_3(monstertextcount));
        }

    }

    public IEnumerator playertype1_boss(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(true);
        boss.transform.parent.gameObject.SetActive(false);


        if (playertext1_boss[count] == "end")
        {
            playerbuble.SetActive(false);
            chapter1story[4].gameObject.SetActive(false);
            StartCoroutine(Fadeout());
            gm.SkipCutScene();
            //게임시작
        }
        else if (playertext1_boss[count] != "next")
        {
            StartCoroutine(Type(playertext1_boss[count], playerbuble_text));
            playertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(playertype1_boss(playertextcount));
        }
        else
        {
            playertextcount++;
            StartCoroutine(monstertype1_boss(monstertextcount));
        }

    }

    public IEnumerator playertype1_ending(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(true);
        end_rose.transform.parent.parent.gameObject.SetActive(false);
        end_horse.transform.parent.parent.gameObject.SetActive(false);
        end_big.transform.parent.parent.gameObject.SetActive(false);
        end_boss_buble.transform.parent.gameObject.SetActive(false);

        if (playertext1_bossend[count] == "end")
        {
            StartCoroutine(Fadeout());
            //클리어 챕터2 오픈
            Debug.Log("메인으로");
            if (PlayerPrefs.GetInt("chapter") == 1)
            {
                PlayerPrefs.SetInt("chapter", 2);
            }
            StartCoroutine(GoMain());
        }
        else if (playertext1_bossend[count] != "next")
        {
            StartCoroutine(Type(playertext1_bossend[count], playerbuble_text));
            playertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(playertype1_ending(playertextcount));
        }

        else
        {
            playertextcount++;
            StartCoroutine(monstertype1_ending(monstertextcount));
        }

    }

    public IEnumerator monstertype(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(false);
        if (monstertext[count] == "end")
        {
            playerbuble_text.transform.parent.gameObject.SetActive(false);
            ch1_rose_buble.transform.parent.gameObject.SetActive(false);
            ch1_horse_buble.transform.parent.gameObject.SetActive(false);
            ch1_big_buble.transform.parent.gameObject.SetActive(false);

            chapter1story[0].gameObject.SetActive(false);
            //게임시작
        }
        else if (monstertext[count] != "next")
        {
            string tmp = monstertext[count].Substring(0, 6);
            int maxtext = monstertext[count].Length - 1;
            switch (tmp)
            {
                case "ch1_r:":
                    monstertext[count] = monstertext[count].Remove(0, 6);
                    ch1_rose_buble.transform.parent.gameObject.SetActive(true);
                    ch1_horse_buble.transform.parent.gameObject.SetActive(false);
                    ch1_big_buble.transform.parent.gameObject.SetActive(false);
                    StartCoroutine(Type(monstertext[count], ch1_rose_buble));

                    break;
                case "ch1_h:":
                    monstertext[count] = monstertext[count].Remove(0, 6);
                    ch1_horse_buble.transform.parent.gameObject.SetActive(true);
                    ch1_rose_buble.transform.parent.gameObject.SetActive(false);
                    ch1_big_buble.transform.parent.gameObject.SetActive(false);
                    StartCoroutine(Type(monstertext[count], ch1_horse_buble));

                    break;
                case "ch1_b:":
                    monstertext[count] = monstertext[count].Remove(0, 6);
                    ch1_horse_buble.transform.parent.gameObject.SetActive(false);
                    ch1_rose_buble.transform.parent.gameObject.SetActive(false);
                    ch1_big_buble.transform.parent.gameObject.SetActive(true);
                    StartCoroutine(Type(monstertext[count], ch1_big_buble));
                    break;

            }
            monstertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(monstertype(monstertextcount));
        }
        else
        {
            if (monstertext[count] == "end")
            {
                playerbuble_text.transform.parent.gameObject.SetActive(false);
                ch1_rose_buble.transform.parent.gameObject.SetActive(false);
                ch1_horse_buble.transform.parent.gameObject.SetActive(false);
                ch1_big_buble.transform.parent.gameObject.SetActive(false);
                Debug.Log("게임 시작");
                //게임시작
            }
            else
            {
                monstertextcount++;
                StartCoroutine(playertype(playertextcount));
            }

        }
        yield return null;
    }

    public IEnumerator monstertype1_1(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(false);
        rose.transform.parent.gameObject.SetActive(true);
        if (monstertext1_1[count] == "end")
        {
            playerbuble_text.transform.parent.gameObject.SetActive(false);
            rose.transform.parent.gameObject.SetActive(false);

        }
        else if (monstertext1_1[count] != "next")
        {

            StartCoroutine(Type(monstertext1_1[count], rosebuble));

            monstertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(monstertype1_1(monstertextcount));
        }
        else
        {

            monstertextcount++;
            StartCoroutine(playertype1_1(playertextcount));

        }
        yield return null;
    }

    public IEnumerator monstertype1_2(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(false);
        horse.transform.parent.gameObject.SetActive(true);
        if (monstertext1_2[count] == "end")
        {
            playerbuble_text.transform.parent.gameObject.SetActive(false);
            horse.transform.parent.gameObject.SetActive(false);
            //StartCoroutine(Fadeout());
            gm.SkipCutScene();
        }
        else if (monstertext1_2[count] != "next")
        {

            StartCoroutine(Type(monstertext1_2[count], horsebuble));

            monstertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(monstertype1_2(monstertextcount));
        }
        else
        {

            monstertextcount++;
            StartCoroutine(playertype1_2(playertextcount));

        }
        yield return null;
    }
    public IEnumerator monstertype1_3(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(false);
        big.transform.parent.gameObject.SetActive(true);
        if (monstertext1_3[count] == "end")
        {
            playerbuble_text.transform.parent.gameObject.SetActive(false);
            big.transform.parent.gameObject.SetActive(false);
            gm.SkipCutScene();

        }
        else if (monstertext1_3[count] != "next")
        {

            StartCoroutine(Type(monstertext1_3[count], bigbuble));

            monstertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(monstertype1_3(monstertextcount));
        }
        else
        {

            monstertextcount++;
            StartCoroutine(playertype1_3(playertextcount));

        }
        yield return null;
    }
    public IEnumerator monstertype1_boss(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(false);
        boss.transform.parent.gameObject.SetActive(true);

        if (monstertext1_boss[count] == "end")
        {
            playerbuble_text.transform.parent.gameObject.SetActive(false);
            boss.transform.parent.gameObject.SetActive(false);
            chapter1story[4].SetActive(false);

            gm.SkipCutScene();

        }
        else if (monstertext1_boss[count] != "next")
        {
            string tmp = monstertext1_boss[count].Substring(0, 2);
            int maxtext = monstertext1_boss[count].Length - 1;
            switch (tmp) //애니매이션 제어
            {
                case "a:":
                    boss.transform.parent.parent.GetComponent<Animator>().SetTrigger("Boss_Skill_Anger");

                    monstertext1_boss[count] = monstertext1_boss[count].Remove(0, 2);

                    break;
                case "s:":
                    boss.transform.parent.parent.GetComponent<Animator>().SetTrigger("Boss_Skill_Shine");

                    monstertext1_boss[count] = monstertext1_boss[count].Remove(0, 2);


                    break;
                case "t:":
                    boss.transform.parent.parent.GetComponent<Animator>().SetTrigger("Boss_Skill_Tear");

                    monstertext1_boss[count] = monstertext1_boss[count].Remove(0, 2);

                    break;

            }

            StartCoroutine(Type(monstertext1_boss[count], bosbuble));

            monstertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(monstertype1_boss(monstertextcount));
        }
        else
        {

            monstertextcount++;
            StartCoroutine(playertype1_boss(playertextcount));

        }
        yield return null;
    }

    public IEnumerator monstertype1_ending(int count)
    {
        playerbuble_text.transform.parent.gameObject.SetActive(false);
        if (monstertext1_bossend[count] == "end")
        {
            playerbuble_text.transform.parent.gameObject.SetActive(false);
            end_rose_buble.transform.parent.gameObject.SetActive(false);
            end_horse_buble.transform.parent.gameObject.SetActive(false);
            end_big_buble.transform.parent.gameObject.SetActive(false);

        }
        else if (monstertext1_bossend[count] == "all")
        {

            StartCoroutine(Fadeout());
            yield return new WaitForSecondsRealtime(FadeTime);
            end_rose.transform.parent.parent.gameObject.SetActive(true);
            end_horse.transform.parent.parent.gameObject.SetActive(true);
            end_big.transform.parent.parent.gameObject.SetActive(true);
            end_boss.transform.parent.parent.gameObject.SetActive(false);
            ch1_player.SetActive(false);

            monstertextcount++;
            istype = true;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);

            StartCoroutine(monstertype1_ending(monstertextcount));
        }
        else if (monstertext1_bossend[count] == "boss")
        {
            end_boss.transform.parent.parent.GetComponent<SpriteRenderer>().sprite = sprite_talk;

            StartCoroutine(Fadeout());
            end_rose.transform.parent.parent.gameObject.SetActive(false);
            end_horse.transform.parent.parent.gameObject.SetActive(false);
            end_big.transform.parent.parent.gameObject.SetActive(false);
            end_boss.transform.parent.parent.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(FadeTime);
            ch1_player.SetActive(true);
            end_rose.transform.parent.parent.gameObject.SetActive(false);
            end_horse.transform.parent.parent.gameObject.SetActive(false);
            end_big.transform.parent.parent.gameObject.SetActive(false);
            end_boss.transform.parent.parent.gameObject.SetActive(true);

            monstertextcount++;
            istype = true;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(monstertype1_ending(monstertextcount));
        }
        else if (monstertext1_bossend[count] == "together")
        {
            StartCoroutine(Fadeout());
            end_rose.transform.parent.parent.gameObject.SetActive(false);
            end_horse.transform.parent.parent.gameObject.SetActive(false);
            end_big.transform.parent.parent.gameObject.SetActive(false);
            end_boss.transform.parent.parent.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(FadeTime);

            end_rose.transform.parent.parent.gameObject.SetActive(true);
            end_horse.transform.parent.parent.gameObject.SetActive(true);
            end_big.transform.parent.parent.gameObject.SetActive(true);
            end_boss.transform.parent.parent.gameObject.SetActive(false);
            ch1_player.SetActive(false);
            end_bossL.transform.parent.parent.gameObject.SetActive(true);
            istype = true;

            end_rose_buble.transform.parent.gameObject.SetActive(false);
            end_horse_buble.transform.parent.gameObject.SetActive(false);
            end_big_buble.transform.parent.gameObject.SetActive(false);

            monstertextcount++;

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(monstertype1_ending(monstertextcount));
        }
        else if (monstertext1_bossend[count] == "player")
        {

            StartCoroutine(Fadeout());
            end_rose.transform.parent.parent.gameObject.SetActive(false);
            end_horse.transform.parent.parent.gameObject.SetActive(false);
            end_big.transform.parent.parent.gameObject.SetActive(false);
            end_bossL.transform.parent.parent.gameObject.SetActive(false);
            yield return new WaitForSecondsRealtime(FadeTime);

            end_rose.transform.parent.parent.gameObject.SetActive(false);
            end_horse.transform.parent.parent.gameObject.SetActive(false);
            end_big.transform.parent.parent.gameObject.SetActive(false);
            end_bossL.transform.parent.parent.gameObject.SetActive(false);
            end_boss_buble.transform.parent.gameObject.SetActive(false);

            ch1_player.SetActive(true);
            end_boss.transform.parent.parent.gameObject.SetActive(true);

            monstertextcount++;

            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(monstertype1_ending(monstertextcount));
        }
        else if (monstertext1_bossend[count] != "next")
        {
            string tmp = monstertext1_bossend[count].Substring(0, 6);
            int maxtext = monstertext1_bossend[count].Length - 1;
            switch (tmp)
            {


                case "ch1_q:":
                    monstertext1_bossend[count] = monstertext1_bossend[count].Remove(0, 6);
                    /* end_rose_buble.transform.parent.gameObject.SetActive(false);
                     end_horse_buble.transform.parent.gameObject.SetActive(false);
                     end_big_buble.transform.parent.gameObject.SetActive(false);
                     end_bossL.transform.parent.gameObject.SetActive(false);
                     */
                    end_boss_buble.transform.parent.gameObject.SetActive(true);
                    StartCoroutine(Type(monstertext1_bossend[count], end_boss_buble));
                    break;
                case "ch1_r:":
                    monstertext1_bossend[count] = monstertext1_bossend[count].Remove(0, 6);
                    end_bossL_buble.transform.parent.gameObject.SetActive(false);
                    end_rose_buble.transform.parent.gameObject.SetActive(true);
                    end_horse_buble.transform.parent.gameObject.SetActive(false);
                    end_big_buble.transform.parent.gameObject.SetActive(false);
                    StartCoroutine(Type(monstertext1_bossend[count], end_rose_buble));

                    break;
                case "ch1_h:":
                    monstertext1_bossend[count] = monstertext1_bossend[count].Remove(0, 6);
                    end_bossL_buble.transform.parent.gameObject.SetActive(false);
                    end_horse_buble.transform.parent.gameObject.SetActive(true);
                    end_rose_buble.transform.parent.gameObject.SetActive(false);
                    end_big_buble.transform.parent.gameObject.SetActive(false);
                    StartCoroutine(Type(monstertext1_bossend[count], end_horse_buble));

                    break;
                case "ch1_b:":
                    monstertext1_bossend[count] = monstertext1_bossend[count].Remove(0, 6);
                    end_bossL_buble.transform.parent.gameObject.SetActive(false);
                    end_horse_buble.transform.parent.gameObject.SetActive(false);
                    end_rose_buble.transform.parent.gameObject.SetActive(false);
                    end_big_buble.transform.parent.gameObject.SetActive(true);
                    StartCoroutine(Type(monstertext1_bossend[count], end_big_buble));
                    break;

                case "ch1_l:":
                    monstertext1_bossend[count] = monstertext1_bossend[count].Remove(0, 6);
                    end_bossL_buble.transform.parent.gameObject.SetActive(true);
                    end_rose_buble.transform.parent.gameObject.SetActive(false);
                    end_horse_buble.transform.parent.gameObject.SetActive(false);
                    end_big_buble.transform.parent.gameObject.SetActive(false);
                    StartCoroutine(Type(monstertext1_bossend[count], end_bossL_buble));

                    break;

            }
            monstertextcount++;
            istype = false;
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) == true && istype && EventSystem.current.IsPointerOverGameObject(pointerID) == false);
            StartCoroutine(monstertype1_ending(monstertextcount));
        }
        else
        {
            if (monstertext1_bossend[count] == "end")
            {
                playerbuble_text.transform.parent.gameObject.SetActive(false);
                end_rose_buble.transform.parent.gameObject.SetActive(false);
                end_horse_buble.transform.parent.gameObject.SetActive(false);
                end_big_buble.transform.parent.gameObject.SetActive(false);
                Debug.Log("게임 시작");
                //게임시작
            }
            else
            {
                monstertextcount++;
                StartCoroutine(playertype1_ending(playertextcount));
            }

        }
        yield return null;
    }
    IEnumerator GoMain()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main");
    }

    public IEnumerator Type(string text, Text textbox)
    {
        textbox.text = "";
        /*foreach (char letter in text.ToCharArray())
        {
            textbox.text = text;
            yield return new WaitForSeconds(typingSpeed);
        }*/
        yield return new WaitForSecondsRealtime(typingSpeed);

        textbox.text = text;
        istype = true;
        yield return 0;

    }
}
