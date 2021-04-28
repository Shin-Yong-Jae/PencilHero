using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public RectTransform Background;
    public RectTransform Joystickpoint;
    SpriteRenderer sprite;
    public GameObject Player;
    public float MoveSpeed;
    Rigidbody2D ri2d;
    public float radius;
    public bool isTouch;
    public static bool IsJump = false;
    public bool IsMove = false;
    public float JumpPos;
    Vector2 MovePosition;
    public bool isSkill = false;

    void Start()
    {
        radius = Background.rect.width / 2;       
        ri2d = Player.GetComponent<Rigidbody2D>();
        sprite = Player.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (isTouch && Player.GetComponent<Warp>().CanWarping == true)
        {
            if (MovePosition.x < 0 && !isSkill)
            {
                Player.transform.localScale = new Vector3(-1, 1, 1);
                Player.GetComponent<Playercontrol>().Pos.position = new Vector3(-4f + Player.transform.position.x, Player.transform.position.y, 0);
            }
            else
            {
                Player.transform.localScale = new Vector3(1, 1, 1);
                Player.GetComponent<Playercontrol>().Pos.position = new Vector3(4f + Player.transform.position.x, Player.transform.position.y, 0);
              
            }
            if(!isSkill && IsMove)
            Player.transform.Translate(MovePosition.x, 0, 0);
        }
    }

    public void Jump()
    {

        if (ri2d.velocity == Vector2.zero)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Playercontrol>().Jump();
            IsMove = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector2 Value = eventData.position - (Vector2)Background.position;
        Value = Vector2.ClampMagnitude(new Vector2(Value.x, 0), radius);
        Joystickpoint.localPosition = Value;


        //float distance = Vector2.Distance(Background.position, Joystickpoint.position);      
        Vector2 Dis = (Joystickpoint.position -Background.position);
        Dis = Dis.normalized;
        
        

        MovePosition = new Vector2(MoveSpeed * Time.deltaTime * Dis.x, MoveSpeed * Time.deltaTime * Dis.y);

        //if (!IsJump)
        IsMove = true;


    }

    public void OnPointerDown(PointerEventData eventData)
    {

        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        Joystickpoint.localPosition = Vector3.zero;
        IsMove = false;
    }

}

