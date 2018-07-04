using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour {
    PlatformController platformController;
    GameObject player;
    Movement playerMovement;
    Vector3 initialPosition;
    Vector3 initialForward;
    CameraController cameraController;
    public float flashTime;
    InterfaceController ic;

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
    }

    private void Start()
    {
        initialPosition = player.transform.position;
        initialForward = player.transform.forward;

        resetting = 0;
        starting = 1;
        fullReset = false;
        cameraController.SetIntensity(1);
    }

    // Update is called once per frame
    void Update () {       

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
