using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Text StartText;
    void Awake()
    {
        Screen.SetResolution(1600, 900, false);
    }
    void Start()
    {
        StartCoroutine(StartTextCon());
        //PlayerPrefs.DeleteAll();
    }
    static void FirstLoad()
    {

    }

    IEnumerator StartTextCon()
    {
        while (true)
        {
            StartText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            StartText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }  
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
   
}
