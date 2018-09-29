using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour {

    public Camera firstPersonCamera;
    public Camera topCamera;

    public Movement movement;
    public CameraLook cameraLook;

    public float blendTime;

    PlatformController platformController;

    InterfaceController ic;
    public Texture2D cursorTexture;
    public Texture2D menuCursorTexture;

    Collider playerCollider;

    CameraBlend fpCameraBlend;
    CameraBlend topCameraBlend;

    bool topDownView;

    float startTimer;
    bool starting;

    private void Awake()
    {
        ic = GetComponent<InterfaceController>();
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
        platformController = GameObject.FindGameObjectWithTag("Platform Controller").GetComponent<PlatformController>();
        fpCameraBlend = firstPersonCamera.GetComponent<CameraBlend>();
        topCameraBlend = topCamera.GetComponent<CameraBlend>();

        fpCameraBlend.blendTime = blendTime;
        topCameraBlend.blendTime = blendTime;
    }

    void Start () {
        startTimer = 0;
        starting = true;

        movement.StopControl();
        cameraLook.StopControl();

        //StartTopDown();
        //Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);     
    }
	
	// Update is called once per frame
	void Update () {

        if (starting)
        {
            startTimer += Time.deltaTime;

            if (startTimer > 0.5)
            {
                starting = false;
                movement.StartControl();
                cameraLook.StartControl();
                //StartTopDown();
                //StartFirstPerson();
            }
        }
        

        if (Input.GetKeyDown("left ctrl") && !ic.ShowingUI())
        {
            topDownView = !topDownView;
            if (topDownView)
            {                
                StartTopDown();
                topCameraBlend.Switch(); // Don't want this to happen at start
            }
            else
            {                
                StartFirstPerson();
                fpCameraBlend.Switch();
            }
        }
    }

    public void StartFirstPerson()
    {
        firstPersonCamera.depth = 1;
        topCamera.depth = 0;
        topDownView = false;
        movement.StartControl();
        cameraLook.StartControl();
        platformController.StopControl();
        Cursor.visible = false;
        playerCollider.enabled = true;        
    }

    public void StartTopDown()
    {
        firstPersonCamera.depth = 0;
        topCamera.depth = 1;
        topDownView = true;
        movement.StopControl();
        cameraLook.StopControl();
        platformController.StartControl();
        Cursor.visible = true;
        playerCollider.enabled = false;        
    }

    public bool IsTopDown()
    {
        return topDownView;
    }
}
