using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerDialogue : MonoBehaviour
{

    CameraRender mainCamera;
    float intensity;

    public void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraRender>();
    }

    public void SetIntensity(float newIntensity)
    {
        intensity = newIntensity;
        mainCamera.intensity = intensity;        
    }

    public float GetIntensity()
    {
        return intensity;
    }
}
