using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : MonoBehaviour {
    public float flashTime;
    public bool switchLimit = false;
    public int maxNoOfSwitches = 0;
    int switchCount;

    PlatformController platformController;
    GameObject player;
    Movement playerMovement;
    Vector3 initialPosition;
    Vector3 initialForward;
    CameraController cameraController;    
    InterfaceController ic;
    Canvas switchCountCanvas;
    Text switchCountText;

    int resetting; // 0 = doing nothing, 1 = before moving, 2 = after moving
    int starting; // 1 = starting, 0 = not starting;
    bool fullReset;

    // Use this for initialization
    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Movement>();
        cameraController = GameObject.FindGameObjectWithTag("Controllers").GetComponent<CameraController>();
        platformController = GameObject.FindGameObjectWithTag("Platform Controller").GetComponent<PlatformController>();
        ic = GetComponent<InterfaceController>();

        if (switchLimit)
        {
            switchCountCanvas = GameObject.FindGameObjectWithTag("SwitchCountCanvas").GetComponentInChildren<Canvas>();
            switchCountText = GameObject.FindGameObjectWithTag("SwitchCountCanvas").GetComponentInChildren<Text>();
        }
    }

    private void Start()
    {
        initialPosition = player.transform.position;
        initialForward = player.transform.forward;
        switchCount = 0;

        resetting = 0;
        starting = 1;
        fullReset = false;
        cameraController.SetIntensity(1);

        
        if (switchLimit)
        {
            switchCountText.text = maxNoOfSwitches.ToString();
            switchCountCanvas.enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {

        // If limit on number of switches, decrement counter on switch
        if (Input.GetKeyDown("left ctrl") && !ic.ShowingUI() && switchLimit)
        {
            switchCountCanvas.enabled = !switchCountCanvas.enabled;
            if (switchCount == maxNoOfSwitches)
            {
                switchCount = 0;
                playerMovement.alive = false;
                RestartLevel();
            }
            else
            {
                switchCount++;
                switchCountText.text = ((maxNoOfSwitches - switchCount)/2).ToString();
            }
        }

        if (!playerMovement.alive && resetting == 0 && !playerMovement.finished)
        {
            resetting = 1;
        }

        if (resetting == 1)
        {
            cameraController.SetIntensity(cameraController.GetIntensity() + Time.deltaTime / flashTime);

            if (cameraController.GetIntensity() >= 1)
            {
                playerMovement.Reset(initialPosition, initialForward, fullReset);
                player.GetComponent<CameraLook>().Reset();
                GetComponent<ViewController>().StartFirstPerson();         

                if (!playerMovement.easyMode || fullReset)
                {
                    platformController.ResetPlatforms();
                }
                                
                resetting = 2;
            }
        }

        if (resetting == 2)
        {
            cameraController.SetIntensity(cameraController.GetIntensity() - Time.deltaTime / flashTime);
            if (cameraController.GetIntensity() <= 0)
            {
                resetting = 0;
                cameraController.SetIntensity(0);
                fullReset = false;
            }
        }

        if (starting == 1)
        {
            cameraController.SetIntensity(cameraController.GetIntensity() - Time.deltaTime / (3 * flashTime));
            if (cameraController.GetIntensity() <= 0)
            {
                starting = 0;
                cameraController.SetIntensity(0);
                GameObject.FindGameObjectWithTag("Controllers").GetComponent<DCLevel>().StartShowingDialogue();
            }
        }

        if (playerMovement.finished)
        {
            cameraController.SetIntensity(cameraController.GetIntensity() + Time.deltaTime / flashTime);

            if (cameraController.GetIntensity() >= 1)
            {
                LevelController.levelController.LoadNextLevel();
            }
        }        
    }

    public void RestartLevel()
    {       
        if (playerMovement == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerMovement = player.GetComponent<Movement>();
        }

        fullReset = true;
        playerMovement.alive = false;        
    }

    public void Respawn()
    {

    }
}
