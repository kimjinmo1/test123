using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

//�巡�� ������ �κ��丮 ������
public class ItemUi : MonoBehaviour ,  IBeginDragHandler , IDragHandler , IEndDragHandler
{
    Transform canvas;//�巡���Ҷ� ���� UI�ڷ� �׷����°��� �����ϱ� ���� ��� �̿��� ĵ���� 
    Transform beforeParent;//Ȥ�� �߸��� ��ġ�� ����ϰԵǸ� ���ƿ��� ���� ��ġ 

    CanvasGroup canvasGroup;//�ڽĵ��� ���� �����ϴ� ������Ʈ


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        canvas = InventoryManager.Instance.CanvasInventory;
    }
    /// <summary>
    /// idx �ѹ��� ���޹����� �ش� �������� Josn���� ���� �˻��Ͽ� ã��
    /// �ش� ������ �����Ϳ��� �ʿ��� �������� �����ͼ� �ش� ��ũ��Ʈ�� ä����
    /// </summary> 
    /// <param name="_idx">�������� �ε��� �ѹ�</param>
    public void SetItem(string _idx)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beforeParent = transform.parent;

        transform.SetParent(canvas);

        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
       transform .position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(transform.parent == canvas)
        {
            transform.SetParent(beforeParent);
            transform.position = beforeParent.position;
        }

        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

    }
}

   
