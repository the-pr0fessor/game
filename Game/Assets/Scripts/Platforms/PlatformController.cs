using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {
    public bool enabled = true;
    protected Camera topCamera;
    public float speed = 1f;
    public float responsiveness = 9f;

    protected int mask;
    bool platformSelected;

    protected bool topDownView;

    protected PlatformMove platform;

    protected InterfaceController ic;
    
    private void Awake()
    {
        topCamera = GameObject.FindGameObjectWithTag("Second Camera").GetComponent<Camera>();
        mask = LayerMask.GetMask("Platform");
        ic = GameObject.FindGameObjectWithTag("Controllers").GetComponent<InterfaceController>();
    }

    // Use this for initialization
    protected virtual void Start () {
        platformSelected = false;        
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        if (topDownView && !ic.ShowingUI() && enabled)
        {
            // Get platform clicked on
            if (Input.GetButtonDown("Fire1"))
            {
                GetPlatform();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            // Move the platform based on mouse input
            else if (Input.GetButton("Fire1"))
            {
                if (platformSelected && platform != null)
                {
                    float h = Input.GetAxis("Mouse X");
                    float v = Input.GetAxis("Mouse Y");
                    
                    v = Mathf.Clamp(v, -responsiveness, responsiveness);
                    h = Mathf.Clamp(h, -responsiveness, responsiveness);
                    
                    if (h != 0)
                    {
                        platform.Move("h", h, speed);
                    }

                    if (v != 0)
                    {
                        platform.Move("v", v, speed);
                    }
                }
            }

            // Deselect the platform
            if (Input.GetButtonUp("Fire1"))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (platform != null)
                {
                    platform.DeSelect();
                    platform = null;
                }
            }
        }		
	}

    // Cast ray in to scene to get platform clicked on
    protected virtual void GetPlatform()
    {
        if (platform != null)
        {
            platform.DeSelect();
            platform = null;
        }
        
        Ray ray = topCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, mask))
        {
            platform = hit.collider.GetComponent<PlatformMove>();
            platform.Select();
            platformSelected = true;
        }
        else
        {            
            platformSelected = false;
        }
    }

    public void StartControl()
    {
        topDownView = true;
        platformSelected = false;
    }

    public void StopControl()
    {
        topDownView = false;
        if (platform != null)
        {
            platform.DeSelect();
            platform = null;            
        }
        Cursor.lockState = CursorLockMode.None;
        platformSelected = false;        
    }

    public void ResetPlatforms()
    {
        PlatformMove[] pms = GetComponentsInChildren<PlatformMove>();
        foreach (PlatformMove pm in pms)
        {
            pm.ResetPosition();
        }
    }
}
