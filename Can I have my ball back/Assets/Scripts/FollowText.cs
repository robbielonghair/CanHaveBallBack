using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowText : MonoBehaviour {
    Transform playerTransform;

    public Vector3 offset = new Vector3(0, 1,0);

    //used for when the player is on either extremity of the screen
    public float extremityOffset = 1;

    int xTremityMin = 960;
    int xTremityMax = 961;

    public bool isMouseText;

    float textXPos;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

	// Update is called once per frame
	void Update ()
    {
        if(isMouseText)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
        }
        else
        {
            //left side extremity is and x of < 300
            //right side extremity is an x of > 1620
            textXPos = Camera.main.WorldToScreenPoint(playerTransform.position).x;

            if(textXPos <= xTremityMin)
            {
                transform.position = new Vector3(playerTransform.position.x + extremityOffset, 0, playerTransform.position.z - offset.y);
                GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                return;
            }

            if (textXPos >= xTremityMax)
            {
                transform.position = new Vector3(playerTransform.position.x - extremityOffset, 0, playerTransform.position.z - offset.y);
                GetComponent<Text>().alignment = TextAnchor.MiddleRight;
                return;
            }

        }
    }
}
