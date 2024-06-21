using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    [SerializeField] Transform chaseTrs; 

    void Update()
    {
        Vector3 fixedPos = chaseTrs.position;

        fixedPos.z = transform.position.z;

        transform.position = fixedPos;
    }
}
