using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRotator : MonoBehaviour {
    public float rotSpeed;

    private void Start()
    {
        if (Random.value >= 0.9f)
        {
            Destroy(gameObject);      
        }
    }


    void Update () {
        transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
	}
}
