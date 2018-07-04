using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {
    
    public float lookSpeed = 1f;
    public float smoothing = 5.0f;
    public float maxLookAngle = 85f;
    public float initialAngle = 0f;

    InterfaceController ic;
    
    Vector3 lookPoint;
    const float dtr = Mathf.PI / 180f; // degrees to radians
    float cameraDistance = 10f;
    float cameraXAng;
    float cameraYAng = 0f;
    bool topDownView;

    public Camera thisCamera;

    Vector3 oldPosition;

    private void Awake()
    {
        thisCamera = GetComponentInChildren<Camera>();
        ic = GameObject.FindGameObjectWithTag("Controllers").GetComponent<InterfaceController>();
    }

    void Start () {
        lookPoint = transform.position + (transform.forward * 5);
        cameraXAng = initialAngle;
        topDownView = false;
    }

    void FixedUpdate () {             
        if (!topDownView && !ic.ShowingUI())
        {
            float mouseX = 0;
            float mouseY = 0;

            mouseX = Input.GetAxisRaw("Mouse X");
            mouseY = Input.GetAxisRaw("Mouse Y");

            cameraXAng += mouseX * lookSpeed * Time.deltaTime;
            cameraYAng -= mouseY * lookSpeed * Time.deltaTime;

            // Make sure camera doesn't rotate past top or bottom
            if (cameraYAng > maxLookAngle)
            {
                cameraYAng = maxLookAngle;
            }
            if (cameraYAng < -maxLookAngle)
            {
                cameraYAng = -maxLookAngle;
            }

            // Use sphere equations to get 3D point on sphere to look at
            lookPoint.x = Mathf.Lerp(lookPoint.x, transform.position.x - cameraDistance * Mathf.Sin(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr), smoothing * Time.deltaTime);
            lookPoint.y = Mathf.Lerp(lookPoint.y, transform.position.y - cameraDistance * Mathf.Sin(cameraYAng * dtr), smoothing * Time.deltaTime);
            lookPoint.z = Mathf.Lerp(lookPoint.z, transform.position.z - cameraDistance * Mathf.Cos(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr), smoothing * Time.deltaTime);

            //lookPoint.x = transform.position.x - cameraDistance * Mathf.Sin(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr);
            //lookPoint.y = transform.position.y - cameraDistance * Mathf.Sin(cameraYAng * dtr);
            //lookPoint.z = transform.position.z - cameraDistance * Mathf.Cos(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr);

            thisCamera.transform.LookAt(lookPoint);

            Vector3 playerLookPoint = lookPoint;
            playerLookPoint.y = transform.position.y;
            transform.LookAt(playerLookPoint);
        }      
    }

    public void Reset()
    {
        lookPoint = transform.position + (transform.forward * 5);
        cameraXAng = initialAngle;
        cameraYAng = 0;
    }

    public void StartControl()
    {
        topDownView = false;
        Vector3 movement = transform.position - oldPosition;
        lookPoint += movement;
    }

    public void StopControl()
    {
        topDownView = true;
        oldPosition = transform.position;
    }
}