using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ConversationScript : MonoBehaviour {

    public List<string> conversationLine;
    public List<Text> textBoxLine;
    public List<float> lineDelay;

    Text currentTextBox;

    int letterNumber = 0;

    string currentLine;

    bool isUpdatingLine;

    public float letterSpeed = 0.05f;

    float lineEndSpeed = 2;

    int textBoxNumber = 0;

    int lineNumber = 0;

    public float startConversationDelay = 2;

    public bool loops;

    bool isEyeWallRoom = false;

    public List<Text> cultistsAppeared;

    public int exitLine;

    public string loadSceneName;

    void Start()
    {
        if (Application.loadedLevelName.Equals("EyeRoom"))
            isEyeWallRoom = true;

        if(cultistsAppeared == null)
        {
            for(int i=0; i<textBoxLine.Count; i++)
            {
                textBoxLine[i].text = "";
            }
            cultistsAppeared = new List<Text>();
        }

        Invoke("StartLine", startConversationDelay);
    }

    public List<Text> GetMovedCharacters()
    {
        return cultistsAppeared;
    }

    public void StartLine()
    {
        // if we get another line straight away, clear last text box and cancel all future functions
        if(!cultistsAppeared.Contains(textBoxLine[textBoxNumber]) && Application.loadedLevelName.Equals("EyeRoom"))
        {
            BobCultist();
            cultistsAppeared.Add(textBoxLine[textBoxNumber]);
        }

        if(lineNumber == exitLine && exitLine > 0)
        {
            MakePlayerExit();
        }

        

        currentLine = conversationLine[lineNumber];
        currentTextBox = textBoxLine[textBoxNumber];

        if(currentTextBox.name.Equals("OtherPlayer Text"))
        {
            GameObject.Find("Actual Player").GetComponent<PlayerManager>().MakeMouthOpen();
            GameObject.Find("Actual Player").GetComponent<PlayerManager>().MakeTalk();
        }
        if (currentTextBox.name.Equals("Player Text"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().MakeMouthOpen();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().MakeTalk();
        }


        letterNumber = 0;
        currentTextBox.text = "";
        isUpdatingLine = true;
        UpdateString();
    }

    void MakePlayerExit()
    {
        print("PLAYER EXITING");
        Transform portal = GameObject.Find("EyeRoom").transform.Find("Portal");
        Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshAgent>());
        Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>());
        iTween.MoveTo(GameObject.FindGameObjectWithTag("Player").gameObject, iTween.Hash("position", portal.transform.position,"time", 5f));
        Invoke("StartRotating", 0.5f);
    }

    void ShrinkPlayer()
    {
        iTween.ScaleTo(GameObject.FindGameObjectWithTag("Player").gameObject, iTween.Hash("scale", new Vector3(0,0,0), "time", 5f));

    }

    void StartRotating()
    {
        iTween.RotateAdd(GameObject.FindGameObjectWithTag("Player").gameObject, iTween.Hash("amount", new Vector3(0, 360, 0), "time", 1, "looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
        Invoke("ShrinkPlayer", 0.5f);
    }

    void BobCultist()
    {
        iTween.MoveBy(textBoxLine[textBoxNumber].gameObject, iTween.Hash("amount", new Vector3(0, 0.65f, 0),"time", 2f, "islocal", true));
    }


    void UpdateString()
    {
        letterNumber++;
        if (letterNumber > currentLine.Length)
        {
            Invoke("EndLine", lineDelay[lineNumber]);
            //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().StopTalking();
            return;
        }

        currentTextBox.text = currentLine.Substring(0, letterNumber);
        Invoke("UpdateString", letterSpeed);
    }

    void EndLine()
    {
        currentTextBox.text = "";
        lineNumber++;
        textBoxNumber++;

        if (currentTextBox.name.Equals("OtherPlayer Text"))
        {
            GameObject.Find("Actual Player").GetComponent<PlayerManager>().StopTalking();
        }

        if (currentTextBox.name.Equals("Player Text"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().StopTalking();
        }

      

        if (lineNumber == conversationLine.Count)
        {
            if (loops)
            {
                lineNumber = 0;
                textBoxNumber = 0;
            }
            else
            {
                if (exitLine > 0)
                {
                    LoadGarden();
                }

                if(loadSceneName != "")
                {
                    GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().LoadLevel(loadSceneName);
                }

                if (Application.loadedLevelName.Equals("FinalScene"))
                {
                    GameObject.Find("Final Scene Manager").GetComponent<FinalSceneManager>().OpenPlayerInventory();
                }
                return;
            }
        }


      

        Invoke("StartLine", lineDelay[lineNumber]);
    }

    void LoadGarden()
    {
        GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().LoadLevel("Garden End");
    }
}
