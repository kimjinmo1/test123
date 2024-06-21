using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] Transform trsHand;
    [SerializeField] GameObject objThrowWaepon;
    [SerializeField] Transform trsWeapon;
    private void Start()
    {
        mainCam = Camera.main;//메인카메라
        //카메라가 2개이상일 경우도 존재함
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

        //fixedPos.x > 0 또는 transform. localScale. x -1 => 오른쪽 , 1 => 왼쪽

        float angle = Quaternion.FromToRotation(
             transform.localScale.x < 0 ? Vector3.right : Vector3.left
             , fixedPos).eulerAngles.z;
        trsHand. rotation = Quaternion.Euler(0, 0 ,angle );

     }

    private void CheckCreate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            createWeapon();
        }
    }

    private void createWeapon()
    {
        //GameObject go = Instantiate(objThrowWaepon, );
    }
}
