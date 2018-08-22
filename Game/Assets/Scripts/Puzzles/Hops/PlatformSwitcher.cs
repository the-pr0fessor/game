using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwitcher : PlatformController
{
    int currentlySelected;
    SwitchPlatform sp;

    SwitchPlatform[] platforms;

    protected override void Start()
    {
        base.Start();
        platforms = GetComponentsInChildren<SwitchPlatform>();
        currentlySelected = 0;
        topDownView = true;

        // Hide all the black platforms
        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].platformType == 1)
            {
                platforms[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (topDownView && !ic.ShowingUI())
        {
            // Get platform clicked on
            if (Input.GetButtonDown("Fire1"))
            {
                GetPlatform();
            }        
            
        }

        if (Input.GetKeyDown("left ctrl") && !ic.ShowingUI())
        {    
            topDownView = !topDownView;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //protected override void Update()
    //{
    //    base.Update();
    //    // Update is called once per frame
        
    //    if (Input.GetKeyDown("left ctrl") && !ic.ShowingUI())
    //    {
    //        topDownView = !topDownView;
    //    }
      
    //}

    protected override void GetPlatform()
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

            // Deselect the current platform
            if (currentlySelected != 0)
            {
                DisablePlatform();
            }

            // Select the current platform
            currentlySelected = platform.gameObject.GetComponent<SwitchPlatform>().platformNo;
            EnablePlatform();
        }       
    }

    void DisablePlatform()
    {
        // Hide the black platform
        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].platformNo == currentlySelected && platforms[i].platformType == 1)
            {
                platforms[i].gameObject.SetActive(false);
            }
        }

        // Show the red platform
        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].platformNo == currentlySelected && platforms[i].platformType == 0)
            {
                platforms[i].gameObject.SetActive(true);
            }
        }
    }

    void EnablePlatform()
    {
        // Show the black platform
        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].platformNo == currentlySelected && platforms[i].platformType == 1)
            {
                platforms[i].gameObject.SetActive(true);
            }
        }

        // Hide the red platform
        for (int i = 0; i < platforms.Length; i++)
        {
            if (platforms[i].platformNo == currentlySelected && platforms[i].platformType == 0)
            {
                platforms[i].gameObject.SetActive(false);
            }
        }
    }

}
