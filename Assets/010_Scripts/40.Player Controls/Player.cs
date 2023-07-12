using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject ItemInventory;
    public InventoryObject MemoryInventory;
    public GameObject MemoryUI;

    string _guiText;

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     _guiText = "File saved";
        //     ItemInventory.Save();
        // }

        // if (Input.GetKeyDown(KeyCode.L))
        //     ItemInventory.Load();

        // if (Input.GetKeyDown(KeyCode.M) && !MemoryUI.activeInHierarchy)
        // {
        //     MemoryUI.SetActive(true);
        // }
        // else if (Input.GetKeyDown(KeyCode.M) && MemoryUI.activeInHierarchy)
        // {
        //     MemoryUI.SetActive(false);
        // }

    }

    public void OnTriggerEnter(Collider other)
    {
        var _item = other.GetComponent<GroundItem>();
        var _memoryFragment = other.GetComponent<MemoryFragment>();

        if (_item)
        {
            print(other);
            AudioManager.Instance.RandomSoundEffect(AudioManager.Instance.collectItem);
            ItemInventory.AddItem(new Item(_item.ItemObj), 1);
            Destroy(other.gameObject);
        }
        else if (_memoryFragment)
        {
            MemoryInventory.AddItem(new Item(_memoryFragment.ItemObj), 1);
            AudioManager.Instance.RandomSoundEffect(AudioManager.Instance.collectMemory);
            DialogueInitiator dialogueInitiator = other.GetComponent<DialogueInitiator>();
            dialogueInitiator?.Interact();
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
