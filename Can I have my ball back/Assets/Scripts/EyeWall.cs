using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeWall : MonoBehaviour {

    public List<Sprite> eyeSprites;

    public bool open = true;

    List<EyeWall> eyeWalls;

    void Start()
    {
        eyeWalls = new List<EyeWall>();

        foreach(Transform current in GameObject.Find("EyeRoom").transform)
        {
            if(current.name.Substring(0,4).Equals("Wall"))
            {
                eyeWalls.Add( current.GetComponent<EyeWall>());
            }
        }
    }


    void CheckAllEyeWalls()
    {
        int total = 0;

        for(int i=0; i<eyeWalls.Count; i++)
        {
            if(!eyeWalls[i].open)
            {
                total++;
            }
        }

        //all eyes closed, room complete
        if(total == 5)
        {
            //initiate transition cutscene to final scene
            CompleteRoom();
        }
    }

   

    void CompleteRoom()
    {
        PlayerManager player = GameObject.Find("Player").GetComponent<PlayerManager>();

        player.StopMoving();

        //shake screen
        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("amount", new Vector3(0.3f, 0.3f,0), "time", 1f, "looptype", iTween.LoopType.pingPong));

        Invoke("ShowPortal", 2);

        ActivateEndConversation();
    }

    void ActivateEndConversation()
    {
        Destroy(GameObject.Find("EyeRoom").transform.Find("Conversation").gameObject);
        GameObject.Find("EyeRoom").transform.Find("Conversation Two").gameObject.SetActive(true);
        GameObject.Find("EyeRoom").transform.Find("Conversation Two").GetComponent<ConversationScript>().cultistsAppeared = GameObject.Find("EyeRoom").transform.Find("Conversation").GetComponent<ConversationScript>().GetMovedCharacters();
    }

    void ShowPortal()
    {
        //activate portal
        GameObject.Find("EyeRoom").transform.Find("Portal").gameObject.SetActive(true);
    }

    public void ToggleWall()
    {
        open = !open;

        if (!open)
        {
            ShutEyes();
        }
        else OpenEyes();
    }

    void ShutEyes()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = eyeSprites[1];
            transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }

        CheckAllEyeWalls();
    }

    void OpenEyes()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = eyeSprites[0];
            transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
        }
    }

}
