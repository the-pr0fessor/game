using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour {

    public bool FXAA = false;
    private Material fXAAmaterial;

    // Use this for initialization
    void Start () {
        fXAAmaterial = new Material(Shader.Find("Hidden/Post FX/FXAA"));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Called every time frame rendered
    private void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        // If rendering depth, process w/ depth shader
        // This is where the depth texture is passed to the shader
        if (FXAA)
        {            
            Graphics.Blit(source, dest, fXAAmaterial);
        }        
        else
        {
            Graphics.Blit(source, dest);
        }
    }
}
