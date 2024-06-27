using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



//�������� �巡�� �����ϰų� �巡���ؼ� ������ �� ��ũ��Ʈ�� �巡�׾�������
//�ľ��Ͽ� �ڽ����� �з�����

public class InventorySlot : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler , IDropHandler
{

    Image imgSlot;
    RectTransform rect;
    void Start()
    {
       
    }

    
    void Update()
    {
        
    }

    private void Awake()
    {
        imgSlot = GetComponent<Image>();
        rect= GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        imgSlot.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imgSlot.color = Color.white;
    }
    /// <summary>
    /// �̺�Ʈ �ý������� ���� �巡�� �Ǵ� ����� �� ��ũ��Ʈ ������ ��ӵǰ� �Ǹ� 
    /// �ش� ��� ������Ʈ�� ���� �ڽ����� �����մϴ�.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrop(PointerEventData eventData)
    {
       if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.transform.position = rect.position;
        }
    }
}

