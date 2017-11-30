using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyUp(KeyCode.Escape))
        {
            ToggleUI();
        }
	}

    void ToggleUI()
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteKey("Stages");
        GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().LoadLevel("Garden");
    }
}
