using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenCapture : MonoBehaviour {

    Camera thisCamera;
    RenderTexture cameraRenderTexture;

    void Start () {
        cameraRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        thisCamera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public Texture2D GetScreen()
    {
        Texture2D imageTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false); ;

        // Render camera to texture
        thisCamera.targetTexture = cameraRenderTexture;
        thisCamera.Render();

        // Set cameraRenderTexture as the active texture so the screenshot reads from it
        RenderTexture.active = cameraRenderTexture;
        imageTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        //byte[] imageData = imageTexture.EncodeToPNG();

        //var file = File.Create(@"G:\Game\game\Game\Assets\Textures\test1.png");
        //file.Write(imageData, 0, imageData.Length);
        //file.Close();

        thisCamera.targetTexture = null;
        RenderTexture.active = null;

        return imageTexture;
    }
}
