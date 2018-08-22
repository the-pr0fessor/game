using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DCDialgoue : DialogueController {
    public Canvas choiceCanvas;
    public Text option1Text;
    public Text option2Text;

    public float flashTime;
    protected Image background;
    int loading; // 0 = not loading, 1 = starting, 2 = finishing
    int changing; // changing background, same as above
    string backgroundAddress;
    string choice;
    int choiceNo;


    CameraControllerDialogue cameraController;
    
    // Screen flashing and stuff
    protected override void Update()
    {
        base.Update();

        if (loading == 1)
        {
            cameraController.SetIntensity(cameraController.GetIntensity() - Time.deltaTime / (3 * flashTime));
            if (cameraController.GetIntensity() <= 0)
            {
                cameraController.SetIntensity(0);
                loading = 0;
                StartShowingDialogue();
            }
        }

        if (loading == 2)
        {
            cameraController.SetIntensity(cameraController.GetIntensity() + Time.deltaTime / flashTime);

            if (cameraController.GetIntensity() >= 1)
            {
                LevelController.levelController.LoadNextLevel();
            }
        }

        if (changing == 1)
        {
            cameraController.SetIntensity(cameraController.GetIntensity() + Time.deltaTime / (flashTime*3));

            if (cameraController.GetIntensity() >= 1)
            {
                cameraController.SetIntensity(1.0f);
                changing = 2;

                background.sprite = Resources.Load<Sprite>(backgroundAddress);
            }
        }

        if (changing == 2)
        {
            cameraController.SetIntensity(cameraController.GetIntensity() - Time.deltaTime / (flashTime * 3));

            if (cameraController.GetIntensity() <= 0)
            {
                cameraController.SetIntensity(0);
                changing = 0;

                done = true;
                currentTextArr.Clear();
                GetNextDialogue(); // Get the next actual text
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        cameraController.SetIntensity(1.0f);
        choiceCanvas.enabled = false;
    }

    protected override void Awake()
    {
        base.Awake();
        background = GameObject.FindGameObjectWithTag("Image").GetComponent<Image>();
        cameraController = GameObject.FindGameObjectWithTag("Controllers").GetComponent<CameraControllerDialogue>();
        loading = 1;
        changing = 0;
    }

    protected override void Finish()
    {
        base.Finish();
        loading = 2;        
    }


    // Can also change the background, character sprites, etc. not needed in levels
    public override void GetNextDialogue()
    {
        currentTextArr = new List<string>(dr.dialogueEntries[entryNo].text.Split(' '));
        string character = dr.dialogueEntries[entryNo].character;
        

        // Change the background
        if (character == "background")
        {
            backgroundAddress = "Backgrounds/" + currentTextArr[0];
            changing = 1;
            smallText.text = "";
            nameText.text = "";

            //background.sprite = Resources.Load<Sprite>("Backgrounds/" + currentTextArr[0]);
            //done = true;
            //currentTextArr.Clear();
            //GetNextDialogue(); // Get the next actual text
        }
        // A choice
        else if (character == "choice")
        {
            dialogueCanvasImage.enabled = false;
            smallText.enabled = false;
            nameText.enabled = false;
            choiceCanvas.enabled = true;
            noMoreDialogue = true;

            choice = dr.dialogueEntries[entryNo].text.Split('^')[0];
            option1Text.text = dr.dialogueEntries[entryNo].text.Split('^')[1];
            option2Text.text = dr.dialogueEntries[entryNo].text.Split('^')[2];
        }
        // Dialogue that depends on a previous choice
        else if (character == "depend")
        {
            choiceNo = LevelController.levelController.currentSave.GetChoice(currentTextArr[0]);
            forceNext = true;
        }
        // Get the right dialogue for the choice
        else if (character == "option")
        {            
            // If this wasn't the choice made, skip ahead to next block
            if (choiceNo.ToString() != currentTextArr[0])
            {
                

                if (dr.dialogueEntries[entryNo].text != "end")
                {
                    entryNo++;

                    // While there's still text in the block
                    while (dr.dialogueEntries[entryNo].character != "option")
                    {
                        entryNo++;
                    }
                }

                
            }

            forceNext = true;
        }
        // Normal dialogue
        else
        {
            PutSpacesBack();
            smallText.text = "";
            nameText.text = character;
            done = false;
            timer = 0;
            wordNo = 0;
            previousSmallText = "";
        }

        entryNo++;
    }

    public void ClickOption(int optionNo)
    {
        dialogueCanvasImage.enabled = true;
        smallText.enabled = true;
        nameText.enabled = true;
        choiceCanvas.enabled = false;
        LevelController.levelController.currentSave.SetChoice(choice, optionNo);
        noMoreDialogue = false;
        forceNext = true;
    }
}
