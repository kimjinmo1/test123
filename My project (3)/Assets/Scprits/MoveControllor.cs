using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

public class MoveControllor : MonoBehaviour
{
    [Header("�÷��̾� �̵� �� ����")]
    Rigidbody2D rigid;
    CapsuleCollider2D coll;
    Animator anim;
    Vector3 moveDir;
    float verticalVelocity = 0f;//�������� �������� ��

    [SerializeField] float jumpForce;
    [SerializeField] float moveSpeed;
    [SerializeField] bool showGoundCheck;
    [SerializeField] float groundCheckLength;//�� ���̰� ���ӿ��� �󸶸�ŭ�� ���̷� �������� �������� ������������ �˼�������
    [SerializeField] Color colorGroundCheck;


    [SerializeField] bool isGround;//�ν����Ϳ��� �÷��̾ �÷���Ÿ�Ͽ� ���� �ߴ���
    bool isJump;

    private void OnDrawGizmos()
    {
        if(showGoundCheck == true)
        {
            Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), colorGroundCheck);
        }
        Debug.DrawLine(transform.position, transform.position - new Vector3(0, groundCheckLength), Color.red);

        //Debug.DrawLind(); ����׵� üũ�뵵�� �� ī�޶� ���� ���� �� ����
        //Gizmos.DrawSphere(); ����� ���� �� ���� �ð�ȿ���� ����
        //Handles.DrawWireArc
    }


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkGrounded();

        moving();
        jump();
        checkGravity();

        doAnim();
    }

    private void checkGrounded()
    {

        isGround = false;

        if(verticalVelocity > 0f)
        {
             return;
        }

        //if(gameObject.CompareTag("Player")== true)//�±״� string ���� ����� �±׸� ����
        //{

        //}

        //Layer int �� ����� ���̾ ����
        //Layer�� int �� ���������� Ȱ���ϴ� ont�� �ٸ�
        //Wall Layer , Ground Layer 
        RaycastHit2D hit =
        Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength, LayerMask.GetMask("Ground"));

        if (hit)
        {
            isGround = true;
        }
    }


    private void moving()
    {
        //�¿�Ű�� ������ �¿�� �����δ�
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a L A Key -1 , d , R A Key1 , �ƹ��͵� �Է����� ������ 0
        moveDir.y = rigid.velocity.y;
        //���ð��� ���鶧�� ������Ʈ�� �ڵ忡 ���ؼ� �����̵� �ϰ� ��������� 
        //���������ؼ� �̵�
        rigid.velocity = moveDir;//y 0 
    }

    private void jump()
    {
        //if (isGround == true && isJump == false && Input.GetKeyDown(KeyCode.Space))
        //{
        //    rigid.AddForce(new Vector2(0 , jumpForce), ForceMode2D.Impulse);//������ �̴� ��
        //    isJump = true;
        //}

        if (isGround == false)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            isJump = true;
        }
    }

    private void checkGravity()
    {
        if(isGround == false)//���߿� ���ִ� ����
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;//-9.81 

            if(verticalVelocity < -10f)
            {
                verticalVelocity = -10f;
            }
        }
        else if ( isJump == true)
        {
            isJump = false;
            verticalVelocity = jumpForce;
        }
        else if(isGround == true)
        {
            verticalVelocity = 0;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }
    private void doAnim()
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
        anim.SetBool("IsGroind", isGround);
    }
}