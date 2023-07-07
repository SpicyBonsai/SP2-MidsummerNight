using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject ItemInventory;
    public InventoryObject MemoryInventory;
    public GameObject MemoryUI;
    public GameObject WorldItemsUI;

    string _guiText;

    private void Start()
    {
        //MemoryUI = DisplayMemories.Instance.gameObject;
        //print(MemoryUI);
        //WorldItemsUI = DisplayInventory.Instance.gameObject;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _guiText = "File saved";
            ItemInventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
            ItemInventory.Load();
        #region UI controllers
        if (Input.GetKeyDown(KeyCode.M) && !MemoryUI.activeInHierarchy)
        {
            InputManager.GetInstance().SwitchToUI();
            MemoryUI.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.M) && MemoryUI.activeInHierarchy)
        {
            InputManager.GetInstance().SwitchToGameplay();
            MemoryUI.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.N) && !WorldItemsUI.activeInHierarchy)
        {
            InputManager.GetInstance().SwitchToUI();
            WorldItemsUI.SetActive(true);
            print("opening Panel");
        }
        else if (Input.GetKeyDown(KeyCode.N) && WorldItemsUI.activeInHierarchy)
        {
            InputManager.GetInstance().SwitchToGameplay();
            WorldItemsUI.SetActive(false);
            print("closing dat shit");
        }
        #endregion
        print(InputManager.GetInstance());
        /*
                if(MouseHover.HoveredObj.GetComponent<IUIInterface>() )
                    InputManager.GetInstance().SwitchToUI();
                else
                    InputManager.GetInstance().SwitchToGameplay();*/

/*        if (MouseHover.CursorIsOverUI)
            InputManager.GetInstance().SwitchToUI();
        else
            InputManager.GetInstance().SwitchToGameplay();*/

    }

    public void OnTriggerEnter(Collider other)
    {
        var _item = other.GetComponent<GroundItem>();
        var _memoryFragment = other.GetComponent<MemoryFragment>();

        if (_item)
        {
            print(other);
            ItemInventory.AddItem(new Item(_item.ItemObj), 1);
            Destroy(other.gameObject);
        }
        else if (_memoryFragment)
        {
            MemoryInventory.AddItem(new Item(_memoryFragment.ItemObj), 1);
            //Destroy(other.gameObject.transform.parent.gameObject);
            Destroy(other.gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        ItemInventory.Container.Items.Clear();
        MemoryInventory.Container.Items.Clear();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 50, 20), _guiText);
    }

}
