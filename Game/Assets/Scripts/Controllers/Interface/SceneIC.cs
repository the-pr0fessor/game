using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneIC : InterfaceController {
    ViewController viewController;
    DCLevel dialogueController;
    LifeController lifeController;

    bool showingDialogue;

    protected override void Awake()
    {
        base.Awake();
        viewController = GameObject.FindGameObjectWithTag("Controllers").GetComponent<ViewController>();
        dialogueController = GameObject.FindGameObjectWithTag("Controllers").GetComponent<DCLevel>();
        lifeController = GameObject.FindGameObjectWithTag("Controllers").GetComponent<LifeController>();
    }

    public override bool ShowingUI()
    {
        if (showingDialogBox)
        {
            Debug.Log("DB");
        }

        if (showingMenu)
        {
            Debug.Log("M");
        }

        if (showingSubMenu)
        {
            Debug.Log("Sm");
        }

        return base.ShowingUI() || showingDialogue;
    }

    public override void HideMenu()
    {
        base.HideMenu();

        if (viewController == null)
        {
            viewController = GameObject.FindGameObjectWithTag("Controllers").GetComponent<ViewController>();
        }

        if (viewController.IsTopDown())
        {
            Cursor.visible = true;
        }
    }

    public void StartShowingDialogue()
    {
        showingDialogue = true;
    }

    public void StopShowingDialogue()
    {
        showingDialogue = false;
    }

    public void LoadMainMenu()
    {
        LevelController.levelController.LoadMainMenu();
    }

    public void RestartLevel()
    {
        dialogueController = GameObject.FindGameObjectWithTag("Controllers").GetComponent<DCLevel>();
        if (!dialogueController.Displaying())
        {
            lifeController = GameObject.FindGameObjectWithTag("Controllers").GetComponent<LifeController>();
            HideMenu();
            lifeController.RestartLevel();
        }     
    }
}
