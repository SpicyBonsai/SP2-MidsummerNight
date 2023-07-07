using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataSave : MonoBehaviour
{
    public List<InventoryObject> Inventories;

    private static DataSave _instance;
    public static DataSave Instance
    {
        get
        {
            if (_instance == null)
                _instance = new DataSave();
            return _instance;
        }
    }
    public string savePath;


    [ContextMenu("Save")]
    public void Save()
    {
        for (int i = 0; i < Inventories.Count; i++)
        {
            //JSON formatted save file

            string _saveData = JsonUtility.ToJson(Inventories[i], true);
            BinaryFormatter _bf = new BinaryFormatter();
            FileStream _file = File.Create(string.Concat(Application.persistentDataPath, savePath));
            _bf.Serialize(_file, _saveData);
            _file.Close();
            Debug.Log(string.Concat(Application.persistentDataPath, savePath));
        }


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
            BinaryFormatter _bf = new BinaryFormatter();
            FileStream _file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(_bf.Deserialize(_file).ToString(), this);
            _file.Close();

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
}
