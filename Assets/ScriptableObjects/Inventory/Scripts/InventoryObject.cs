using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject//, ISerializationCallbackReceiver
{
    public string savePath; 
    public ItemDatabaseObject Database;
    public Inventory Container;

    public void AddItem(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            if (Container.Items[i].Item.ID == _item.ID)
            {
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }

        //commented 17.06
        Container.Items.Add(new InventoryItemSlot(_item.ID, _item, _amount));
        
        //17.06 added
        //SetEmptyFirstSlot(_item, _amount);

    }

    public void RemoveItem(Item _item)
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            if(Container.Items[i].Item.ID == _item.ID)
            {
                //Container.Items[i].UpdateItemSlot(-1, null, 0);
                
                Container.Items[i].RemoveItem();
                if(Container.Items[i].Amount < 1)
                {
                    Container.Items[i].UpdateItemSlot(-1, null, 0);
                    Container.Items.Remove(Container.Items[i]);
                }
                //return;
            }
        }
    }

    public void MoveItem(InventoryItemSlot _item1, InventoryItemSlot _item2)
    {
        //Debug.Log("Move Item");
        InventoryItemSlot _temp = new InventoryItemSlot(_item2.ID, _item2.Item, _item2.Amount);
        _item2.UpdateItemSlot(_item1.ID, _item1.Item, _item1.Amount);
        _item1.UpdateItemSlot(_temp.ID, _temp.Item, _temp.Amount);
    }

    public InventoryItemSlot SetEmptyFirstSlot(Item _item, int _amount) //added 17.06
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            if(Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateItemSlot(_item.ID, _item, _amount);
                return Container.Items[i];
            }    
        }

        //set up for full inventory (if using Array)
        return null;
    }

    //implement for using JSON format
/*    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            Container.Items[i].Item = Database.GetItem[Container.Items[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }*/

    [ContextMenu("Save")]
    public void Save()
    {
        //JSON formatted save file

        /*        string _saveData = JsonUtility.ToJson(this, true);
                BinaryFormatter _bf = new BinaryFormatter();
                FileStream _file = File.Create(string.Concat(Application.persistentDataPath, savePath));
                _bf.Serialize(_file, _saveData);
                _file.Close();
                Debug.Log(string.Concat(Application.persistentDataPath, savePath));*/


        // Use this to prevent changing game save file data

/*        IFormatter _formatter = new BinaryFormatter();
        Stream _stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        _formatter.Serialize(_stream, Container);
        _stream.Close();

        Debug.Log(string.Concat(Application.persistentDataPath, savePath));*/
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            /*            BinaryFormatter _bf = new BinaryFormatter();
                        FileStream _file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
                        JsonUtility.FromJsonOverwrite(_bf.Deserialize(_file).ToString(), this);
                        _file.Close();*/

            //Protected Save file
/*            IFormatter _formatter = new BinaryFormatter();
            Stream _stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory _newContainer = (Inventory)_formatter.Deserialize(_stream);
            for (int i = 0; i < Container.Items.Count; i++)
            {
                _newContainer.Items[i].UpdateItemSlot(_newContainer.Items[i].ID, _newContainer.Items[i].Item, _newContainer.Items[i].Amount);
            }
            _stream.Close();*/
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }
}

[System.Serializable]
public class Inventory{
    public List<InventoryItemSlot> Items = new List<InventoryItemSlot>();
    //public InventoryItemSlot[] Items = new InventoryItemSlot[10];
}

[System.Serializable]
public class InventoryItemSlot
{
    public int ID;
    public Item Item;
    public int Amount;
    public InventoryItemSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        Item = _item;
        Amount = _amount;
    }
    public void UpdateItemSlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        Item = _item;
        Amount = _amount;
    }

    public InventoryItemSlot() //added 17.06
    {
        ID = -1;
        Item = null;
        Amount = 0;
    }
    public void AddAmount(int _value)
    {
        Amount += _value;
    }

    public void RemoveItem()
    {
        Amount -= 1;
    }
}