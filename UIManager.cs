using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameObject player;
    GameObject TitleManager;
    GameObject Scope;
    private bool pauseOn = false;
    public bool mainsoundOn = false;
    public bool subsoundOn = false;
    public bool vibrationOn = false;
    private bool editBtn = false;
    public GameObject GM;

    [Header("UI Tool")]
    public GameObject PauseBtn;
    public GameObject PauseImg;
    public GameObject PauseImg2;
    public GameObject SubUI;
    public GameObject AttackBtn;
    public GameObject EditUI;
    public GameObject MainSoundBtn;
    public GameObject SubSoundBtn;
    public GameObject VibrationBtn;
    public GameObject JoistickBtn;
    public GameObject HeroAttackBtn;
    public GameObject HeroAlphaAttackBtn;
    public GameObject Credit;

    void Start()
    {
        player = GameObject.Find("Player");
        TitleManager = GameObject.Find("TitleManger");
        Scope = GameObject.Find("scope");
        mainsoundOn = false;
        subsoundOn = false;
        vibrationOn = false;
        editBtn = false;
        pauseOn = false;
    }

    public void ActivePauseBt()//pause기능
    {
        if (pauseOn == false)
        {
            if (!player.GetComponent<Playercontrol>().Heromode)
            {
                Time.timeScale = 0;
                PauseBtn.SetActive(false);
                SubUI.SetActive(false);
                AttackBtn.SetActive(false);
                JoistickBtn.SetActive(false);
                Vector2 direction = (Scope.transform.position - player.transform.position);
                if (direction.x >= 0)
                {
                    PauseImg.transform.position = Camera.main.WorldToScreenPoint(player.transform.position+new Vector3(2.2f,2.7f,0));
                    if (GM.GetComponent<GameManger>().isCutScene == false)
                    {
                        PauseImg.SetActive(true);
                    }
                    else pauseOn = true;
                }
                else
                {
                    PauseImg2.transform.position = Camera.main.WorldToScreenPoint(player.transform.position+new Vector3(-2.2f,2.7f,0));
                    if (GM.GetComponent<GameManger>().isCutScene == false)
                    {
                        PauseImg2.SetActive(true);
                    }
                    else pauseOn = true;
                }
            }
            else
            {
                Time.timeScale = 0;
                PauseBtn.SetActive(false);
                HeroAttackBtn.SetActive(false);
                HeroAlphaAttackBtn.SetActive(false);
                JoistickBtn.SetActive(false);
                Vector2 direction = (Scope.transform.position - player.transform.position);
                if (direction.x >= 0)
                {
                    PauseImg.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(2.2f, 2.7f, 0));
                    PauseImg.SetActive(true);
                }
                else
                {
                    PauseImg2.transform.position = Camera.main.WorldToScreenPoint(player.transform.position + new Vector3(-2.2f, 2.7f, 0));
                    PauseImg2.SetActive(true);
                }
            }
        }
        else
        {
            if (!player.GetComponent<Playercontrol>().Heromode)
            {
                Time.timeScale = 1.0f;
                editBtn = false;
                SubUI.SetActive(true);
                PauseBtn.SetActive(true);
                AttackBtn.SetActive(true);
                JoistickBtn.SetActive(true);
                EditUI.SetActive(false);
                Credit.SetActive(false);
                PauseImg.SetActive(false);
                PauseImg2.SetActive(false);
            }
            else
            {
                Time.timeScale = 1.0f;
                editBtn = false;
                PauseBtn.SetActive(true);
                HeroAttackBtn.SetActive(true);
                HeroAlphaAttackBtn.SetActive(true);
                JoistickBtn.SetActive(true);
                EditUI.SetActive(false);
                PauseImg.SetActive(false);
                PauseImg2.SetActive(false);
            }
        }
        pauseOn = !pauseOn; //bool값 반전
    }
    public void ActiveEditBt()
    {
        if (editBtn == false)
        {
            if (!pauseOn)
            {
                ActivePauseBt();
            }
            EditUI.SetActive(true);
        }
        else
        {
            EditUI.SetActive(false);
        }
        editBtn = !editBtn;
    }
    public void ActiveEditCloseBt()
    {
        EditUI.SetActive(false);
    }

    public void ActiveMainSoundBt()
    {
        if (mainsoundOn == false)
        {
            MainSoundBtn.SetActive(false);
            player.GetComponent<AudioSource>().Stop();
            player.GetComponent<Playercontrol>().OnMainSound = false;
        }
        else
        {
            MainSoundBtn.SetActive(true);
            player.GetComponent<AudioSource>().Play(); //오디오 재생
            player.GetComponent<Playercontrol>().OnMainSound = true;
        }
        mainsoundOn = !mainsoundOn;
    }

    public void ActiveSubSoundBt()
    {
        if (subsoundOn == false)
        {
            SubSoundBtn.SetActive(false);
            player.GetComponent<Playercontrol>().OnSubSound = false;
        }
        else
        {
            SubSoundBtn.SetActive(true);
            player.GetComponent<Playercontrol>().OnSubSound = true;
        }
        subsoundOn = !subsoundOn;
    }
    public void ActiveVibrationBt()
    {
        if (vibrationOn == false)
        {
            VibrationBtn.SetActive(false);
            player.GetComponent<Playercontrol>().OnVibration = false;
        }
        else
        {
            VibrationBtn.SetActive(true);
            player.GetComponent<Playercontrol>().OnVibration = true;
        }
        vibrationOn = !vibrationOn;
    }
    public void ActiveCredit()
    {
        EditUI.SetActive(false);
        Credit.SetActive(true);
    }
    public void ActiveCreditCloseBt()
    {
        Credit.SetActive(false);
        EditUI.SetActive(true);
    }
    public void GoMainScenebt()
    {
        Time.timeScale = 1.0f;
        PlayerData.Instance.chapter1count = 0;
        SceneManager.LoadScene("Main");
    }
}
