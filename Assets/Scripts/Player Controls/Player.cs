using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject Inventory;

    string _guiText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _guiText = "File saved";
            Inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
            Inventory.Load();

    }

    public void OnTriggerEnter(Collider other)
    {
        var _item = other.GetComponent<GroundItem>();
        if (_item)
        {
            Inventory.AddItem(new Item(_item.ItemObj), 1);
            Destroy(other.gameObject);
        }
    }
    private void OnApplicationQuit()
    {
        Inventory.Container.Items.Clear();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 50, 20), _guiText);
    }

}
