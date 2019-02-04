using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterMovement : MonoBehaviour {

    public float riseSpeed, defaultRiseSpeed;
    public Transform player;




	void Update () {
        transform.Translate(0, riseSpeed * Time.deltaTime, 0);

        if (player.position.y < transform.position.y)
        {
            player.GetComponent<PlayerManager>().Died();
        }

	}
}
