using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNumber : MonoBehaviour {

    public Canvas numberCanvas;

	// Use this for initialization
	void Start () {
        numberCanvas.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("left ctrl"))
        {
            numberCanvas.enabled = !numberCanvas.enabled;
        }
	}
}
