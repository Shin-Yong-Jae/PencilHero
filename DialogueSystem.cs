using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DialogueSystem : MonoBehaviour
{  
    [BoxGroup("속성")]
    [SerializeField]
    private float textspeed;

    [BoxGroup("필드별 스토리")] [ReorderableList] public List<DialogueBundle> test = new List<DialogueBundle>();




}

[System.Serializable]

public class DialogueBundle {
    public List<Dialogue> dialogues = new List<Dialogue>();
}


[System.Serializable]

public class Dialogue
{
    public GameObject target;
    public string[] text;
}
