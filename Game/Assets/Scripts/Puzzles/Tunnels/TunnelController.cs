using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TunnelController : MonoBehaviour {
    public int[] positions;
    public int currentTunnel;

    public Text numberText;

    GameObject player;
    Vector3 playerStartPos;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStartPos = player.transform.position;
        currentTunnel = 1;
        numberText.text = positions[currentTunnel+1].ToString();
        SetPositions();
    }

    void SetPositions()
    {
        for (int i = 0; i <= positions.Length - 2; i++)
        {
            positions[i] = Random.Range(1, 4);
            Debug.Log(positions[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ExitTunnel (int tunnel, int position)
    {    
        if (tunnel == currentTunnel)
        {
            // If position correct
            if (positions[currentTunnel] == position)
            {
                currentTunnel++;                
            }
            // Otherwise restart
            else
            {
                currentTunnel = 1;
                player.transform.position = new Vector3(playerStartPos.x, player.transform.position.y, player.transform.position.z);
                SetPositions();
            }
        }
        // Going backwards
        else
        {
            currentTunnel--;
        }

        // Set the text      
        Debug.Log(currentTunnel + 1);
        numberText.text = positions[currentTunnel+1].ToString();
        
        // Finish level if at the end
        if (currentTunnel == positions.Length - 2)
        {
            LevelController.levelController.LoadNextLevel();
        }
    }
}
