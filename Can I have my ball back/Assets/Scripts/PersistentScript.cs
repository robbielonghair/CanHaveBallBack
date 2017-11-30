using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentScript : MonoBehaviour {

    public float timeCreated;

    string levelToLoadName;
    string lastLevelName;

	// Use this for initialization
	void Start ()
    {
        transform.Find("Background").GetComponent<SpriteRenderer>().color = Color.black;
        DontDestroyOnLoad(gameObject);
        timeCreated = Time.time;
        if (Time.time == 0)
            ShowScene();
	}

    public void FlashScreen()
    {
        GameObject.Find("World Background").GetComponent<SpriteRenderer>().color = Color.white;

        iTween.ColorTo(GameObject.Find("World Background").gameObject, new Color(1, 1, 1, 0), 1);

    }

    void SetPlayerPosition()
    {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().playerAgent)
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().playerAgent.Warp(GetPlayerPosition());
    }

    Vector3 GetPlayerPosition()
    {
        switch(levelToLoadName)
        {
            case "Garden":
                {
                    if(lastLevelName.Equals("Shed"))
                    {
                        return GameObject.Find("EnterPoint").transform.position;
                    }
                    else return GameObject.Find("NormalPoint").transform.position;
                }

            case "Shed":
                {
                    if (lastLevelName.Equals("Hole"))
                    {
                        return GameObject.Find("EnterPoint").transform.position;
                    }
                    else return GameObject.Find("NormalPoint").transform.position;
                }

            case "Hole":
                {
                    if (lastLevelName.Equals("Office"))
                    {
                        return GameObject.Find("EnterPoint").transform.position;
                    }
                    else return GameObject.Find("NormalPoint").transform.position;
                }

            case "Office":
                {
                    if (lastLevelName.Equals("Corridor"))
                    {
                        return GameObject.Find("EnterPoint").transform.position;
                    }
                    else return GameObject.Find("NormalPoint").transform.position;
                }

            case "Corridor":
                {
                    if (lastLevelName.Equals("Lab"))
                    {
                        return GameObject.Find("EnterPoint").transform.position;
                    }
                    else return GameObject.Find("NormalPoint").transform.position;
                }

            case "Lab":
                {
                    if (lastLevelName.Equals("Vent"))
                    {
                        return GameObject.Find("EnterPoint").transform.position;
                    }
                    else return GameObject.Find("NormalPoint").transform.position;
                }

            case "Vent":
                {
                    if (lastLevelName.Equals("Vent 2"))
                    {
                        return GameObject.Find("EnterPoint").transform.position;
                    }
                    else return GameObject.Find("NormalPoint").transform.position;
                }
        }

        return GameObject.Find("NormalPoint").transform.position;
    }

    void OnLevelWasLoaded()
    {
        Invoke("CheckForOtherScripts", 0.05f);
    }

    void CheckForOtherScripts()
    {
        if (timeCreated > 0)
            return;

        if (timeCreated == 0)
        {
            foreach (GameObject current in GameObject.FindGameObjectsWithTag("Persistent"))
            {
                if (current != gameObject)
                    Destroy(current);
            }
        }

        SetPlayerPosition();
        ShowScene();
    }

    void ShowScene()
    {
        print("showing screen");
        iTween.ColorTo(transform.Find("Background").gameObject, new Color(1,1,1,0), 1);
    }

    public void LoadLevel(string levelName)
    {
        if (Application.isLoadingLevel)
            return;

        levelToLoadName = levelName;

        PlayerPrefs.SetString("LevelName", levelName);


        iTween.ColorTo(transform.Find("Background").gameObject, new Color(1, 1, 1, 1), 1);

        Invoke("LoadNextLevel", 1);
    }

    void LoadNextLevel()
    {
        lastLevelName = Application.loadedLevelName;

        Application.LoadLevel(levelToLoadName);

    }


}
