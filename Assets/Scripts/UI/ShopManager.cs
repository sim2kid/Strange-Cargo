using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ViewPort;

    private List<GameObject> ItemList;
    private float itemSize = 100;
    private float itemMargin = 10;

    private void RenderShop() 
    {
        foreach(Transform child in ViewPort.transform)
            Destroy(child);
        RectTransform rT = ViewPort.GetComponent<RectTransform>();
        float maxSize = itemMargin + (Mathf.Ceil(ItemList.Count / 2) * (itemSize * itemMargin));
        rT.sizeDelta = new Vector2(rT.sizeDelta.x, maxSize);
        for (int i = 0; i < ItemList.Count; i++) 
        {
            GameObject obj = Instantiate(ItemList[i]);
            RectTransform objRT = obj.GetComponent<RectTransform>();
            int down = (int)Mathf.Ceil(i / 2);
            objRT.position = new Vector2(itemMargin + ((i % 2) * (itemMargin + itemSize)), itemMargin + itemSize * down);
            objRT.parent = rT;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
