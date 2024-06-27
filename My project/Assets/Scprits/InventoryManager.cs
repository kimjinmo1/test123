using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{


    public static InventoryManager Instance;

    [SerializeField] GameObject viewInventory;//�κ��丮
    [SerializeField] GameObject fabItem; //�κ��丮�� ������ ������

    [SerializeField] Transform canvasInventory;
    public Transform CanvasInventory => canvasInventory;

    List<Transform> listTrsInventory = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        initInventory();
    }

    private void initInventory()
    {
        listTrsInventory.Clear();

        Transform[] childs = viewInventory.GetComponentsInChildren<Transform>();//������ ����

        listTrsInventory.AddRange(childs);
        listTrsInventory.RemoveAt(0);
    }
    /// <summary>
    /// �κ��丮�� �����ִٸ� ����, �����ִٸ� ����
    /// </summary>
    public void InActiveInventory()
    {
      viewInventory.SetActive(!viewInventory.activeSelf);
    }
    /// <summary>
    /// ����ִ� �κ��丮 �ѹ��� �����մϴ�. -1�� ���ϵȴٸ� ����̴� ������ ���ٴ� �ǹ��Դϴ�.
    /// </summary>
    /// <returns>����ִ� ������ ���� ��ȣ</returns>
    private int getEmptyItemSlot()
    {
        int count = listTrsInventory.Count;
        for(int iNum = 0; iNum < count; ++iNum)
        {
            Transform trsSlot = listTrsInventory[iNum];
            if(trsSlot.childCount == 0)
            {
                return iNum;
            }
        }
        return -1;
    }


    public bool GetItem(string _idx)
    {
        int slotNum = getEmptyItemSlot();
        if(slotNum == -1)
        {
            return false;
        }

        GameObject go = Instantiate(fabItem, listTrsInventory[slotNum]);
        //������Ʈ���� �ʴ� _idx��ȣ�� ���� ���� �����;� 
        return  true;

        
    }
}


