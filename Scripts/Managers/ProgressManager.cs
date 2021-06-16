using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class DataToSave
{
    public int Chapter;
}
[System.Serializable]
public class SaveGameData
{
    public string Description;
}
[System.Serializable]
public class CheckPointData
{
    public int CheckPoint;
}
public class ProgressManager : MonoBehaviour
{
    [Header("Options")]
    public bool ProgressiveMenu;

    [Header("Panels")]
    GameObject[] SavedGame;

    [Header("Progress")]
    [SerializeField] int Chapter;
    [SerializeField] int CheckPoint;

    void Awake()
    {
        if (ProgressiveMenu)
        {
            LoadProgress();
        }
        //Static for now. Also creates one save file on start for testing purposes.
        SavedGame = new GameObject[3];
        SavedGame = GameObject.FindGameObjectsWithTag("SavedGamePanel");
        UpdateMenu();
    }
    public int GetCheckPointInfo()
    {
        return CheckPoint;
    }
    void LoadProgress()
    {
       if(File.Exists(Application.persistentDataPath + "/" +"MenuProgress" + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "MenuProgress" + ".dat", FileMode.Open);
            DataToSave datatoload = new DataToSave();
            datatoload = bf.Deserialize(file) as DataToSave;
            Chapter = datatoload.Chapter;
        }
        if (File.Exists(Application.persistentDataPath + "/" + "CheckPoint" + ".dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "CheckPoint" + ".dat", FileMode.Open);
            CheckPointData datatoload = new CheckPointData();
            datatoload = bf.Deserialize(file) as CheckPointData;
            CheckPoint = datatoload.CheckPoint;
        }
    }
    void UpdateMenu()
    {
        for(int i = 0; i < SavedGame.Length; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/" + "SavedGame" + "/" + "Save" + i + ".dat"))
            {
                SaveGameData s = new SaveGameData();
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fStream = File.Open(Application.persistentDataPath + "/" + "SavedGame" + "/" + "Save" + i + ".dat", FileMode.Open);
                s = bf.Deserialize(fStream) as SaveGameData;
                SavedGame[i].transform.GetChild(2).GetComponent<Text>().text = s.Description; 
            }
            else
            {
                SavedGame[i].transform.GetChild(2).GetComponent<Text>().text = "Free Slot";
            }
        }
    }
    void SaveCheckPoint()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + "CheckPoint" + ".dat");
        CheckPointData datatosave = new CheckPointData();
        datatosave.CheckPoint = new int();
        datatosave.CheckPoint = CheckPoint;
        bf.Serialize(file, datatosave);
        file.Close();
    }
    public int CheckProgress()
    {
        return Chapter;
        //set bg
        //music etc..
    }
    void Update()
    {
        
    }
}
