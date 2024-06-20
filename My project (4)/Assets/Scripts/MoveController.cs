using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

public class MoveController : MonoBehaviour
{
    //(Case by case임) manager, 비동기적으로 호출이 왔을때만 대응
    //controller, update사용 동기적으로 호출이 오지 않더라도 타 기능을 불러서 사용하는 경우가 많음

    [Header("플레이어 이동 및 점프")]
    Rigidbody2D rigid;
    CapsuleCollider2D capcol;
    BoxCollider2D boxcol;
    Animator anim;
    Vector3 moveDir;
    float virticalVelocity = 0f;//수직으로 떨어지는 힘

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;

    [SerializeField] bool showGroundCheck;
    [SerializeField] float groundCheckLength;//이 길이가 게임에서 얼마만큼의 길이로 나오는지 육안으로 보기전까지는 알수가 없음
    [SerializeField] Color colorGroundCheck;

    [SerializeField] bool isGround;//인스펙터에서 플레이어가 플랫폼 타일에 착지했는지
    bool isJump;
    float verticalVelocity;

    Camera camMain;
    [Header("벽 점프")]
    [SerializeField] bool TouchWall;
    bool isWallJump;
    [SerializeField] float wallJumpTime = 0.3f;
    float wallJumpTimer = 0.0f; // 타이머 

    [Header("e대시")]
    [SerializeField] private float dashTime = 0.3f;
    [SerializeField] private float dashSpeed = 20.0f;
     float dashTimer = 0.0f;//타이머
    //대시이펙트
    
    //Unity Editor에서만 사용이 가능하다.
    private void OnDrawGizmos()
    {
        if (showGroundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);
        }

        //Debug.DrawLine(); 디버그도 체크용도로 씬 카메라에 선을 그려줄수 있음
        //Gizmos.DrawSphere(); 디버그보다 더 많은 시각효과를 제공(Gizmos.Draw)
        //Handles.DrawWireArc(); Gizmos보다 더 많은 시각효과를 제공한다(Handles.Draw) 단 Unity Editor기능이므로 게임에는 가지고 갈 수 없다.
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))//상대방의 콜라이더를 가져옴, 누가 가져온지를 모름
    //    {
    //        TouchWall = true;
    //    }
    //}

    //private void OnTriggerExit2D (Collider2D collision)
    //{
    //    if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
    //    {
    //        TouchWall = false;
    //    }
    //}

    public void TriggerEnter(HitBox.ehitboxType _type , Collider2D _collision)
    {
        if(_type == HitBox.ehitboxType.WallChcek)
        {
            if (_collision.gameObject.layer == LayerMask.NameToLayer("Wall"))//상대방의 콜라이더를 가져옴, 누가 가져온지를 모름
             {
               TouchWall = true;
             }
        }
    }

    public void TriggerExit(HitBox.ehitboxType _type, Collider2D _collision)
    {
        if (_type == HitBox.ehitboxType.WallChcek)
        {
            if (_collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
               TouchWall = false;
             }
        }
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        boxcol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        camMain = Camera.main;
    }

    void Update()
    {
        checkTimers();

        dash();

        moving();
        checkAim();
        checkGrounded();
        jump();
        checkGravity();
        doAnim();
    }


    private void dash()
    {
        if (dashTimer == 0.0f && 
            (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.F)))
        {
            dashTimer = dashTime;
            verticalVelocity = 0;

            //if(transform.localScale.x > 0 )//왼쪽
            //{
            //    rigid.velocity = new Vector2(-dashSpeed, verticalVelocity);
            //}
            //else//오른쪽
            //{
            //    rigid.velocity = new Vector2(dashSpeed, verticalVelocity);
            //}

            // rigid.velocity = transform.localScale.x > 0 ? new Vector2(-dashSpeed, verticalVelocity) : new Vector2(dashSpeed , verticalVelocity);

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
            }
        }
    }
    private void checkGrounded()
    {
        isGround = false;

        if (verticalVelocity > 0f)
        {
            return;
        }
        //if (gameObject.CompareTag("Player") == true)//태그는 string으로 대상의 태그를 구분
        //{
        //}
        //Layer int로 대상의 레이어를 구분한다.
        //Layer의 int와 공통적으로 활용하는 int와 다르다
        //Wall Layer, Ground Layer
        #region 선 하나로 지상판정을 하는 코드
        RaycastHit2D hit =

        // Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

        //if (hit)
        //{
        //    isGround = true;
        //}
        #endregion
        Physics2D.BoxCast(boxcol.bounds.center, boxcol.bounds.size, 0f, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

        if (hit)
        {
            isGround = true;
        }
    }

    private void moving()

    {
        if (wallJumpTimer > 0.0f || dashTimer > 0.0f)
        {
            return ;
        }

        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a,L A key -1, d, R A Key 1, 아무것도 입력하지 않으면 0
        moveDir.y = rigid.velocity.y;
        //슈팅게임을 만들때는 오브젝트를 코드에 대해서 순간이동하게 만들었지만
        //물리에 의해서 이동 물리에 의해서 이동될때는 time.deltaTime값을 줄 필요가 없다.
        rigid.velocity = moveDir;//moveDir값의 default값이 0,0,0인데 y의 값이 0이기 때문에 계속해서 초기화됨(물리와 코드가 별도로 동작)
    }

    private void checkAim()
    {
        #region 키보드를 통한 방향전환 기능(주석처리됨)
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
        #endregion

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
        //if (isGround == true && isJump == false  Input.GetKeyDown(KeyCode.Space)) 
        //{
        //    rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);//지긋이 미는힘
        //}

        if (isGround == false) // 공중에 떠있는 상태라면
        {
            //벽에 붙어있고, 그리고 벽을 향해 플레이어가 방향키를 누르고 있는데 점프키를 눌렀다면 
            if (TouchWall == true && moveDir.x != 0f && Input.GetKeyDown(KeyCode.Space))
            {
                isWallJump = true; 
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJump = true;
        }
        else if (isGround == true)
        {
            verticalVelocity = 0;
        }
    }

    private void checkGravity()
    {
        if(dashTimer > 0.0f)
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
            //일정시간 유저가 입력할수 없어야 벽을 발로찬 x 값을 볼수있음
            //입력불가 타이머를 작동시켜야함
            wallJumpTimer = wallJumpTime;
        }
        if (isGround == false)//공중에 떠있는 상태
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;//-9.81 유니티 자체 중력으로 사용하면 공기저항이 없는이상 계속해서 가속을 하게된다.

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
        else if(isGround == true)
        {
            verticalVelocity = 0f;
        }
        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void doAnim()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("isGround", isGround);
    }
}
