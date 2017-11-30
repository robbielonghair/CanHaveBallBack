using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMask : MonoBehaviour {

    Vector3 startPosition;
    public Vector3 endPostion;

    void Start()
    {
        startPosition = transform.position;
    }

	public void ToggleMaskPositionDown()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", endPostion,"time", 5f,"islocal", true));
        ShakeSacrificialTable();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
            ToggleMaskPositionDown();
    }

    void ShakeSacrificialTable()
    {
        iTween.ShakePosition(GameObject.Find("Room").transform.Find("Sacrificial Table").gameObject, iTween.Hash("amount", new Vector3(0.1f, 0.1f, 0), "time", 1f, "looptype", iTween.LoopType.pingPong));

        GameObject.Find("World Canvas").transform.Find("Sacrifice Texts").gameObject.SetActive(true);
    }

}
