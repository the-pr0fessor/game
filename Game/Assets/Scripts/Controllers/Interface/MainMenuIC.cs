using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuIC : InterfaceController {
    public Canvas newCanvas;
    public Canvas overwriteCanvas;
    public Canvas loadCanvas;
    public Canvas noSaveCanvas;
    public Canvas saveCorruptedCanvas;

    int slotNo;

    protected override void Awake()
    {
        menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
        showingMenu = true;
        //showingSubMenu = false;
        showingDialogBox = false;
        Cursor.visible = true;
        menuCanvas.enabled = true;
        newCanvas.enabled = false;
        overwriteCanvas.enabled = false;
        loadCanvas.enabled = false;
        noSaveCanvas.enabled = false;
        saveCorruptedCanvas.enabled = false;
}

    public void CreateNewSave(int newSlotNo)
    {
        slotNo = newSlotNo;
        if (LevelController.levelController.CreateNewSave(slotNo, false))
        {
            LoadSave(slotNo);
            return;
        }
        else
        {
            // Make dialog box to ask if want to overwrite save
            ShowDialogBox(overwriteCanvas);
        }        
    }


    public void OverwriteYes()
    {
        LevelController.levelController.CreateNewSave(slotNo, true);
        LoadSave(slotNo);
    }

    public void OverwriteNo()
    {
        HideDialogBox(overwriteCanvas);
        slotNo = 0;
    }

    public void LoadSave(int newSlotNo)
    {
        int result = LevelController.levelController.Load(newSlotNo);

        if (result == 1)
        {
            ShowDialogBox(noSaveCanvas);
        }       
        else if (result == 2)
        {
            ShowDialogBox(saveCorruptedCanvas);
        }
    }

    public void ShowNewCanvas()
    {
        menuCanvas.enabled = false;
        newCanvas.enabled = true;
        showingMenu = false;
        showingSubMenu = true;
    }

    public void ShowLoadCanvas()
    {
        menuCanvas.enabled = false;
        loadCanvas.enabled = true;
        showingMenu = false;
        showingSubMenu = true;
    }
}
