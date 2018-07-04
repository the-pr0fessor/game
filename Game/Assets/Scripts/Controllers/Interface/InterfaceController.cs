using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class InterfaceController : MonoBehaviour {
    protected bool showingMenu;
    protected bool showingSubMenu;
    protected bool showingDialogBox;
    protected Canvas menuCanvas;


    protected virtual void Awake()
    {
        showingMenu = false;
        showingSubMenu = false;
        showingDialogBox = false;
        Cursor.visible = false;
        menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
        menuCanvas.enabled = false;
        //dialogBoxCanvas.enabled = false;
        //notificationCanvas.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!showingMenu)
            {
                ShowMenu();
            }
            else if (showingMenu)
            {
                HideMenu();                
            }                
        }       
    }

    public virtual bool ShowingUI()
    {
        return showingMenu || showingSubMenu || showingDialogBox;
    }
    
    public virtual void HideMenu()
    {
        showingMenu = false;

        if (menuCanvas == null)
        {
            menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
        }
        menuCanvas.enabled = false;
        if (!showingSubMenu)
        {
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }

    public void ShowMenu()
    {
        Time.timeScale = 0;
        showingMenu = true;
        menuCanvas.enabled = true;
        Cursor.visible = true;
    }

    public void ShowCanvas(Canvas canvas)
    {        
        showingSubMenu = true;
        HideMenu();
        canvas.enabled = true;        
    }

    public void HideCanvas(Canvas canvas)
    {
        canvas.enabled = false;
        ShowMenu();
        showingSubMenu = false;
    }

    public void ShowDialogBox(Canvas dialogBox)
    {
        showingDialogBox = true;
        dialogBox.enabled = true;
    }

    public void HideDialogBox(Canvas dialogBox)
    {
        dialogBox.enabled = false;
        showingDialogBox = false;
    }

    public void ShowOptions()
    {
        Debug.Log("options");
    }


    public void ExitProgram()
    {
        Application.Quit();
    }
}