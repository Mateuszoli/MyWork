using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveGameEvent : MonoBehaviour
{
    [Header("Data to save")]
    [SerializeField] string Description;

    [Header("External")]
    [SerializeField] ProgressManager progressmanager;
    [SerializeField] bool Saving;

    void Start()
    {
        progressmanager = FindObjectOfType<ProgressManager>();
        Description = "Test";
        SaveGame(Description);
    }
    void SaveGame(string Desc)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if(!Directory.Exists(Application.persistentDataPath + "/" + "SavedGame"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + "SavedGame");
        }
        FileStream file = File.Create(Application.persistentDataPath + "/" + "SavedGame" + "/" + "Save" + progressmanager.GetCheckPointInfo() + ".dat");
        SaveGameData datatosave = new SaveGameData();
        Saving = true;  //for loading bar
        datatosave.Description = ""; //Constructor
        datatosave.Description = Desc;
        bf.Serialize(file, datatosave);
        file.Close();
    }

}
