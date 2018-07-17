using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DCDialgoue : DialogueController {

    public float flashTime;
    protected Image background;
    int loading; // 0 = not loading, 1 = starting, 2 = finishing
    CameraControllerDialogue cameraController;
    
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
    }

    protected override void Start()
    {
        base.Start();
        cameraController.SetIntensity(1.0f);
    }

    protected override void Awake()
    {
        base.Awake();
        background = GameObject.FindGameObjectWithTag("Image").GetComponent<Image>();
        cameraController = GameObject.FindGameObjectWithTag("Controllers").GetComponent<CameraControllerDialogue>();
        loading = 1;        
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
        entryNo++;

        // Change the background
        if (character == "background")
        {
            background.sprite = Resources.Load<Sprite>("Backgrounds/" + currentTextArr[0]);
            done = true;
            currentTextArr.Clear();
            GetNextDialogue(); // Get the next actual text
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
    }
}
