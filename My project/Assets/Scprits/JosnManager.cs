using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class JosnManager : MonoBehaviour
{

    public static JosnManager Instance;

    List<cItemData> itemDatas;
    //[SerializeField] TextAsset itemDta;

    TextAsset itemData;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);

        }

        initJosnDatas();
    }
    private void initJosnDatas()
    {
       // itemData = Resources.Load<TextAsset>("itemData");//null
       // itemData = (TextAsset)Resources.Load("itemData");
        itemData = Resources.Load("itemData") as TextAsset;
        itemDatas = JsonConvert.DeserializeObject<List<cItemData>>(itemData.ToString());
    }

    public string GetNameFromldx(string _idx)
    {
        if (itemData == null) return string.Empty;

        return itemDatas.Find(x => x.idx == _idx).sprite;
    }

}
