using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.AI;

public class DisplayInventory : MonoBehaviour
{
    public MouseItem MouseItemInstance = new MouseItem();
    public GameObject InventoryPrefab;
    public InventoryObject InventoryObj;
    public Vector2 _placeholderSpriteSize = new Vector2(300, 100);

    Transform _parentAfterDrag;
    private int numberOfColumns;
    GameObject _instantiatedObj;
    GameObject _objToInsantiate;
    // bool _3DcursorInstance = false; //to instantiate 3D object at cursor pos. only once
    // bool _conditionToRemoveItem = true;

    //Dictionary<InventoryItemSlot, GameObject> itemsDisplayed = new Dictionary<InventoryItemSlot, GameObject>();
    Dictionary<GameObject, InventoryItemSlot> itemsInSlotsDisplayed = new Dictionary<GameObject, InventoryItemSlot>();

    public Dictionary<InventoryItemSlot, GameObject> itemsDisplayed = new Dictionary<InventoryItemSlot, GameObject>();

    //Dictionary<InventoryItemSlot, GameObject> _instantiatedObjects = new Dictionary<InventoryItemSlot, GameObject>();
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
        //print(MouseItemInstance._obj);
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
                //_instantiatedObjects.Add(_slot, null);
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
    #region Added 17.06 for static inventory
    /*    public void CreateSlots() 
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
        }*/
    #endregion

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
        //creating img holder
        var _mouseObj = new GameObject();
        _mouseObj.name = "ItemSpriteHolder";
        var _rt = _mouseObj.AddComponent<RectTransform>();
        _rt.sizeDelta = _placeholderSpriteSize; //the same as img

        //creating obj holder
        _objToInsantiate = InventoryObj.Database.GetItem[itemsInSlotsDisplayed[_obj].Item.ID].ItemPrefab;
        var _mouseGameObj = new GameObject();
        _mouseGameObj.name = "ItemObjectHolder";
        _mouseGameObj.transform.localScale = _objToInsantiate.transform.localScale;
        _mouseGameObj.transform.eulerAngles = _objToInsantiate.transform.eulerAngles;
        var _meshFilter = _mouseGameObj.AddComponent<MeshFilter>();
        _meshFilter.mesh = _objToInsantiate.GetComponent<MeshFilter>().sharedMesh;
        var _meshRenderer = _mouseGameObj.AddComponent<MeshRenderer>();
        _meshRenderer.materials = _objToInsantiate.GetComponent<MeshRenderer>().sharedMaterials;
        //var _collider = _mouseGameObj.AddComponent<MeshCollider>();
        //_collider.sharedMesh = _objToInsantiate.GetComponent<MeshCollider>().sharedMesh;
        //_collider.convex = true;


        _parentAfterDrag = transform.parent;
        _mouseObj.transform.SetParent(transform.root);

        //if (itemsDisplayed[_obj].ID >= 0   //in the tutorial the reverted dictionary is used and he uses Array instead of List

        var _img = _mouseObj.AddComponent<Image>();
        _img.sprite = InventoryObj.Database.GetItem[_inventoryItemSlot.Item.ID].ItemSprite;
        _img.raycastTarget = false;

        MouseItemInstance._obj = _mouseObj;
        MouseItemInstance._gameObj = _mouseGameObj;
        MouseItemInstance._item = _inventoryItemSlot;
    }
    public void OnDragEnd(GameObject _obj, InventoryItemSlot _inventoryItemSlot)
    {
        string _itemName = _objToInsantiate.name;

        if (MouseItemInstance._hoverObj)
        {
            InventoryObj.MoveItem(itemsInSlotsDisplayed[_obj], itemsInSlotsDisplayed[MouseItemInstance._hoverObj]);
        }
        else if (!MouseItemInstance._hoverObj)
        {
            var hoveredObj = MouseHover.HoveredObj;
            if (hoveredObj?.name == _itemName && !hoveredObj.GetComponent<MeshRenderer>().enabled) // && hoveredObj.transform.parent.GetComponent<BrokenObject>()._brokenParts[hoveredObj])
            {

                //MouseItemInstance._gameObj.GetComponent<MeshCollider>().isTrigger = false; // the object won't be interactable anymore
                //MouseItemInstance._gameObj.AddComponent<NavMeshObstacle>();

                //hoveredObj.transform.parent.GetComponent<BrokenObject>()._brokenParts[hoveredObj] = false;
                //hoveredObj.GetComponent<BrokenPart>().isFixed = true;
                if (itemsInSlotsDisplayed[_obj].Amount == 1)
                {
                    InventoryObj.RemoveItem(itemsInSlotsDisplayed[_obj].Item);
                    itemsDisplayed.Remove(_inventoryItemSlot);
                    itemsInSlotsDisplayed.Remove(_obj);
                    Destroy(_obj);
                }
                else if (itemsInSlotsDisplayed[_obj].Amount > 1)
                    InventoryObj.RemoveItem(itemsInSlotsDisplayed[_obj].Item);

                hoveredObj.GetComponent<MeshRenderer>().enabled = true;
                
            }
        }

        Destroy(MouseItemInstance._obj);
        Destroy(MouseItemInstance._gameObj);
        MouseItemInstance._item = null;
    }
    public void OnDrag(GameObject _obj)
    {
        if (MouseItemInstance._obj != null)
            MouseItemInstance._obj.GetComponent<RectTransform>().position = InputManager.GetInstance().MousePosition;

        Vector3 _screenPoint = InputManager.GetInstance().MousePosition;
        //_screenPoint.z = 5.84f;
        _screenPoint.z = 5.84f;
        Vector3 _instantiatePos = Camera.main.ScreenToWorldPoint(_screenPoint);
        if (MouseItemInstance._gameObj != null)
            MouseItemInstance._gameObj.transform.position = _instantiatePos;

        if (MouseHover.CursorIsOverUI)
        {
            MouseItemInstance._obj.SetActive(true);
            MouseItemInstance._gameObj.SetActive(false);
        }
        else
        {
            MouseItemInstance._obj.SetActive(false);
            MouseItemInstance._gameObj.SetActive(true);
        }
    }


    /*    public Vector3 GetPosition(int i)
        {
            return new Vector3(0, (-Y_SpaceBetweenItems * (i % numberOfColumns)), 0); //number of columns = items quantity
        }*/


}


public class MouseItem
{
    public GameObject _obj;
    public GameObject _gameObj;
    public InventoryItemSlot _item;
    public InventoryItemSlot _hoverItem;
    public GameObject _hoverObj;
}

/*class MappedValue
{
    public GameObject _
}*/
