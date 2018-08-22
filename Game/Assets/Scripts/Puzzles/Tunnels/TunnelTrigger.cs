using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelTrigger : MonoBehaviour {

    public int TunnelNo;
    public int TriggerNo;

    TunnelController tc;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag.Equals("Player"))
        {
            //Debug.Log("Tunnel: " + TunnelNo + ", trigger:" + TriggerNo);
            tc.ExitTunnel(TunnelNo, TriggerNo);
        }
        
    }

    // Use this for initialization
    void Start () {
        tc = GameObject.FindGameObjectWithTag("Controllers").GetComponent<TunnelController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
