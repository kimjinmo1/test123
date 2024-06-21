using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] Transform trsHand;

    private void Start()
    {
        mainCam = Camera.main;//����ī�޶�
        //ī�޶� 2���̻��� ��쵵 ������
        //Camera.current
    }

    void Update()
    {
        checkAim();
    }

    private void checkAim()
    {
        Vector2 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = transform.position;
        Vector2 fixedPos = mouseWorldPos - playerPos;

        //fixedPos.x > 0 �Ǵ� transform. localScale. x -1 => ������ , 1 => ����

        float angle = Quaternion.FromToRotation(Vector3.left , fixedPos).eulerAngles.z;
        trsHand. rotation = Quaternion.Euler(0, 0 ,angle );

     }
}
