using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public enum ehitboxType
    {
        WallChcek,
        BodyCheck,
    }

    [SerializeField] ehitboxType hitboxType;
    MoveController moveController;

    private void Awake()
    {
        moveController = GetComponentInParent<MoveController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        moveController.TriggerEnter(hitboxType, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        moveController.TriggerExit(hitboxType, collision);
    }
    // Start is called before the first frame update
    
}
