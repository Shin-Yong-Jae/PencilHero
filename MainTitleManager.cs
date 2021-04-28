using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class MainTitleManager : MonoBehaviour //메인씬에서 챕터별로 씬이동
{
    public static MainTitleManager instance;
    public int TitleLevel;
    public Camera _mainCamera;
    public GameObject maincanvas;
    public GameObject MainController;
    public GameObject OpenningTimeLine;
    public GameObject LoadingCanvas;
    public GameObject LoadingImage;

    public Sprite Ch1_logo;
    public Sprite Ch2_logo;
    public Sprite Ch3_logo;
    public void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Field1()
    {

        if (PlayerPrefs.GetInt("isopenning")==1)
        {
            //OpenningTimeLine.GetComponent<PlayableDirector>().Play();
            MainController.SetActive(false);
            SceneLoader.Instance.LoadScene("Field1");
            TitleLevel = 1;
            LoadingCanvas.SetActive(true);
            LoadingImage.GetComponent<Image>().sprite = Ch1_logo;
        }
        else
        {
            OpenningTimeLine.SetActive(true);
            MainController.SetActive(false);
        }
    }

    public void Field2()
    {
        MainController.SetActive(false);
        SceneLoader.Instance.LoadScene("Field1");
        TitleLevel = 2;
        LoadingCanvas.SetActive(true);
        LoadingImage.GetComponent<Image>().sprite = Ch2_logo;
    }

    public void Field3()
    {
        MainController.SetActive(false);
        SceneLoader.Instance.LoadScene("Field1");
        TitleLevel = 3;
        LoadingCanvas.SetActive(true);
        LoadingImage.GetComponent<Image>().sprite = Ch3_logo;
    }
}
