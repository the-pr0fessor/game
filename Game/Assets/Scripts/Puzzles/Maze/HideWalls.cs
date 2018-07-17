using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWalls : MonoBehaviour {

    public GameObject wallsObject;
    InterfaceController ic;
    MeshRenderer[] wallMeshes;

    private void Awake()
    {
        ic = GetComponent<InterfaceController>();
    }

    void Start()
    {
        wallMeshes = wallsObject.GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("left ctrl") && !ic.ShowingUI())
        {           
            foreach (MeshRenderer wallMesh in wallMeshes)
            {
                wallMesh.enabled = !wallMesh.enabled;
            }            
        }
    }
}
