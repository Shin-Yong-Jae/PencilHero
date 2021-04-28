
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;



public class Playercontrol : MonoBehaviour
{
    public Rigidbody2D r2d;
    public float speed = 2;

    public enum PlayerState { Idle, Move, Attack };
    public PlayerState playerState = PlayerState.Idle;
    public Animator m_Anim; // 애니메이터
    public Joystick2 Playertr;
    public GameObject joystick;
    public Image PlayerHpUi;
    public Image PlayerSkillUi;
    public Image PlayerHeroModeAttackbtn;
    public static bool PlayerAction = false;
    public int Attackcount;
    float lastClick = 0;
    float ComboDelay = 0.8f;
    public bool NextAttack;
    public bool isQuake = false;
    public float PlayerHP;
    float MaxHp = 6000;
    public int power;
    public GameObject[] Wall;
    public GameObject Targetwall;
    public GameObject[] WhiteLine;
    public bool OnHeroAttack = false;
    public float KnockCoolTime = 0.0f;
    public GameObject gameoverobj;
    public bool isskill3doing = false;

    [Header("Skill")]
    public bool DashCoolDone = true;
    public float DashCoolTime = 1.0f;
    public Image img_Dash;
    public bool Skill1CoolDone = true;
    public float Skill1CoolTime = 10.0f;
    public Image img_Skill1;
    public bool Skill2CoolDone = true;
    public float Skill2CoolTime = 15.0f;
    public Image img_Skill2;
    public GameObject Skill2;
    public GameObject Skill2Range;
    public GameObject Skill2_sim1;
    public GameObject Skill2_sim2;
    public GameObject Skill2_sim3;
    public bool Skill3CoolDone = true;
    public float Skill3CoolTime = 30.0f;
    public Image img_Skill3;
    public bool isSkill2 = false; //스킬2 모션중 대쉬, 다른스킬 사용 불가
    public bool IsPlaySomething = false;
    public bool Heromode = false;
    public bool isjump = false;
    public Enemy enemycount;

    [SerializeField]
    private GameObject skill2_Lock;
    [SerializeField]
    private GameObject skill3_Lock;


    [SerializeField]
    private GameObject skill3TimeLine;
    [SerializeField]
    private Enemy[] enemy;
    //카메라 흔들기
    CameraShake Camera;//카메라 지정변수
    Camera MainCamera;

    public GameObject GM;
    public AudioSource audi;
    public GameObject Canvas;
    [Header("Sound")]
    public AudioSource audiosource;
    public AudioClip JumpSound;
    public AudioClip DashSound;
    public AudioClip HeroAttackSound;
    public AudioClip TakeDamageSound;
    public AudioClip Skill1_Sound;
    public AudioClip NormalAttackSound;
    public AudioClip WalkingSound;
    public AudioClip bgm1;
    public AudioClip bgm2;
    public AudioClip bgm3;
    public AudioClip Herobgm;
    public AudioClip Skill2_Sound;
    public AudioClip Skill3_Sound;
    public AudioClip BtnSound;
    public bool OnSubSound = true;
    public bool OnVibration = true;
    public bool OnMainSound = true;

