using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class MoveController : MonoBehaviour
{
    [Header("플레이어 이동 및 점프")]
    Rigidbody2D rigid;//null
    CapsuleCollider2D coll;
    BoxCollider2D box2d;
    Animator anim;//null
    Vector3 moveDir;//0, 0, 0
    float verticalVelocity = 0f;//수직으로 떨어지는 힘

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float groundCheckLength;//이 길이가 게임에서 얼마만큼의 길이로 나오는지 육안으로 보기전까지는 알수가 없음
    [SerializeField] Color colorGroundCheck;

    [SerializeField] bool isGround;//인스펙터에서 플레이어가 플랫폼타일에 착지 했는지
    bool isJump;

    Camera camMain;

    [Header("벽 점프")]
    [SerializeField] bool touchWall;
    bool isWallJump;
    [SerializeField] float wallJumpTime = 0.3f;
    float wallJumpTimer = 0.0f;//타이머

    [Header("대시")]
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashSpeed = 20.0f;
    float dashTimer = 0.0f;//타이머
    TrailRenderer dashEffect;//null
    [SerializeField] private float dashCoolTime = 2f;//2초
    float dashCoolTimer = 0.0f;//타이머

    [Header("대시 UI")]
    [SerializeField] GameObject objDashCoolTime;
    [SerializeField] Image imgFill;
    [SerializeField] TMP_Text TextCoolTime;

    [SerializeField] KeyCode dashKey;

    private void OnDrawGizmos()
    {
        if (showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);
        }

        //Debug.DrawLine(); 디버그도 체크용도로 씬 카메라에 선을 그려줄수 있음
        //Gizmos.DrawSphere(); 디버그 보다 더 많은 시각효과를 제공
        //Handles.DrawWireArc
    }

    //private void OnTriggerEnter2D(Collider2D collision)//상대방의 콜라이더를 가져옴, 누가 실행시킨지는 모름
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        touchWall = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        touchWall = false;
    //    }
    //}

    public void TriggerEnter(HitBox.ehitboxType _type ,Collider2D _collision)
    {
        if (_type == HitBox.ehitboxType.WallCheck)
        {
            if (_collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                touchWall = true;
            }
        }
    }

    public void TriggerExit(HitBox.ehitboxType _type, Collider2D _collision)
    {
        if (_type == HitBox.ehitboxType.WallCheck)
        {
            if (_collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                touchWall = false;
            }
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        box2d = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        dashEffect = GetComponent<TrailRenderer>();
        dashEffect.enabled = false;
        initUI();
    }

    void Start()
    {
        camMain = Camera.main;
    }

    void Update()
    {
        checkTimers();

        checkGrounded();

        dash();

        moving();
        checkAim();
        jump();

        checkGravity();

        doAnim();
    }

    private void dash()
    {
        if (dashTimer == 0.0f && dashCoolTimer == 0.0f &&
            (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.F)))
        {
            dashTimer = dashTime;
            dashCoolTimer = dashCoolTime;
            verticalVelocity = 0;
            dashEffect.enabled = true;
            //if (transform.localScale.x > 0)//왼쪽
            //{
            //    rigid.velocity = new Vector2(-dashSpeed, verticalVelocity);
            //}
            //else//오른쪽
            //{
            //    rigid.velocity = new Vector2(dashSpeed, verticalVelocity);
            //}

            //rigid.velocity = transform.localScale.x > 0 ? new Vector2(-dashSpeed, verticalVelocity) : new Vector2(dashSpeed, verticalVelocity);

            rigid.velocity = 
                new Vector2(transform.localScale.x > 0 ? -dashSpeed : dashSpeed, 0.0f);
        }
    }

    private void checkTimers()
    {
        if (wallJumpTimer > 0.0f)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer < 0.0f)
            {
                wallJumpTimer = 0.0f;
            }
        }

        if (dashTimer > 0.0f)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer < 0.0f) 
            {
                dashTimer = 0.0f;
                dashEffect.enabled = false;
                dashEffect.Clear();
            }
        }

        if (dashCoolTimer > 0.0f)
        {
            if (objDashCoolTime.activeSelf == false)
            {
                objDashCoolTime.SetActive(true);
            }

            dashCoolTimer -= Time.deltaTime;
            if (dashCoolTimer < 0.0f)
            {
                dashCoolTimer = 0.0f;
                objDashCoolTime.SetActive(false);
            }

            //dashCoolTime = 2초, 스킬을 쓰면 0, 점점 1이 되어가야함
            //2(타이머)/2(최대타이머) = 1, 0.5, 0
            //0, 0.5, 1

            imgFill.fillAmount = 1 - dashCoolTimer / dashCoolTime;
            TextCoolTime.text = dashCoolTimer.ToString("F1");
        }
    }

    private void checkGrounded()
    {
        isGround = false;

        if (verticalVelocity > 0f)
        {
            return;
        }

        //Layer int로 대상의 레이어를 구분
        //Layer의 int와 공통적으로 활용하는 int와 다름
        //Wall Layer, Ground Layer 
        RaycastHit2D hit =
        //Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

        Physics2D.BoxCast(box2d.bounds.center, box2d.bounds.size, 0f, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

        if (hit)
        {
            isGround = true;
        }
    }

    private void moving()
    {
        if (wallJumpTimer > 0.0f || dashTimer > 0.0f)
        {
            return;
        }
        //좌우키를 누르면 좌우로 움직인다
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a, L A Key -1, d, R A Key 1, 아무것도 입력하지 않으면 0
        moveDir.y = rigid.velocity.y;
        //슈팅게임 만들때는 오브젝트를 코드에 의해서 순간이동 하게 만들었지만
        //물리에의해서 이동
        rigid.velocity = moveDir;//y 0, time.deltaTime;
    }

    private void checkAim()
    {
        //Vector3 scale = transform.localScale;
        //if (moveDir.x < 0 && scale.x != 1.0f)//왼쪽 
        //{
        //    scale.x = 1.0f;
        //    transform.localScale = scale;
        //    Debug.Log("<color=red>동작</color>");
        //}
        //else if (moveDir.x > 0 && scale.x != -1.0f)//오른쪽 
        //{
        //    scale.x = -1.0f;
        //    transform.localScale = scale;
        //    Debug.Log("<color=blue>동작</color>");
        //}

        Vector2 mouseWorldPos = camMain.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 fixedPos = mouseWorldPos - playerPos;

        Vector3 playerScale = transform.localScale;
        if (fixedPos.x > 0 && playerScale.x != -1.0f)
        {
            playerScale.x = -1.0f;
        }
        else if (fixedPos.x < 0 && playerScale.x != 1.0f)
        {
            playerScale.x = 1.0f;
        }
        transform.localScale = playerScale;
    }

    private void jump()
    {
        //if (isGround == true && Input.GetKeyDown(KeyCode.Space))
        //{
        //    rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);//지긋이 미는 힘
        //}

        if (isGround == false)//공중에 떠있는 상태라면
        {
            //벽에 붙어있고, 그리고 벽을 향해 플레이어가 방향키를 누르고 있는데 점프키를 눌러다면
            if (touchWall == true && moveDir.x != 0f && Input.GetKeyDown(KeyCode.Space))
            {
                isWallJump = true;
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJump = true;
        }
    }

    private void checkGravity()
    {
        if (dashTimer > 0.0f)
        {
            return;
        }
        else if (isWallJump == true)
        {
            isWallJump = false;

            Vector2 dir = rigid.velocity;
            dir.x *= -1f;//반대방향
            rigid.velocity = dir;

            verticalVelocity = jumpForce * 0.5f;
            //일정시간 유저가 입력할수 없어야 벽을 발로찬 x값을 볼수 있음
            //입력불가 타이머를 작동시켜야함
            wallJumpTimer = wallJumpTime;
        }
        else if (isGround == false)//공중에 떠있는 상태
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;//-9.81

            if (verticalVelocity < -10f)
            {
                verticalVelocity = -10f;
            }
        }
        else if (isJump == true)
        {
            isJump = false;
            verticalVelocity = jumpForce;
        }
        else if (isGround == true)
        {
            verticalVelocity = 0;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void doAnim()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("IsGround", isGround);
    }

    private void initUI()
    {
        objDashCoolTime.SetActive(false);
        imgFill.fillAmount = 0;
        TextCoolTime.text = "";
    }

}
