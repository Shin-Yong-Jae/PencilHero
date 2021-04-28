using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingBubble : MonoBehaviour
{
    public GameObject EnemyBubble;
    public GameObject GM;

    public void Onbubble1()
    {
        EnemyBubble.SetActive(true);
        GM.GetComponent<GameManger>().Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(GM.GetComponent<GameManger>().textcount - 1).gameObject.SetActive(true);
        GM.GetComponent<GameManger>().TimeLineing = false;
    }

    public void Onbubble2()
    {
        EnemyBubble.SetActive(true);
        GM.GetComponent<GameManger>().Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(GM.GetComponent<GameManger>().textcount - 0).gameObject.SetActive(true);
        GM.GetComponent<GameManger>().TimeLineing = false;
    }

    public void Onbubble3()
    {
        EnemyBubble.SetActive(true);
        GM.GetComponent<GameManger>().Ch3_TextBox.gameObject.transform.GetChild(4).GetChild(GM.GetComponent<GameManger>().textcount +1).gameObject.SetActive(true);
        GM.GetComponent<GameManger>().TimeLineing = false;
    }
    public void FalseTimeLine()
    {
        GM.GetComponent<GameManger>().TimeLineing = false;
    }

}
