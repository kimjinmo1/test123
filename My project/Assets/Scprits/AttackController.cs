using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] Transform trsHand;
    [SerializeField] GameObject objThrowWaepon;
    [SerializeField] Transform trsWeapon;
    [SerializeField] Transform trsDynamic;
    [SerializeField] Vector2 throwforce = new Vector2(10f, 0f);
    private void Start()
    {
        mainCam = Camera.main;//메인카메라
        //카메라가 2개이상일 경우도 존재함
        //Camera.current
    }

    void Update()
    {
        checkAim();
        CheckCreate();
        
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
        GameObject go = Instantiate(objThrowWaepon, trsWeapon.position, trsWeapon.rotation, trsDynamic);
        Throwweapon goSc =go.GetComponent<Throwweapon>();
        bool isRight = transform.localScale.x < 0 ? true : false;
        Vector2 fixedThrowForce = throwforce;
        if(isRight == false)
        {
            fixedThrowForce = -throwforce;//x10 y0
        }

        goSc.SetForce(trsWeapon.rotation * fixedThrowForce, isRight);

    }
}
