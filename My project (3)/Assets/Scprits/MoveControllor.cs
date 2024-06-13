using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveControllor : MonoBehaviour
{
    [Header("플레이어 이동 및 점프")]
    Rigidbody2D rigid;
    CapsuleCollider2D coll;
    Animator anim;
    Vector3 moveDir;
    float verticalVelocity = 0f;//수직으로 떨어지는 힘

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;
    [SerializeField] bool showGoundCheck;
    [SerializeField] float groundCheckLength;//이 길이가 게임에서 얼마만큼의 길이로 나오는지 육안으로 보기전까지는 알수가없음
    [SerializeField] Color colorGroundCheck;


    [SerializeField] bool isGround;//인스펙터에서 플레이어가 플랫폼타일에 착지 했는지

    private void OnDrawGizmos()
    {
        if(showGoundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);
        }
        Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), Color.red);

        //Debug.DrawLind(); 디버그도 체크용도로 씬 카메라에 선을 그을 수 있음
        //Gizmos.DrawSphere(); 디버그 보다 더 많은 시각효과를 제공
        //Handles.DrawWireArc
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();

        moving();
    }

    private void checkGrounded()
    {
        if(gameObject.CompareTag("Player")== true)//태그는 string 으롷 대상의 태그를 구분
        {

        }

        //Layer int 로 대상의 레이어를 구분
        //Layer의 int 와 공통적으로 활용하는 ont와 다름
        //Wall Layer , Ground Layer 
        RaycastHit2D hit =
        Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

        if (hit)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        } 
    }


    private void moving()
    {
        //좌우키를 누르면 좌우로 움직인다
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a L A Key -1 , d , R A Key1 , 아무것도 입력하지 않으면 0
        moveDir.y = rigid.velocity.y;
        //슈팅게임 만들때는 오브젝트를 코드에 의해서 순간이동 하게 만들었지만 
        //물리에의해서 이동
        rigid.velocity = moveDir;//y 0 

       
    }
}
