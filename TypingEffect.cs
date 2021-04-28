using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingEffect : MonoBehaviour
{
    public Text tx1;
    private int textcount = 2;
    public bool FinishText = false;
    public bool OneClick = false;
    private string m_text;

    public GameObject bubble1;
    public GameObject bubble2;
    public GameObject scripttext1;
    public GameObject scripttext2;

    public GameObject Timeline1;
    public GameObject Timeline2;
    public GameObject Timeline3;
    public GameObject Timeline4;
    public GameObject Timeline5;

    public GameObject MainManager;
    public GameObject MainSceneController;
    public GameObject OpenningController;

    #region 텍스트
    private string m_text1= "누군가가 쓰러져 있다." +
        "\n" +
        "\n이상한 빵모자와 머플러" +
        "\n" +
        "\n그리고 연필...";

    private string m_text2= "앗!" +
        "\n" +
        "\n정신을 차린 듯 하다." +
        "\n" +
        "\n심플한 이목구비가" +
        "\n" +
        "\n인상적인 그...";

    private string m_text3 = "무슨 생각을 하고 있을까?" +
        "\n" +
        "\n쓰러져 있게 된 이유?" +
        "\n" +
        "\n앞으로 해야 할 일?";

    private string m_text4 = "그는 지금 배고팠던 것이다." +
        "\n" +
        "\n자신보다 큰 연필에 의지해" +
        "\n" +
        "\n조심스레 몸을 일으킨다.";
    private string m_text5 = "느닷없이 점프를 하는 그!" +
        "\n" +
        "\n잠깐만," +
        "\n" +
        "\n대체 왜 점프를 한 것일까?";
    private string m_text6 = "그는 예측하기" +
        "\n" +
        "\n어려운 인물인 듯 하다." +
        "\n" +
        "\n멍하니 무언가를 바라보는 그...";
    private string m_text7 = "그는 기억을 잃었던 것이다." +
        "\n" +
        "\n그렇다면 그는 어디에서 왔으며" +
        "\n" +
        "\n이제 어디로 " +
        "\n" +
        "\n나아가야 하는 것인가!";
    private string m_text8 = "일단" +
        "\n" +
        "\n그는 달린다!";

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_typing1());
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && OneClick == false)
        {
            OneClick = true;
            StopAllCoroutines();
            FinishText = true;
            if (textcount == 4)
            {
                bubble1.SetActive(true);
                scripttext1.SetActive(true);
            }
            tx1.text = m_text;
        }
        else if(Input.GetMouseButtonDown(0)&& OneClick==true)
        {
            OneClick = false;
            if (textcount != 9)
            {
                StartCoroutine("_typing" + textcount.ToString());
            }
            else //게임시작
            {
                MainSceneController.SetActive(true);
                OpenningController.SetActive(false);
                MainManager.GetComponent<MainTitleManager>().Field1();
            }
        }
        //textcount++;
        //StopAllCoroutines();
        //Debug.Log("클릭");
        //Debug.Log(textcount);
        //StartCoroutine("_typing" + textcount);
    }

    #region 코루틴
    IEnumerator _typing1()
    {
        yield return new WaitForSeconds(0.2f);
        FinishText = false;
        m_text = m_text1;
        for (int i=0;  i<= m_text1.Length; i++)
        {
            tx1.text = m_text1.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            if (i == m_text1.Length) OneClick = true;
        }
    }
    IEnumerator _typing2()
    {
        textcount++;
        Timeline1.SetActive(false);
        Timeline2.SetActive(true);
        m_text = m_text2;
        FinishText = false;
        for (int i = 0; i <= m_text2.Length; i++)
        {
            tx1.text = m_text2.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            if (i == m_text2.Length) OneClick = true;
        }
    }
    IEnumerator _typing3()
    {
        textcount++;
        m_text = m_text3;
        FinishText = false;
        for (int i = 0; i <= m_text3.Length; i++)
        {
            tx1.text = m_text3.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            if (i == m_text3.Length)
            {
                OneClick = true;
                bubble1.SetActive(true);
                scripttext1.SetActive(true);
            }
        }
    }
    IEnumerator _typing4()
    {
        bubble1.SetActive(false);
        scripttext1.SetActive(false);
        Timeline2.SetActive(false);
        Timeline3.SetActive(true);
        textcount++;
        m_text = m_text4;
        FinishText = false;
        for (int i = 0; i <= m_text4.Length; i++)
        {
            tx1.text = m_text4.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            if (i == m_text4.Length) OneClick = true;
        }
    }
    IEnumerator _typing5()
    {
        Timeline3.SetActive(false);
        Timeline4.SetActive(true);
        textcount++;
        m_text = m_text5;
        FinishText = false;
        for (int i = 0; i <= m_text5.Length; i++)
        {
            tx1.text = m_text5.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            if (i == m_text5.Length) OneClick = true;
        }
    }
    IEnumerator _typing6()
    {
        textcount++;
        m_text = m_text6;
        FinishText = false;
        for (int i = 0; i <= m_text6.Length; i++)
        {
            tx1.text = m_text6.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            if (i == m_text6.Length) OneClick = true;
        }
    }
    IEnumerator _typing7()
    {
        bubble2.SetActive(true);
        scripttext2.SetActive(true);
        textcount++;
        m_text = m_text7;
        FinishText = false;
        for (int i = 0; i <= m_text7.Length; i++)
        {
            tx1.text = m_text7.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            if (i == m_text7.Length) OneClick = true;
        }
    }
    IEnumerator _typing8()
    {
        bubble2.SetActive(false);
        scripttext2.SetActive(false);
        Timeline4.SetActive(false);
        Timeline5.SetActive(true);
        textcount++;
        m_text = m_text8;
        FinishText = false;
        for (int i = 0; i <= m_text8.Length; i++)
        {
            tx1.text = m_text8.Substring(0, i);
            yield return new WaitForSeconds(0.15f);
            if (i == m_text8.Length) OneClick = true;
        }
    }
    #endregion
}
