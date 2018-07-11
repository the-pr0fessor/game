using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraBlend : MonoBehaviour {
    public float blendTime;

    float blendValue; // fraction of first person camera image to use
    Material blendMaterial;
    ScreenCapture otherCameraCapture;
    Texture2D otherScreen;
    bool switching;

    // Creates a private material used to the effect
    void Awake()
    {
        blendMaterial = new Material(Shader.Find("Custom/ScreenBlend"));
           
        // Get the other camera
        if (gameObject.tag == "MainCamera")
        {
            otherCameraCapture = GameObject.FindGameObjectWithTag("Second Camera").GetComponent<ScreenCapture>();
            blendMaterial.SetInt("_Direction", 1);
        }
        else
        {
            otherCameraCapture = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenCapture>();
            blendMaterial.SetInt("_Direction", -1);
        }
        
        switching = false;
    }

    void GetOtherTexture()
    {
        // This fuckery makes it work don't change it unless you know what's going on
        otherScreen = otherCameraCapture.GetScreen();
        byte[] imageData = otherScreen.EncodeToPNG();
        otherScreen = new Texture2D(Screen.width, Screen.height);
        otherScreen.LoadImage(imageData);
        blendMaterial.SetTexture("_OtherTex", otherScreen);
    }

    public void Switch()
    {
        GetOtherTexture();
        switching = true;
        blendValue = 0.0f;
    }

    //public void SwitchToTopDown()
    //{
    //    GetTopTexture();

    //    switching = -1;
    //    blendValue = 1.0f;
    //}

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (switching)
        {            

            blendValue += (Time.deltaTime / blendTime);

            if (blendValue >= 1)
            {
                switching = false;                
            }
            
            blendMaterial.SetFloat("_BlendValue", blendValue);
            Graphics.Blit(source, destination, blendMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
            //Graphics.Blit(source, destination, blendMaterial);
        }
    }
}
