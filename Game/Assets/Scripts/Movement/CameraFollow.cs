using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    const float dtr = Mathf.PI / 180f; // degrees to radians

    public Transform target;
    public float moveSmoothing = 5.0f;
    public float lookSmoothing = 5.0f;
    public float heightOffset = 0;
    public float cameraDistV = 0;
    public float cameraDistH = 0;
    public float mouseSpeed = 100;
    public float maxLookAngle = 85f;
    public float maxHeightDiff = 5f;
    public float maxLookSpeed = 1f;
    

    MovementNew targetMovement;

    float cameraXAng = 0f;
    float cameraYAng = 0f;


    Vector3 offset;

    void Awake()
    {        
        offset = transform.position - target.position;
        targetMovement = target.GetComponent<MovementNew>();
    }

    void FixedUpdate()
    {
        // Camera rotation
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        mouseX = Mathf.Clamp(mouseX, -maxLookSpeed, maxLookSpeed);
        mouseY = Mathf.Clamp(mouseY, -maxLookSpeed, maxLookSpeed);

        Vector3 lookPoint = new Vector3();
        float cameraDistance = offset.magnitude;

        cameraXAng += mouseX * mouseSpeed * Time.deltaTime;
        cameraYAng -= mouseY * mouseSpeed * Time.deltaTime;

        //cameraDistV += 0.25f * mouseY;
        cameraDistV -= (mouseSpeed / 1000) * mouseY;

        if (cameraDistV > maxHeightDiff)
        {
            cameraDistV = maxHeightDiff;
        }

        if (cameraDistV < 0)
        {
            cameraDistV = 0;
        }

        // Make sure camera doesn't rotate past top or bottom
        if (cameraYAng > maxLookAngle)
        {
            cameraYAng = maxLookAngle;
        }
        if (cameraYAng < -maxLookAngle)
        {
            cameraYAng = -maxLookAngle;
        }

        //lookPoint.x = Mathf.Lerp(lookPoint.x, target.transform.position.x - cameraDistance * Mathf.Sin(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr), lookSmoothing * Time.deltaTime);    
        //lookPoint.x = target.transform.position.x - cameraDistance * Mathf.Sin(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr);
        //lookPoint.y = 0;
        //lookPoint.z = Mathf.Lerp(lookPoint.z, target.transform.position.z - cameraDistance * Mathf.Cos(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr), lookSmoothing * Time.deltaTime);
        //lookPoint.z = target.transform.position.z - cameraDistance * Mathf.Cos(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr);

        lookPoint.x = target.transform.position.x - cameraDistance * Mathf.Sin(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr);
        lookPoint.y = 0;
        lookPoint.z = target.transform.position.z - cameraDistance * Mathf.Cos(cameraXAng * dtr) * Mathf.Cos(cameraYAng * dtr);

        if (targetMovement.direction == "b")
        {
            //float temp = lookPoint.x;
            //lookPoint.x = lookPoint.z;
            //lookPoint.z = temp;
            lookPoint.x = -lookPoint.x;
            lookPoint.z = -lookPoint.z;
        }

        // TODO: fix looking for walking backwards
        // E.g. rotate look point 180 degrees on sphere when walking backwards
        target.transform.LookAt(lookPoint);

        if (targetMovement.MovedThisFrame())
        {

        }
        else
        {

        }

        // Camera movement
        //Vector3 fwd = target.transform.forward;
        Vector3 fwd = targetMovement.virtualForward;

        fwd.y = 0;
        fwd.Normalize();
        fwd *= -cameraDistH;


        //Vector3 targetCamPos = target.position + offset;
        Vector3 targetCamPos = target.position + fwd;
        targetCamPos.y += cameraDistV;

        transform.position = Vector3.Lerp(transform.position, targetCamPos, moveSmoothing * Time.deltaTime);
        Vector3 lookPos = new Vector3();
        lookPos = target.transform.position;
        lookPos.y += heightOffset;
        transform.LookAt(lookPos);        
    } 
 }