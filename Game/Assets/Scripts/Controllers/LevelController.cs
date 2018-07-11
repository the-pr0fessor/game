using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LevelController : MonoBehaviour {

    public static LevelController levelController;

    public bool editor;

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

        G gameData = new G();
        gameData.c = currentLevel;
        bf.Serialize(file, gameData);

        file.Close();
    }
	
    // 0 = successful load
    // 1 = file doesn't exist
    // 2 = file is corrupted
    public int Load(int newSaveNo)
    {
        if (File.Exists(GetPath(newSaveNo)))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(GetPath(newSaveNo), FileMode.Open);
                G gameData = (G)bf.Deserialize(file);
                file.Close();

                currentLevel = gameData.c;
                saveNo = newSaveNo;
                LoadLevel(currentLevel);
                return 0;
            }
            catch
            {
                return 2;
            }
        }
        return 1;
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
            if (overwrite)
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
        // Make the save file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Create(GetPath(newSaveNo));
        saveNo = newSaveNo;
        G gameData = new G();
        gameData.c = 1;
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
class G
{
    public int c;
}

[Serializable]
class GeneralData
{
    public int lastSlot;
}

