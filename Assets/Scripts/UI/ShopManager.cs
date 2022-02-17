using PersistentData.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ViewPort;

    [SerializeField]
    private List<GameObject> ItemList;
    private float itemSize = 100;
    private float itemMargin = 10;

    private Queue<System.Func<GameObject>> SetPositions;

    private void RenderShop() 
    {
        DestoryShop();
        RectTransform rT = ViewPort.GetComponent<RectTransform>();
        float maxSize = itemMargin + (Mathf.Ceil(ItemList.Count / 2f) * (itemSize + itemMargin));
        rT.sizeDelta = new Vector2(0, maxSize);
        for (int i = 0; i < ItemList.Count; i++) 
        {
            GameObject obj = Instantiate(CreateButton(ItemList[i]), rT.position, rT.rotation, rT);
            obj.transform.localScale = Vector3.one;

            RectTransform objRT = obj.GetComponent<RectTransform>();
            int down = (int)Mathf.Ceil(i / 2);
            float x = itemMargin + ((i % 2) * (itemMargin + itemSize)) + (itemSize / 2);
            float y = itemMargin + ((itemSize + itemMargin) * down) + (itemSize / 2);
            SetPositions.Enqueue(() => 
            {
                objRT.localPosition = new Vector3(x, -y, objRT.localPosition.z);
                return objRT.gameObject;
            });
        }
    }

    private void DestoryShop() 
    {
        foreach (Transform child in ViewPort.transform)
            Destroy(child.gameObject);
        RectTransform rT = ViewPort.GetComponent<RectTransform>();
        rT.sizeDelta = new Vector2(0, 0);
    }

    private GameObject CreateButton(GameObject Prefab, PrefabData prefabData = null) 
    {
        return Prefab;
    }

    private void OnEnable()
    {
        SetPositions = new Queue<System.Func<GameObject>>();
        RenderShop();   
    }

    private void OnDisable()
    {
        DestoryShop();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SetPositions.Count > 0) 
        {
            foreach (var runMe in SetPositions) 
            {
                runMe.Invoke();
            }
        }
        SetPositions.Clear();
    }
}
