using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentCollapse : MonoBehaviour {

    PlayerManager manager;

    public List<GameObject> objectsToActivate;

	// Use this for initialization
	void Start ()
    {
        manager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            CollapseVent();
        }
    }

    void CollapseVent()
    {
        //stop playing moving
        manager.StopMoving();
        manager.GetComponent<Rigidbody>().useGravity = true;
        manager.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -3);

        manager.playerAgent.enabled = false;

        //show vent open and hide this sprite
        GetComponent<SpriteRenderer>().enabled = false;
        for(int i=0;i< objectsToActivate.Count;i++)
        {
            objectsToActivate[i].SetActive(true);
        }

        //turn player eyes off
        GameObject.Find("Player").GetComponent<SpriteLayerSelector>().BlinkIndefinitely();

        Invoke("LoadEyeRoom", 2f);
    }

    void LoadEyeRoom()
    {
        GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().LoadLevel("EyeRoom");

        //Application.LoadLevel("EyeRoom");
    }



}
