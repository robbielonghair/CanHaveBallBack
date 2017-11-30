using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleShake : MonoBehaviour {

    public Vector3 rotationAmount;
    public float animationTime;
    public iTween.EaseType easingType;

	// Use this for initialization
	void Start ()
    {
        //shake object for x rotation
        iTween.RotateTo(gameObject, iTween.Hash("rotation", rotationAmount, "time", animationTime, "looptype", iTween.LoopType.pingPong, "easetype", easingType));
	}
	
}
