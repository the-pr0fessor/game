using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelController : MonoBehaviour {

    public static LevelController levelController;

    public int currentLevel;

    string fileName = "save";
    string fileFormat = ".sav";

    int saveNo;

    private void Awake()
    {
        // Make sure only one level controller
        if (levelController == null)
        {
            DontDestroyOnLoad(gameObject);
            levelController = this;
        }
        else if (levelController != this)
        {
            Destroy(gameObject);
        }
    }


    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file;

        if (!File.Exists(GetPath(saveNo)))
        {
            file = File.Create(GetPath(saveNo));
        }
        else
        {
            file = File.Open(GetPath(saveNo), FileMode.Open);
        }

        GameData gameData = new GameData();
        gameData.currentLevel = currentLevel;
        gameData.slotUsed = true;

        bf.Serialize(file, gameData);

        file.Close();
    }
	

    public bool Load(int newSaveNo)
    {
        if (File.Exists(GetPath(newSaveNo)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GetPath(newSaveNo), FileMode.Open);
            GameData gameData = (GameData) bf.Deserialize(file);
            file.Close();

            currentLevel = gameData.currentLevel;
            saveNo = newSaveNo;
            LoadLevel(currentLevel);
            return true;
        }
        return false;
    }

    // Return false if save already exists, call again with overwrite true to overwrite the save
    public bool CreateNewSave(int newSaveNo, bool overwrite)
    {
        if (!File.Exists(GetPath(newSaveNo)))
        {
            WriteNewSave(newSaveNo);            
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            file = File.Open(GetPath(newSaveNo), FileMode.Open);
            GameData gameData = (GameData)bf.Deserialize(file);
            file.Close();

            if (!gameData.slotUsed || overwrite)
            {
                WriteNewSave(newSaveNo);
            }
            else
            {
                return false;
            }            
        }

        return true;
    }

    void WriteNewSave(int newSaveNo)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Create(GetPath(newSaveNo));
        saveNo = newSaveNo;
        GameData gameData = new GameData();
        gameData.slotUsed = true;
        gameData.currentLevel = 1;
        bf.Serialize(file, gameData);
        file.Close();
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        Save();

        if (Application.CanStreamedLevelBeLoaded(currentLevel))
        {
            LoadLevel(currentLevel);
        }
        else
        {
            LoadMainMenu();
        }
        
    }

    string GetPath(int no)
    {
        return Application.persistentDataPath + @"/" + fileName + no.ToString() + fileFormat;
    }

	// Update is called once per frame
	void Update () {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        saveNo = 0;
        currentLevel = 0;
        SceneManager.LoadScene(0, LoadSceneMode.Single);        
    }

    public void LoadLevel(int levelNo)
    {
        //SceneManager.LoadScene("Level " + levelNo.ToString(), LoadSceneMode.Single);
        SceneManager.LoadScene(levelNo, LoadSceneMode.Single);
    }
}

[Serializable]
class GameData
{
    public int currentLevel;
    public bool slotUsed;
}

[Serializable]
class GeneralData
{
    public int lastSlot;
}

