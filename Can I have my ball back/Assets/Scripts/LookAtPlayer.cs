using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour {

    Transform player;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(player);
        transform.localRotation = new Quaternion(0, 0, -transform.localRotation.y, transform.localRotation.w);
	}
}