    [SerializeField]
    private LayerMask groundLayer;
    private BoxCollider2D boxCollider2D;
    public bool isground = false;
    private Vector3 footPosition;
    public int countdamage=1;
    public bool skil3invincibility = false;
    private void Awake()
    {
        PlayerHP = MaxHp;
        Playertr = Playertr.GetComponent<Joystick2>();
        joystick = GameObject.Find("JoystickBackground");
        r2d = GetComponent<Rigidbody2D>();
        PlayerHpUi = PlayerHpUi.GetComponent<Image>();
        PlayerSkillUi = PlayerSkillUi.GetComponent<Image>();
        PlayerHeroModeAttackbtn = PlayerHeroModeAttackbtn.GetComponent<Image>();
        audi = gameObject.GetComponent<AudioSource>();
        Camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Attackcount = 1;
        NextAttack = false;   
    }

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        enemycount = GetComponent<Enemy>();
        WhiteLine = GameObject.FindGameObjectsWithTag("WhiteLine");
        if (GM.GetComponent<GameManger>().ChapterLevel == 1) audiosource.clip = bgm1; //오디오에 bgm이라는 파일 연결
        else if (GM.GetComponent<GameManger>().ChapterLevel == 2) audiosource.clip = bgm2;
        else if (GM.GetComponent<GameManger>().ChapterLevel == 3) audiosource.clip = bgm3;
        else audiosource.clip = bgm1; // GameScene에서 바로시작할경우 Default bgm은 bgm1
        if(OnMainSound) audiosource.Play(); //오디오 재생
        audiosource.loop = true; //반복 여부
        Skill_Check();
    }

    [Header("공격범위관련")]
    private float curTime;
    public float CoolTime;
    public Transform Pos;
    public Vector2 BoxSize;
    public Vector2 BoxSize2;
    bool IsAttack = false;



    void FixedUpdate()
    {
        Bounds bounds = boxCollider2D.bounds;
        footPosition = new Vector2(bounds.center.x, bounds.min.y);
        isground = Physics2D.OverlapCircle(footPosition, 0.1f, groundLayer);

        if (PlayerSkillUi.fillAmount >= 1.0f && !Heromode) HeroMode();
        else if (PlayerSkillUi.fillAmount <= 0.0f && Heromode) IdleMode();
        if (!Heromode) OnHeroAttack = false;
        PlayerHeroModeAttackbtn.fillAmount = PlayerSkillUi.fillAmount;
        KnockCoolTime -= Time.unscaledDeltaTime;
        //if (Heroattackbtn_down) gameObject.transform.GetChild(0).gameObject.SetActive(true);
        //else gameObject.transform.GetChild(0).gameObject.SetActive(false);

        if (Time.time - lastClick > ComboDelay)
        {
            Attackcount = 1;
            IsAttack = false;

            m_Anim.SetBool("Attack2", false);
            m_Anim.SetBool("Attack3", false);
            m_Anim.SetBool("Attack4", false);
            //m_Anim.SetBool("Attack5", false);
            //m_Anim.SetBool("Attack6", false);
        }

        switch (playerState)
        {
            case PlayerState.Idle: Idle(); break;
            case PlayerState.Move: Move(); break;
            case PlayerState.Attack:break;
            default: Idle(); break;
        }

        if (IsAttack)
        {
            playerState = PlayerState.Attack;
            m_Anim.SetBool("AttackedBool", false);
            joystick.GetComponent<Joystick2>().IsMove = false;
        }
        else if (joystick.GetComponent<Joystick2>().IsMove == true)
        {
            playerState = PlayerState.Move;
            //isjump = false;
        }
        else if (joystick.GetComponent<Joystick2>().IsMove == false)
        {
            playerState = PlayerState.Idle;
            //isjump = false;
        }

        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attacked") || m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attackeffect1")) //공격중이거나 공격받았을때는 못움직이게 해준다.
        {
        }

        if (m_Anim.GetCurrentAnimatorStateInfo(0).IsName("jump")) //공격중이거나 공격받았을때는 못움직이게 해준다.
        {
            Joystick2.IsJump = true;
        }
        else
        {
            Joystick2.IsJump = false;

        }
        curTime -= Time.unscaledDeltaTime;



    }
    #region 공격, 스킬 데미지, 넉백
    public void IsNextAttackT() //연속공격 함수
    {
        NextAttack = true;
    }
    public void IsNextAttackF() //연속공격 함수
    {
        NextAttack = false;
    }
    public void AttackSound()
    {
        if (OnSubSound) audi.PlayOneShot(NormalAttackSound);
    }
    public void AttackBtn() //공격 함수
    {
        if (gameObject.GetComponent<Warp>().CanWarping == true&&IsPlaySomething==false)
        {
            IsAttack = true;

            if (curTime <= 0)
            {
                ComboAttack();
                curTime = CoolTime;
            }
            if (NextAttack)
            {
                Attackcount++;


                if (Attackcount == 2)
                {                    
                    m_Anim.SetBool("Attack2", true);
                }
                else if (Attackcount == 3)
                {                 
                    m_Anim.SetBool("Attack3", true);
                    Attackcount = 1;
                }
                else if (Attackcount == 4)
                {
                    m_Anim.SetBool("Attack4", true);
                }
                else if (Attackcount == 5)
                {
                    m_Anim.SetBool("Attack5", true);
                }
                else if (Attackcount == 6)
                {
                    m_Anim.SetBool("Attack6", true);
                    Attackcount = 1;
                }
                else
                {

                }

                NextAttack = false;

            }
        }
    }
    public void ResetPlayer()
    {
        Attackcount = 1;
        skil3invincibility = false;
        m_Anim.SetBool("IsSkill", false);

    }
    public void OnHeroModeAttackbtn()
    {
        //추가할 예정
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        OnHeroAttack = true;
        if (OnHeroAttack && Heromode)
        {
            StartCoroutine(Heroraser());
        }
    }
    public void UpHeroModeAttackbtn()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        OnHeroAttack = false;
    }
    public void HeroAttackDamage()  // 히어로모드 공격 범위안에 있으면 데미지
    {
        Collider2D[] collider2Ds = Physics2D.OverlapAreaAll(Pos.position, GameObject.Find("scope2").transform.position);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (collider.gameObject.GetComponent<Enemy>() == true)
                {
                    if (collider.GetComponent<Enemy>().InCamera && OnHeroAttack)
                    {
                        collider.gameObject.GetComponent<Enemy>().TakeDamage(20);
                    }
                }
                else if (collider.gameObject.GetComponent<EnemyControl_Basics>() == true)
                {
                    if (collider.GetComponent<EnemyControl_Basics>().InCamera && OnHeroAttack)
                    {
                        collider.gameObject.GetComponent<EnemyControl_Basics>().TakeDamage(20);
                    }
                }
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                if (collider.gameObject.tag == "Enemy_Sushi_Boss")
                {
                    collider.gameObject.GetComponent<EnemyControl_BossSushi>().TakeDamage(20);

                }
                else if (collider.gameObject.tag == "Enemy_Sushi_KingSushi")
                {
                    collider.gameObject.GetComponent<EnemyControl_KingSushi>().TakeDamage(20);
                }
                else if (collider.gameObject.tag == "Enemy_Boss")
                {
                    collider.gameObject.GetComponent<EnemyControl_Boss>().TakeDamage(20);
                }
                else collider.gameObject.GetComponent<Enemy>().TakeDamage(20);
            }
        }
    }
    public void AttackDamage()  // 공격 범위안에 있으면 데미지
    {
        int attackratio = 0;
        switch (countdamage)
        {
            case 1:
                attackratio = Random.Range(6, 11);
                break;
            case 2:
                attackratio = Random.Range(8, 13);
                break;
            case 3:                
                attackratio = Random.Range(10, 15);

                break;
        }

        if(attackratio == 14)
        {
            attackratio = 60;
        }
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                if (collider.gameObject.tag == "Enemy_Sushi_Boss")
                {
                    collider.gameObject.GetComponent<EnemyControl_BossSushi>().TakeDamage(attackratio);
                }
                else if (collider.gameObject.tag == "Enemy_Sushi_KingSushi")
                {
                    collider.gameObject.GetComponent<EnemyControl_KingSushi>().TakeDamage(attackratio);
                }
                else if (collider.gameObject.tag == "Enemy_Boss")
                {
                    collider.gameObject.GetComponent<EnemyControl_Boss>().TakeDamage(attackratio);
                }
                else collider.gameObject.GetComponent<Enemy>().TakeDamage(attackratio);

                if (Heromode == false)
                {
                    if(collider.gameObject.GetComponent<EnemyControl_Sushi>()==true)
                    {
                        PlayerSkillUi.fillAmount += 0.005f;
                    }
                    else PlayerSkillUi.fillAmount += 0.01f;
                }
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (collider.gameObject.GetComponent<Enemy>() == true)
                {
                    collider.gameObject.GetComponent<Enemy>().TakeDamage(attackratio);
                }
                else
                {
                    collider.gameObject.GetComponent<EnemyControl_Basics>().TakeDamage(attackratio);
                }
                if (Heromode == false)
                {
                    if (collider.gameObject.GetComponent<EnemyControl_Sushi>() == true)
                    {
                        PlayerSkillUi.fillAmount += 0.005f;
                    }
                    else PlayerSkillUi.fillAmount += 0.01f;
                }
            }         
        }
    }
    public void skill1Damage()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(gameObject.transform.position, 5);
        //Debug.Log(gameObject.transform.position);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                if (collider.gameObject.tag == "Enemy_Sushi_Boss")
                {
                    collider.gameObject.GetComponent<EnemyControl_BossSushi>().TakeDamage(25);

                }
                else if (collider.gameObject.tag == "Enemy_Sushi_KingSushi")
                {
                    collider.gameObject.GetComponent<EnemyControl_KingSushi>().TakeDamage(25);
                }
                else if (collider.gameObject.tag == "Enemy_Boss")
                {
                    collider.gameObject.GetComponent<EnemyControl_Boss>().TakeDamage(25);
                }
                else collider.gameObject.GetComponent<Enemy>().TakeDamage(25);
            }
            else if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (collider.gameObject.GetComponent<Enemy>() == true)
                {
                    collider.gameObject.GetComponent<Enemy>().TakeDamage(25);
                }
                else collider.gameObject.GetComponent<EnemyControl_Basics>().TakeDamage(25);
            }
        }
    }

    public void ComboAttack()
    {

        lastClick = Time.time;

        if (Attackcount == 1)
        {            
            m_Anim.SetBool("Attack1", true);
        }

    }
    public void CountDamge1()
    {
        countdamage = 1;
    }

    public void CountDamge2()
    {
        countdamage = 2;
    }

    public void CountDamge3()
    {
        countdamage = 3;
    }
    public void TakeDamage(int hit, Vector2 Pos)
    {
        //플레이어 데미지 받음
        if (PlayerHP <= 0)
        {
            DeathCheck(PlayerHP);
        }
        PlayerHP -= hit;

        PlayerHpUi.fillAmount = (PlayerHP / MaxHp);

        Attackcount = 1;
        if (OnSubSound) audi.PlayOneShot(TakeDamageSound);
        Camera.VibrateForTime(0.1f);
        if (!Heromode && gameObject.GetComponent<Warp>().CanWarping)
        {
            //   m_Anim.SetTrigger("Attacked");
            PlayerAction = true; //맞을때 못움직이게
                                 // IsSkillfalse(); //스킬시전중 맞으면 
            if (m_Anim.GetBool("IsSkill") != true)
            {
                float x = transform.position.x - Pos.x;
                if (x < 0)
                {
                    x = -1;
                }
                else
                {
                    x = 1;
                }
                StartCoroutine(Kockback(x));
            }
        }
    }

    IEnumerator Kockback(float dir)
    {
        float KnockTime = 0;
        if (KnockCoolTime <= 0.0f)
        {
            while (KnockTime < 0.2f)
            {
                if (OnVibration) Handheld.Vibrate();
                transform.Translate(Vector2.left * 10f * Time.unscaledDeltaTime * dir);
                KnockTime += Time.unscaledDeltaTime;
                KnockCoolTime = 1.0f;
                yield return null;
            }
        }
    }

    #endregion
    #region 플레이어 상태(ex 히어로 모드, 점프)
    public void HeroMode()
    {
        Debug.Log("히어로 모드");
        Skill2_simEnd();
        Heromode = true;
        m_Anim.ResetTrigger("IdleTrigger");
        m_Anim.SetTrigger("IsHero");
        StartCoroutine(OnHeroMode());
        GameObject.Find("Attackbt").SetActive(false);
        Canvas.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        Canvas.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
        Canvas.transform.GetChild(2).GetChild(7).gameObject.SetActive(true);
        Canvas.transform.GetChild(2).GetChild(6).gameObject.SetActive(false);
        GameObject.Find("SubUIManager").SetActive(false);
        gameObject.transform.GetChild(6).gameObject.SetActive(true);
        for (int i = 0; i < WhiteLine.Length; i++)
        {
            WhiteLine[i].SetActive(false);
        }
        //MainCamera.cullingMask = ~(1 << 14);
        if (OnMainSound)
        {
            audiosource.clip = Herobgm;
            audiosource.Play(); //오디오 재생
            audiosource.loop = false; //반복 여부
        }
    }


    public void IdleMode()
    {
        Debug.Log("기본 모드");
        Heromode = false;
        m_Anim.ResetTrigger("IsHero");
        m_Anim.SetTrigger("DashTrigger");
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(6).gameObject.SetActive(false);
        Canvas.transform.GetChild(0).gameObject.SetActive(true);
        Canvas.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        Canvas.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        Canvas.transform.GetChild(2).GetChild(7).gameObject.SetActive(false);
        Canvas.transform.GetChild(2).GetChild(6).gameObject.SetActive(true);
        Canvas.transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
        for (int i = 0; i < WhiteLine.Length; i++)
        {
            WhiteLine[i].SetActive(true);
        }
 
        if (GM.GetComponent<GameManger>().ChapterLevel == 1) audiosource.clip = bgm1; //오디오에 bgm이라는 파일 연결
        else if (GM.GetComponent<GameManger>().ChapterLevel == 2) audiosource.clip = bgm2;
        else if (GM.GetComponent<GameManger>().ChapterLevel == 3) audiosource.clip = bgm3;
        else audiosource.clip = bgm1; // GameScene에서 바로시작할경우 Default bgm은 bgm1
        if (OnMainSound)
        {
            audiosource.Play(); //오디오 재생
            audiosource.loop = true; //반복 여부
        }
        //MainCamera.cullingMask = -1;
    }
    public void Idle() //기본상태함수
    {
        //m_Anim.SetBool("IsMove", false); 

        if (!Heromode)
        {
            m_Anim.SetBool("AttackedBool", true);
            m_Anim.ResetTrigger("IsJump");
            m_Anim.ResetTrigger("MoveTrigger");
            m_Anim.SetTrigger("IdleTrigger");
        }
        if (Heromode)
        {
            // m_Anim.SetTrigger("IsHero");
            //m_Anim.ResetTrigger("HeroMove");
            m_Anim.SetBool("IsHeroMove", false);
        }

    }
    public void Move() //이동상태함수
    {
        if (!Heromode)
        {
            m_Anim.ResetTrigger("IdleTrigger");
            //m_Anim.SetBool(" Bool", true);
            m_Anim.SetTrigger("MoveTrigger");
            //m_Anim.ResetTrigger("IsJump");
        }
        if (Heromode)
        {
            //m_Anim.ResetTrigger("IsHero");
            //m_Anim.SetTrigger("HeroMove");
            m_Anim.SetBool("IsHeroMove", true);
        }
    }
    //public void MoveSound()
    //{
    //    audi.PlayOneShot(WalkingSound);
    //}
    public void Jump() //점프상태함수
    {
        if (!isSkill2 && gameObject.GetComponent<Warp>().CanWarping == true)
        {
            m_Anim.ResetTrigger("MoveTrigger");
            m_Anim.ResetTrigger("IdleTrigger");
            m_Anim.SetTrigger("IsJump");
            m_Anim.SetBool("AttackedBool", false);
            m_Anim.SetBool("IsSkill", false);
        }
    }
    public void BtnClickSound()
    {
        if (OnSubSound) audi.PlayOneShot(BtnSound);
    }
    public void PlayerJump()
    {
        isjump = true;
        IsPlaySomething = false;
        r2d.AddForce(Vector2.up * 1200);
        if (OnSubSound) audi.PlayOneShot(JumpSound);
    }
    public void PlayerTimeLineAttacked()
    {
        GM.GetComponent<GameManger>().TimeLineing = false;
    }
    public void jumpfinish()
    {
        isjump = false;
    }
    private void Die() //죽었을때 함수
    {
        m_Anim.SetTrigger("IsDie");

    }

    IEnumerator OnHeroMode()
    {
        while (Heromode == true)
        {
            PlayerSkillUi.fillAmount -= 0.1f;
            yield return new WaitForSeconds(0.8f);
        }
    }

    #endregion
    #region 스킬동작관련 함수
    public void IsSkilltrue() //스킬사용중 못움직이게 하는 함수
    {
        joystick.GetComponent<Joystick2>().isSkill = true;
    }
    public void IsSkillfalse() //스킬끝나고 다시 움직이게 하는 함수
    {
        joystick.GetComponent<Joystick2>().isSkill = false;
        IsPlaySomething = false;
        m_Anim.SetBool("IsSkill", false);
        // m_Anim.ResetTrigger("Attacked");
    }
    public void IsSkillBoolTrue()
    {
        m_Anim.SetBool("IsSkill", true);

    }

    public void Dashbtn()
    {
        if (DashCoolDone == true && !isSkill2 && gameObject.GetComponent<Warp>().CanWarping == true && IsPlaySomething == false)
        {
            DashCoolDone = false;
            StartCoroutine(DashCool(DashCoolTime));
            Vector2 direction = (GameObject.Find("scope").transform.position - transform.position).normalized; // direction은 플레이어 좌표에서 마우스 좌표로의 방향을 단위벡터로 만든거
            r2d.AddForce(direction * power, (UnityEngine.ForceMode2D)ForceMode.Acceleration); // direction방향으로 power만큼의 힘을 주자, 대쉬라 했으니, Acceleration(가속도)
            m_Anim.SetTrigger("DashTrigger");
            m_Anim.SetBool("IsSkill", false);
            if (OnSubSound) audi.PlayOneShot(DashSound);
            isjump = false;
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(Pos.position, BoxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Boss"))
                {
                    if (collider.gameObject.tag == "Enemy_Sushi_Boss")
                    {
                        collider.gameObject.GetComponent<EnemyControl_BossSushi>().TakeDamage(10);

                    }
                    else if (collider.gameObject.tag == "Enemy_Sushi_KingSushi")
                    {
                        collider.gameObject.GetComponent<EnemyControl_KingSushi>().TakeDamage(10);
                    }
                    else if (collider.gameObject.tag == "Enemy_Boss")
                    {
                        collider.gameObject.GetComponent<EnemyControl_Boss>().TakeDamage(10);
                    }
                    else collider.gameObject.GetComponent<Enemy>().TakeDamage(10);
                }
                else if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    if (collider.gameObject.GetComponent<Enemy>() == true)
                    {
                        collider.gameObject.GetComponent<Enemy>().TakeDamage(10);
                    }
                    else collider.gameObject.GetComponent<EnemyControl_Basics>().TakeDamage(10);
                }
            }
        }
    }
    public void Skill1btn()
    {
        if (Skill1CoolDone == true && !IsPlaySomething && gameObject.GetComponent<Warp>().CanWarping == true)
        {
            if (OnSubSound) audi.PlayOneShot(Skill1_Sound);
            Attackcount = 1;
            isjump = false;
            Skill1CoolDone = false;
            IsPlaySomething = true;
            m_Anim.SetBool("IsSkill", true);
            StartCoroutine(Skill1Cool(Skill1CoolTime));
            m_Anim.SetTrigger("Skill1");
            StartCoroutine(Skill1Doing());
        }
    }
    public void Skill2btn()
    {
        if (Skill2CoolDone == true && !IsPlaySomething && gameObject.GetComponent<Warp>().CanWarping == true && isjump == false && isground)
        {
            Skill2CoolDone = false;
            IsPlaySomething = true;
            m_Anim.SetBool("IsSkill", true);
            StartCoroutine(Skill2Cool(Skill2CoolTime));
            m_Anim.SetTrigger("Skill2");
            isSkill2 = true;
            if (gameObject.transform.localScale.x >= 1)//오른쪽
            {
                Skill2.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                Skill2.transform.localScale = new Vector3(-1, 1, 1);
            }

            Skill2.SetActive(true);
            Skill2.transform.position = new Vector3(Skill2Range.transform.position.x, 0, 0);
            IsSkilltrue();
        }
    }

    public void Skill3btn()
    {
        if (gameObject.GetComponent<Warp>().CanWarping == true && isjump == false && isground)
            StartCoroutine(Skill3btn_Coroutine());

    }
    IEnumerator Skill3btn_Coroutine()
    {

        if (Skill3CoolDone == true && !IsPlaySomething)
        {
            skil3invincibility = true;
               //skill3TimeLine.GetComponent<PlayableDirector>().Play();            
               Skill3CoolDone = false;
            IsPlaySomething = true;
            isskill3doing = true;
            StartCoroutine(Skill3Cool(Skill3CoolTime));
            StartCoroutine(Skill3_EnemyStop());
            m_Anim.SetTrigger("Skill3");
            r2d.isKinematic = true;
            IsSkilltrue(); //스킬사용중 못움직여
        }
        yield return null;
    }
    IEnumerator Skill3_EnemyStop()
    {
        enemy = FindObjectsOfType<Enemy>();
        GameManger.Instance.EnemyState_Stern = true;

        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].gameObject.tag != "Enemy_Sushi_Boss")
            {              
                if (enemy[i].gameObject.layer == LayerMask.NameToLayer("Boss"))
                {
                    if (enemy[i].gameObject.tag == "Enemy_Sushi_KingSushi")
                    {
                        enemy[i].gameObject.GetComponent<EnemyControl_KingSushi>().anim.SetTrigger("Enemy_Attacked");
                        enemy[i].gameObject.GetComponent<EnemyControl_KingSushi>().anim.SetBool("sk3_IsAttacked", true);
                        Debug.Log("체크");
                    }
                    else if (enemy[i].gameObject.tag == "Enemy_Boss")
                    {
                        enemy[i].gameObject.GetComponent<EnemyControl_Boss>().anim.SetTrigger("Enemy_Attacked");
                        enemy[i].gameObject.GetComponent<EnemyControl_Boss>().anim.SetBool("sk3_IsAttacked", true);
                        Debug.Log("체크");

                    }
                    else
                    {
                        enemy[i].anim.SetTrigger("Enemy_Attacked");
                        enemy[i].anim.SetBool("sk3_IsAttacked", true);
                    }
                }
                else{ 
                enemy[i].anim.SetTrigger("Enemy_Attacked");
                enemy[i].anim.SetBool("sk3_IsAttacked", true);
                }
            }
            else
            {

                if (enemy[i].gameObject.GetComponent<EnemyControl_BossSushi>().fieldMove)
                {

                }
                else
                {
                    //enemy[i].gameObject.GetComponent<EnemyControl_BossSushi>().anim.SetTrigger("Enemy_Attacked");
                    //enemy[i].gameObject.GetComponent<EnemyControl_BossSushi>().anim.SetBool("sk3_IsAttacked", true);
                }
            }


        }

        yield return null;
    }
    public void Skill2_sim1Ani()
    {
        Skill2_sim1.SetActive(true);
        m_Anim.SetBool("IsSkill", true);
        if (OnSubSound) audi.PlayOneShot(Skill2_Sound);
    }
    public void Skill2_sim2Ani()
    {
        Skill2_sim2.SetActive(true);

    }

    public void Skill2_sim3Ani()
    {
        Skill2_sim3.SetActive(true);
    }

    public void Skill2_simEnd()
    {

        Invoke("Skill2_simRe", 1f);
        m_Anim.SetBool("IsSkill", false);
        isSkill2 = false;
        IsSkillfalse();

    }

    public void Skill2_simRe()
    {
        StartCoroutine(Skill2_simReverse());
    }
    public void DeathCheck(float currHp)
    {
        if (currHp <= 0)
        {
            StopAllCoroutines();
            m_Anim.SetBool("IsDie", true);
            gameObject.tag = "Enemy"; //죽는 순간 안맞게 태그 enemy로 바꿔버림 
            Canvas.SetActive(false);
            gameoverobj.SetActive(true);
            StartCoroutine(GoMain());
        }
    }
    IEnumerator Skill2_simReverse()
    {

        for (int i = 0; i < Skill2_sim3.transform.childCount; i++)
            Skill2_sim3.transform.GetChild(i).transform.GetComponent<SKill2_simControl>().StartReverse();
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < Skill2_sim2.transform.childCount; i++)
            Skill2_sim2.transform.GetChild(i).transform.GetComponent<SKill2_simControl>().StartReverse();

        for (int i = 0; i < Skill2_sim1.transform.childCount; i++)
            Skill2_sim1.transform.GetChild(i).transform.GetComponent<SKill2_simControl>().StartReverse();

    }
    IEnumerator GoMain()
    {
        yield return new WaitForSeconds(5f);
        PlayerData.Instance.chapter1count = 0;
        SceneManager.LoadScene("Main");
    }

    IEnumerator Heroraser() //레이저 0.3초마다 데미지 함수 호출
    {
        while (OnHeroAttack)
        {
            HeroAttackDamage();
            if (OnSubSound) audi.PlayOneShot(HeroAttackSound);
            yield return new WaitForSeconds(0.5f);
        }
    }

    #endregion
    #region 스킬 쿨타임

    IEnumerator DashCool(float cool)
    {
        while (img_Dash.fillAmount > 0)
        {
            img_Dash.fillAmount -= 1 * Time.unscaledDeltaTime / cool;
            yield return new WaitForFixedUpdate();
        }
        if (img_Dash.fillAmount <= 0)
        {
            img_Dash.fillAmount = 1.0f;
            DashCoolDone = true;
        }
    }
    IEnumerator Skill1Cool(float cool)
    {
        while (img_Skill1.fillAmount > 0)
        {
            img_Skill1.fillAmount -= 1 * Time.unscaledDeltaTime / cool;
            yield return new WaitForFixedUpdate();
        }
        if (img_Skill1.fillAmount <= 0)
        {
            img_Skill1.fillAmount = 1.0f;
            Skill1CoolDone = true;
        }
    }
    IEnumerator Skill1Doing()
    {
        yield return new WaitForSeconds(3.0f);
        IsPlaySomething = false;
        ResetPlayer();
    }
    IEnumerator Skill2Cool(float cool)
    {
        while (img_Skill2.fillAmount > 0)
        {
            img_Skill2.fillAmount -= 1 * Time.unscaledDeltaTime / cool;
            yield return new WaitForFixedUpdate();
        }
        if (img_Skill2.fillAmount <= 0)
        {
            img_Skill2.fillAmount = 1.0f;
            Skill2CoolDone = true;
        }
    }

    IEnumerator Skill3Cool(float cool)
    {
        while (img_Skill3.fillAmount > 0)
        {
            img_Skill3.fillAmount -= 1 * Time.unscaledDeltaTime / cool;
            yield return new WaitForFixedUpdate();
        }
        if (img_Skill3.fillAmount <= 0)
        {
            img_Skill3.fillAmount = 1.0f;
            Skill3CoolDone = true;
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tear" && !skil3invincibility) //보스 눈물에 맞을경우
        {
            TakeDamage(25, collision.transform.position);
            Playertr.MoveSpeed = 2;
            Invoke("PlayerStateReset", 2f); //2초후 속도 원래대로
        }
        else if (collision.gameObject.tag == "Shine" && !skil3invincibility)
        {
            TakeDamage(250, collision.transform.position);
        }
        else if (collision.gameObject.tag == "Sushi" && !skil3invincibility)
        {
            if (collision.gameObject.transform.parent.gameObject.tag == "Enemy_Sushi_RiceBall" && !skil3invincibility)
            {
                collision.gameObject.transform.parent.GetComponent<Enemy>().TakeDamage(10);
                TakeDamage(20, collision.transform.position);
            }
            else
            {
                TakeDamage(collision.transform.parent.GetComponent<EnemyControl_Sushi>().damage, collision.transform.position);
            }

        }
    }

    public void PlayerStateReset()
    {
        Playertr.MoveSpeed = 5;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.transform.gameObject.tag == "Floor")
        //{
        //    isfloor = true;
        //}
        if (collision.transform.gameObject.tag == "Attack" && !skil3invincibility) //몸통박치기에 사용함
        {
            TakeDamage(1, collision.transform.position);
            collision.gameObject.SetActive(false);
        }
        if (collision.transform.gameObject.tag == "Sushi" && !skil3invincibility)
        {
            TakeDamage(1, collision.transform.position);

        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.transform.gameObject.tag == "Floor")
    //    {
    //        isfloor = false;
    //    }
    //}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(Pos.position, BoxSize);
        //Gizmos.color = Color.green;
        //Gizmos.DrawCube(GameObject.Find("scope2").transform.position, BoxSize2);

    }

    public void Skill_Check()
    {

        int tmp = PlayerPrefs.GetInt("chapter");
        switch (tmp)
        {
            case 1:
                skill2_Lock.SetActive(true);
                skill3_Lock.SetActive(true);
                break;

            case 2:
                skill2_Lock.SetActive(false);
                skill3_Lock.SetActive(true);

                break;

            case 3:
                skill2_Lock.SetActive(false);
                skill3_Lock.SetActive(false);
                break;
        }
    }

}
