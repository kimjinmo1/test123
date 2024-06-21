using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CusorManager : MonoBehaviour
{
    [Header("커서이미지")]
    [SerializeField, Tooltip("0은 <color=red> 디폴트</color> ,1은 <color=rde>클릭<color>")]
    Texture2D[] cursors;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()//마우스 클릭 코드
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            Cursor.SetCursor(cursors[1], new Vector2(cursors[1]. width * 0.5f, cursors[1].height * 0.5f),
                CursorMode.Auto );
        }
        else
        {
            Cursor.SetCursor(cursors[0], new Vector2(cursors[1].width * 0.5f, cursors[1].height * 0.5f),
                CursorMode.Auto);
        }
    }
}
