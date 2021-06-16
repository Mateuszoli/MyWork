using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ProgressModifier : MonoBehaviour
{
    [Header("Data to save")]
    [SerializeField] int Chapter;

    [Header("External")]
    [SerializeField] bool Saving;

    public void SaveProgress(int ch)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + "MenuProgress" + ".dat");
        DataToSave datatosave = new DataToSave();
        Saving = true;
        datatosave.Chapter = new int();
        datatosave.Chapter = ch;
        bf.Serialize(file, datatosave);
        file.Close();
    }

}
