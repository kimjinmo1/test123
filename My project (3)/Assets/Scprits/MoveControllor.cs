using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        if(gameObject.CompareTag("Player")== true)//�±״� string ���� ����� �±׸� ����
        {

        }

        //Layer int �� ����� ���̾ ����
        //Layer�� int �� ���������� Ȱ���ϴ� ont�� �ٸ�
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
        //�¿�Ű�� ������ �¿�� �����δ�
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;//a L A Key -1 , d , R A Key1 , �ƹ��͵� �Է����� ������ 0
        moveDir.y = rigid.velocity.y;
        //���ð��� ���鶧�� ������Ʈ�� �ڵ忡 ���ؼ� �����̵� �ϰ� ��������� 
        //���������ؼ� �̵�
        rigid.velocity = moveDir;//y 0 

       
    }
}
