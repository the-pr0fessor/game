using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should be called CameraFlash but too late to change
public class CameraRender : MonoBehaviour {
    public float intensity;
    private Material flashMaterial;
    
    // Creates a private material used to the effect
    void Awake()
    {
        flashMaterial = new Material(Shader.Find("Custom/ScreenFlash"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (intensity != 0)
        {
            flashMaterial.SetFloat("_Intensity", intensity);
            Graphics.Blit(source, destination, flashMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
            return;
        }        
    }
}
