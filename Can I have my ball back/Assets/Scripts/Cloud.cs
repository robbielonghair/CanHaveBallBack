using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    public float speedVal;
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x + (speedVal*Time.deltaTime), transform.position.y, transform.position.z);
	}
}
