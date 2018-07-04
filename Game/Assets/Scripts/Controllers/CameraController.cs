using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    CameraRender mainCamera;
    CameraRender topCamera;
    float intensity;

    public void Awake()
    {
        mainCamera = Camera.main.GetComponent<CameraRender>();
        topCamera = GameObject.FindGameObjectWithTag("Second Camera").GetComponent<CameraRender>();
    }

    public void SetIntensity(float newIntensity)
    {
        intensity = newIntensity;
        mainCamera.intensity = intensity;
        topCamera.intensity = intensity;
    }

    public float GetIntensity()
    {
        return intensity;
    }
}
