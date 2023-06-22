using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem MouseItemInstance = new MouseItem();
    public GameObject InventoryPrefab;
    public InventoryObject InventoryObj;
    public Vector2 _placeholderSpriteSize = new Vector2(300, 100);

    Transform _parentAfterDrag;
    private int numberOfColumns;
    //Dictionary<InventoryItemSlot, GameObject> itemsDisplayed = new Dictionary<InventoryItemSlot, GameObject>();
    Dictionary<GameObject, InventoryItemSlot> itemsInSlotsDisplayed = new Dictionary<GameObject, InventoryItemSlot>();

    public Dictionary<InventoryItemSlot, GameObject> itemsDisplayed = new Dictionary<InventoryItemSlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
        //CreateSlots(); //created 17.06
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
        //UpdateSlots(); //created 17.06
    }

    void UpdateDisplay()
    {
        for (int i = 0; i < InventoryObj.Container.Items.Count; i++)
        {
            InventoryItemSlot _slot = InventoryObj.Container.Items[i];
            if (itemsDisplayed.ContainsKey(_slot))
            {
                itemsDisplayed[_slot].GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount == 1 ? "" : _slot.Amount.ToString("n0");
                itemsDisplayed[_slot].transform.GetChild(0).GetComponentInChildren<Image>().sprite = InventoryObj.Database.GetItem[_slot.Item.ID].ItemSprite;
            }
            else
            {
                var _obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                //print(InventoryObj.Database.GetItem[_slot.Item.ID]);
                _obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = InventoryObj.Database.GetItem[_slot.Item.ID].ItemSprite;
                _obj.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount == 1 ? "" : _slot.Amount.ToString("n0");

                AddEvent(_obj, EventTriggerType.PointerEnter, delegate { OnEnter(_obj); });
                AddEvent(_obj, EventTriggerType.PointerExit, delegate { OnExit(_obj); });
                AddEvent(_obj, EventTriggerType.BeginDrag, delegate { OnDragStart(_obj, _slot); });
                AddEvent(_obj, EventTriggerType.EndDrag, delegate { OnDragEnd(_obj, _slot); });
                AddEvent(_obj, EventTriggerType.Drag, delegate { OnDrag(_obj); });

                itemsDisplayed.Add(_slot, _obj);
                itemsInSlotsDisplayed.Add(_obj, InventoryObj.Container.Items[i]);
            }
            //print("name: " + _slot.Item.Name + " | ID = " + _slot.Item.ID);
            /*            foreach (KeyValuePair<InventoryItemSlot, GameObject> itemsDusplay in itemsDisplayed)
                        {
                            print(itemsDusplay.Key + " | " + itemsDusplay.Value);
                        }*/
        }
    }

    void CreateDisplay()
    {
        itemsDisplayed = new Dictionary<InventoryItemSlot, GameObject>();

        numberOfColumns = InventoryObj.Container.Items.Count;

        for (int i = 0; i < InventoryObj.Container.Items.Count; i++)
        {
            InventoryItemSlot _slot = InventoryObj.Container.Items[i];

            var _obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            //_obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            _obj.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount == 1 ? "" : _slot.Amount.ToString("n0");

            AddEvent(_obj, EventTriggerType.PointerEnter, delegate { OnEnter(_obj); });
            AddEvent(_obj, EventTriggerType.PointerExit, delegate { OnExit(_obj); });
            AddEvent(_obj, EventTriggerType.BeginDrag, delegate { OnDragStart(_obj, _slot); });
            AddEvent(_obj, EventTriggerType.EndDrag, delegate { OnDragEnd(_obj, _slot); });
            AddEvent(_obj, EventTriggerType.Drag, delegate { OnDrag(_obj); });

            itemsDisplayed.Add(_slot, _obj);
            itemsInSlotsDisplayed.Add(_obj, InventoryObj.Container.Items[i]);
        }

    }

    void RemoveSlot(InventoryItemSlot _inventroyItemSlot)
    {
        for (int i = 0; i < InventoryObj.Container.Items.Count; i++)
        {
            if (InventoryObj.Container.Items[i].ID == _inventroyItemSlot.ID)
            {

            }
        }
    }

    public void CreateSlots() //added 17.06
    {
        itemsInSlotsDisplayed = new Dictionary<GameObject, InventoryItemSlot>();

        for (int i = 0; i < InventoryObj.Container.Items.Count; i++)
        {
            var _obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            itemsInSlotsDisplayed.Add(_obj, InventoryObj.Container.Items[i]);
        }
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventoryItemSlot> _slot in itemsInSlotsDisplayed)
        {
            if(_slot.Value.ID >= 0)
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = InventoryObj.Database.GetItem[_slot.Value.Item.ID].ItemSprite;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.Amount == 1 ? "" : _slot.Value.Amount.ToString("n0");
            }
            else
            {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "" ;
            }
        }
    }

    private void AddEvent(GameObject _obj, EventTriggerType type, UnityAction<BaseEventData> _action)
    {
        EventTrigger _trigger = _obj.GetComponent<EventTrigger>();
        var _eventTrigger = new EventTrigger.Entry();
        _eventTrigger.eventID = type;
        _eventTrigger.callback.AddListener(_action);
        _trigger.triggers.Add(_eventTrigger);
    }

    public void OnEnter(GameObject _obj)
    {
        MouseItemInstance._hoverObj = _obj;
        if (itemsDisplayed.ContainsValue(_obj))
            MouseItemInstance._hoverItem = itemsInSlotsDisplayed[_obj];
    }
    public void OnExit(GameObject _obj)
    {
        MouseItemInstance._hoverObj = null;
        MouseItemInstance._hoverItem = null;
    }
    public void OnDragStart(GameObject _obj, InventoryItemSlot _inventoryItemSlot)
    {
        //print("Drag started");
        var _mouseObj = new GameObject();
        var _rt = _mouseObj.AddComponent<RectTransform>();
        _rt.sizeDelta = _placeholderSpriteSize; //the same as img

        _parentAfterDrag = transform.parent;
        _mouseObj.transform.SetParent(transform.root);

        //if (itemsDisplayed[_obj].ID >= 0   //in the tutorial the reverted dictionary is used and he uses Array instead of List

        var _img = _mouseObj.AddComponent<Image>();
        _img.sprite = InventoryObj.Database.GetItem[_inventoryItemSlot.Item.ID].ItemSprite;
        _img.raycastTarget = false;

        MouseItemInstance._obj = _mouseObj;
        MouseItemInstance._item = _inventoryItemSlot;
    }
    public void OnDragEnd(GameObject _obj, InventoryItemSlot _inventoryItemSlot)
    {
        if (MouseItemInstance._hoverObj)
        {
            InventoryObj.MoveItem(itemsInSlotsDisplayed[_obj], itemsInSlotsDisplayed[MouseItemInstance._hoverObj]);
        }
        else
        {
            InventoryItemSlot _temInvSlot = _inventoryItemSlot;
            GameObject _tempItemObj = _obj;
            
            if (itemsInSlotsDisplayed[_obj].Amount == 1)
            {
                InventoryObj.RemoveItem(itemsInSlotsDisplayed[_obj].Item);
                itemsDisplayed.Remove(_inventoryItemSlot);
                itemsInSlotsDisplayed.Remove(_obj);
                Destroy(_obj);
            }
            else if (itemsInSlotsDisplayed[_obj].Amount > 1)
                InventoryObj.RemoveItem(itemsInSlotsDisplayed[_obj].Item);
        }
        Destroy(MouseItemInstance._obj);
        MouseItemInstance._item = null;
        
    }
    public void OnDrag(GameObject _obj)
    {
        if (MouseItemInstance._obj != null)
            MouseItemInstance._obj.GetComponent<RectTransform>().position = Input.mousePosition;
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        
        print(eventData.pointerCurrentRaycast.gameObject);
    }


    /*    public Vector3 GetPosition(int i)
        {
            return new Vector3(0, (-Y_SpaceBetweenItems * (i % numberOfColumns)), 0); //number of columns = items quantity
        }*/


}


public class MouseItem{
    public GameObject _obj;
    public InventoryItemSlot _item;
    public InventoryItemSlot _hoverItem;
    public GameObject _hoverObj;
}
