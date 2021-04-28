using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : SingleTon<PlayerData>
{

    public int chapterCount;
    public int isopenning = 0;
    public int chapter1count = 0;
    [SerializeField]
    private GameObject chapter1;
    [SerializeField]
    private GameObject chapter2;
    [SerializeField]
    private GameObject chapter3;

    [SerializeField]
    private Sprite img_chapter1;
    [SerializeField]
    private Sprite img_chapter2;
    [SerializeField]
    private Sprite img_chapter3;
    [SerializeField]
    private Sprite Img_Lock2;
    [SerializeField]
    private Sprite Img_Lock3;

    void Start()
    {
        //PlayerPrefs.SetInt("chapter", 2); //임시로 열기

        RoadData();
        DontDestroyOnLoad(gameObject);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("chapter", chapterCount);       
    }
    
    public void RoadData()
    {
        if (PlayerPrefs.HasKey("chapter"))
        {
            chapterCount = PlayerPrefs.GetInt("chapter");
            switch (chapterCount)
            {
                case 1:
                    chapter1.GetComponent<Image>().sprite = img_chapter1;
                    chapter2.GetComponent<Image>().sprite = Img_Lock2;
                    chapter3.GetComponent<Image>().sprite = Img_Lock3;
                    chapter1.GetComponent<Button>().enabled = true;
                    chapter2.GetComponent<Button>().enabled = false;
                    chapter3.GetComponent<Button>().enabled = false;
                    break;
                case 2:
                    chapter1.GetComponent<Image>().sprite = img_chapter1;
                    chapter2.GetComponent<Image>().sprite = img_chapter2;
                    chapter3.GetComponent<Image>().sprite = Img_Lock3;
                    chapter1.GetComponent<Button>().enabled = true;
                    chapter2.GetComponent<Button>().enabled = true;
                    chapter3.GetComponent<Button>().enabled = false;
                    break;
                case 3:
                    chapter1.GetComponent<Image>().sprite = img_chapter1;
                    chapter2.GetComponent<Image>().sprite = img_chapter2;
                    chapter3.GetComponent<Image>().sprite = img_chapter3;
                    chapter1.GetComponent<Button>().enabled = true;
                    chapter2.GetComponent<Button>().enabled = true;
                    chapter3.GetComponent<Button>().enabled = true;
                    break;
            }

        }
        else
        {
            chapterCount = 1;
            PlayerPrefs.SetInt("chapter", chapterCount);
            Debug.Log("기존 데이터 없을때 초기화");            
        }
    }
    public void boolopenning()
    {
        isopenning = PlayerPrefs.GetInt("isopenning");
        Debug.Log(isopenning);
        if (PlayerPrefs.HasKey("isopenning")) //기존의 값이 있다면 오프닝 씬을 본것
        {            
            isopenning = PlayerPrefs.GetInt("isopenning");
        }
        else 
        {
            isopenning = 1;
            PlayerPrefs.SetInt("isopenning", isopenning);           
        }        
    }
}
