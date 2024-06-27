using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{


    public static InventoryManager Instance;

    [SerializeField] GameObject viewInventory;//인벤토리
    [SerializeField] GameObject fabItem; //인벤토리에 생성될 프리팹

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

        Transform[] childs = viewInventory.GetComponentsInChildren<Transform>();//문제가 있음

        listTrsInventory.AddRange(childs);
        listTrsInventory.RemoveAt(0);
    }
    /// <summary>
    /// 인벤토리가 열려있다면 닫힘, 닫혀있다면 열림
    /// </summary>
    public void InActiveInventory()
    {
      viewInventory.SetActive(!viewInventory.activeSelf);
    }
    /// <summary>
    /// 비어있는 인벤토리 넘버를 리턴합니다. -1이 리턴된다면 비어이는 슬롯이 없다는 의미입니다.
    /// </summary>
    /// <returns>비어있는 아이템 슬롯 번호</returns>
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
        //오브젝트에게 너는 _idx번호가 너의 정보 데이터야 
        return  true;

        
    }
}


