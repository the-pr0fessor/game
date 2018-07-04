﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRender : MonoBehaviour {

    public float intensity;
    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Custom/ScreenFlash"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (intensity == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_Intensity", intensity);
        Graphics.Blit(source, destination, material);
    }
}
