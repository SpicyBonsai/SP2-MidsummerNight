using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.AI;

public class DisplayMemories : MonoBehaviour
{
/*    private static DisplayMemories _instance;
    public static DisplayMemories Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DisplayMemories();
            return _instance;
        }
    }*/

    public MouseItem MouseItemInstance = new MouseItem();
    public GameObject InventoryPrefab;
    public InventoryObject InventoryObj;
    public Vector2 _placeholderSpriteSize = new Vector2(300, 100);

    Transform _parentAfterDrag;
    private int numberOfColumns;
    GameObject _instantiatedObj;
    GameObject _objToInsantiate;
    GameObject _panelUI;
    Color _panelUIcolor;

    Dictionary<GameObject, InventoryItemSlot> itemsInSlotsDisplayed = new Dictionary<GameObject, InventoryItemSlot>();

    public Dictionary<InventoryItemSlot, GameObject> itemsDisplayed = new Dictionary<InventoryItemSlot, GameObject>();

    private void Awake()
    {
        _panelUI = gameObject;
        _panelUIcolor = _panelUI.GetComponent<Image>().color;
    }
    void Start()
    {
        CreateDisplay();
    }

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
                //itemsDisplayed[_slot].GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount == 1 ? "" : _slot.Amount.ToString("n0");
                itemsDisplayed[_slot].transform.GetChild(0).GetComponentInChildren<Image>().sprite = InventoryObj.Database.GetItem[_slot.Item.ID].ItemSprite;
            }
            else
            {
                var _obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                _obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = InventoryObj.Database.GetItem[_slot.Item.ID].ItemSprite;
                //_obj.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount == 1 ? "" : _slot.Amount.ToString("n0");

                AddEvent(_obj, EventTriggerType.PointerEnter, delegate { OnEnter(_obj); });
                AddEvent(_obj, EventTriggerType.PointerExit, delegate { OnExit(_obj); });
                AddEvent(_obj, EventTriggerType.BeginDrag, delegate { OnDragStart(_obj, _slot); });
                AddEvent(_obj, EventTriggerType.EndDrag, delegate { OnDragEnd(_obj, _slot); });
                AddEvent(_obj, EventTriggerType.Drag, delegate { OnDrag(_obj); });

                itemsDisplayed.Add(_slot, _obj);
                itemsInSlotsDisplayed.Add(_obj, InventoryObj.Container.Items[i]);
/*                if (_panelUI == null)
                    _panelUI = itemsDisplayed[_slot].transform.parent.gameObject;
                print(_panelUI);*/
            }
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
            //_obj.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Amount == 1 ? "" : _slot.Amount.ToString("n0");

            AddEvent(_obj, EventTriggerType.PointerEnter, delegate { OnEnter(_obj); });
            AddEvent(_obj, EventTriggerType.PointerExit, delegate { OnExit(_obj); });
            AddEvent(_obj, EventTriggerType.BeginDrag, delegate { OnDragStart(_obj, _slot); });
            AddEvent(_obj, EventTriggerType.EndDrag, delegate { OnDragEnd(_obj, _slot); });
            AddEvent(_obj, EventTriggerType.Drag, delegate { OnDrag(_obj); });
            AddEvent(_obj, EventTriggerType.PointerClick, delegate { OnClick(_obj); });

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

        //hiding all UI elements with a lil fade out //not working properly
/*      _panelUI.GetComponent<Image>().CrossFadeAlpha(0, 0.2f, true);
        _panelUI.GetComponent<Image>().raycastTarget = false;
        _obj.GetComponent<Image>().CrossFadeAlpha(0, 0.2f, true);
        _obj.GetComponent<Image>().raycastTarget = false;
        _obj.transform.GetChild(0).GetComponent<Image>().CrossFadeAlpha(0, 0.2f, true);
        _obj.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;*/
        
        _panelUI.GetComponent<Image>().enabled = false;
        _obj.GetComponent<Image>().enabled = false;
        _obj.transform.GetChild(0).GetComponent<Image>().enabled = false;

        _parentAfterDrag = transform.parent;
        MouseItemInstance._gameObj = _mouseGameObj;
        MouseItemInstance._item = _inventoryItemSlot;
    }
    public void OnDragEnd(GameObject _obj, InventoryItemSlot _inventoryItemSlot)
    {
        string _itemName = _objToInsantiate.name;
        bool memoryActivated = false; //is true when player puts the memory into proper place
        var hoveredObj = MouseHover.HoveredObj;
        if (hoveredObj.name == _itemName) //&& !hoveredObj.GetComponent<MeshRenderer>().enabled) // && hoveredObj.transform.parent.GetComponent<BrokenObject>()._brokenParts[hoveredObj])
          {
            memoryActivated = true;
            //MouseItemInstance._gameObj.GetComponent<MeshCollider>().isTrigger = false; // the object won't be interactable anymore
            //MouseItemInstance._gameObj.AddComponent<NavMeshObstacle>();
            hoveredObj.GetComponent<MeshRenderer>().material = MouseItemInstance._gameObj.GetComponent<MeshRenderer>().material;
            //hoveredObj.transform.parent.GetComponent<BrokenObject>()._brokenParts[hoveredObj] = false;
            //hoveredObj.GetComponent<BrokenPart>().isFixed = true;
            //hoveredObj.GetComponent<MeshRenderer>().enabled = true;

          }
        else if (hoveredObj.name != _itemName) // || hoveredObj.GetComponent<MeshRenderer>().enabled)
        {
           // _panelUI.GetComponent<Image>().color = _panelUIcolor;
/*            _panelUI.GetComponent<Image>().raycastTarget = true;
            _obj.GetComponent<Image>().color = new Color(255, 255, 255, 20);
            _obj.GetComponent<Image>().raycastTarget = true;
            _obj.transform.GetChild(0).GetComponent<Image>().color = new Color(255, 255, 255, 255);
            _obj.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
*/
            print("we're here");
            memoryActivated = false;
        }
        print("we're here");
        _panelUI.GetComponent<Image>().enabled = true;
        _obj.GetComponent<Image>().enabled = true;
        _obj.transform.GetChild(0).GetComponent<Image>().enabled = true;
        Destroy(MouseItemInstance._obj);
        Destroy(MouseItemInstance._gameObj);
        MouseItemInstance._item = null;
        if(memoryActivated)
            gameObject.SetActive(false);
    }
    public void OnDrag(GameObject _obj)
    {

        //moving 3D object along with the cursor
        MouseItemInstance._gameObj.SetActive(true);
        Vector3 _screenPoint = Input.mousePosition;
        _screenPoint.z = 5.84f;
        Vector3 _instantiatePos = Camera.main.ScreenToWorldPoint(_screenPoint);
        MouseItemInstance._gameObj.transform.position = _instantiatePos;
        /*        if (MouseHover.CursorIsOverUI)
                {
                    MouseItemInstance._obj.SetActive(true);
                    MouseItemInstance._gameObj.SetActive(false);
                }
                else
                {
                    MouseItemInstance._obj.SetActive(false);
                    MouseItemInstance._gameObj.SetActive(true);
                }*/
    }

    public void OnClick(GameObject _obj)
    {
        print("clicked on memory");
    }
}

