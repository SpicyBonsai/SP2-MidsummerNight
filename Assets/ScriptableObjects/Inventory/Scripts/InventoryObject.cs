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
        Container.Items.Add(new InventoryItemSlot(_item.ID, _item, _amount));
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

        string _saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter _bf = new BinaryFormatter();
        FileStream _file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        _bf.Serialize(_file, _saveData);
        _file.Close();
        Debug.Log(string.Concat(Application.persistentDataPath, savePath));


        // Use this to prevent changing game save file data

        /*        IFormatter _formatter = new BinaryFormatter();
                Stream _stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
                _formatter.Serialize(_stream, Container);
                _stream.Close();*/
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter _bf = new BinaryFormatter();
            FileStream _file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(_bf.Deserialize(_file).ToString(), this);
            _file.Close();

            //Protected Save file
            /*            IFormatter _formatter = new BinaryFormatter();
                        Stream _stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
                        Container = (Inventory)_formatter.Deserialize(_stream);
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
    public void AddAmount(int _value)
    {
        Amount += _value;
    }
}