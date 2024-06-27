using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    InventoryManager inventoryManager;
    [SerializeField] string itemidx;

    private void Awake()
    {
        inventoryManager = InventoryManager.Instance;

        cItemData testData = new cItemData();
        testData.idx = "00000001";
        testData.sprite = GetComponent<SpriteRenderer>().sprite.name;


        string value = JsonConvert.SerializeObject(testData);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))//���� ����� �÷��̾���
        {
            inventoryManager.GetItem(itemidx);
            //�κ��丮 �Ŵ������� ���� ���� �Ǵ��� Ȯ��
        }
    }
}
