using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    [SerializeField] Transform player;
    [SerializeField] GameObject dragHandle, dragAnchor;
    [SerializeField] float heightOffset = 2;
    [SerializeField] float lowHeight = 4;
    [SerializeField] float maxFallDistance = 6;
    [SerializeField] PlayerManager playerManager;

    void Start () {
		if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (playerManager.isAlive)                          //Only update the camera position if the player is "alive"
        {
            CameraAdjust();
        }        
    }


    void CameraAdjust()
    {
        if (player.position.y >= transform.position.y - heightOffset)
        {
            transform.position = new Vector3(-10, player.position.y + heightOffset, -20);
        }

        else if (player.position.y < transform.position.y - lowHeight)
        {
            transform.position = new Vector3(-10, player.position.y + lowHeight, -20);
        }

        if (!playerManager.isGrounded)
        {
            dragHandle.transform.position = new Vector3(-10, transform.position.y-2, -5);
            dragAnchor.transform.position = new Vector3(-10, transform.position.y-2, -5);
        }
    }
}
