using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject InventoryPrefab;
    public InventoryObject InventoryObj;

    private int numberOfColumns;
    Dictionary<InventoryItemSlot, GameObject> itemsDisplayed = new Dictionary<InventoryItemSlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < InventoryObj.Container.Items.Count; i++)
        {
            InventoryItemSlot _slot = InventoryObj.Container.Items[i];
            if (itemsDisplayed.ContainsKey(_slot))
            {
                itemsDisplayed[_slot].GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount.ToString("n0");
            }
            else
            {
                var _obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                _obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = InventoryObj.Database.GetItem[_slot.Item.ID].ItemSprite;
                print(InventoryObj.Database.GetItem[_slot.Item.ID]);
                _obj.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount.ToString("n0");
                itemsDisplayed.Add(_slot, _obj);
            }
        }
    }

    void CreateDisplay()
    {
        numberOfColumns = InventoryObj.Container.Items.Count;
        for (int i = 0; i < InventoryObj.Container.Items.Count; i++)
        {
            InventoryItemSlot _slot = InventoryObj.Container.Items[i];

            var _obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            //_obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            _obj.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount.ToString("n0");
            itemsDisplayed.Add(_slot, _obj);
        }
    }

/*    public Vector3 GetPosition(int i)
    {
        return new Vector3(0, (-Y_SpaceBetweenItems * (i % numberOfColumns)), 0); //number of columns = items quantity
    }*/

}
