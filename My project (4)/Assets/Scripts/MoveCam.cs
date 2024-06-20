using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] Transform chaseTrs;

    void Update()
    {
        Vector3 fixedPos = chaseTrs.position;

        fixedPos.z = transform.position.z;
        transform.position = fixedPos;//z축까지 따라가게 된다.
        //transform.position = chaseTrs.position;//z축까지 따라가게 된다.
    }
}
