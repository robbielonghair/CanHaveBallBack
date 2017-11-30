using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FinalSceneManager : MonoBehaviour {

    PlayerManager player;

    GameObject ball;


    // Use this for initialization
    void Start()
    {
        if (Application.loadedLevelName.Equals("Garden End"))
            Invoke("StartScene", 0.1f);

        if (Application.loadedLevelName.Equals("BallPush"))
            Invoke("ShakeBoy", 0.1f);

    }

    public void OpenPlayerInventory()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().OpenInventory();
        Invoke("TakePetRock", 1.5f);
    }

    void TakePetRock()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().SelectItem(0);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().currentGamestate = PlayerManager.gamestate.inventory;
        Invoke("SayStatement", 1f);
    }

    void SayStatement()
    {
        GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine("he wants to be used on him...", GameObject.Find("Player Text").GetComponent<Text>());
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().MakeMouthOpen();
        Invoke("ShowReply", 2f);
    }
    

    void ShowReply()
    {
        GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine("what?", GameObject.Find("OtherPlayer Text").GetComponent<Text>());
        Invoke("FlashScreen", 1f);
 
    }

    void FlashScreen()
    {
        GameObject.Find("Actual Player").gameObject.SetActive(false);
        GameObject.Find("OtherPlayer Text").GetComponent<Text>().text = "";
        GameObject.Find("PersistentScript").GetComponent<PersistentScript>().FlashScreen();
        GameObject.Find("Conversations").transform.Find("DeadPlayer").gameObject.SetActive(true);
        iTween.ScaleBy(GameObject.Find("Conversations").transform.Find("DeadPlayer").transform.Find("Blood").gameObject, new Vector3(3, 3, 3), 15);
        iTween.ShakePosition(GameObject.Find("Conversations").transform.Find("DeadPlayer").gameObject, iTween.Hash("amount", new Vector3(0.02f, 0.02f, 0.02f), "time", 3, "looptype", iTween.LoopType.pingPong));
        GameObject.Find("Conversations").transform.Find("TransitionCircle").gameObject.SetActive(true);
        GameObject.Find("Conversations").transform.Find("Back").gameObject.SetActive(true);

        Invoke("FinalLine", 5f);
    }

    void FinalLine()
    {
        GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine("i am jeremy...", GameObject.Find("Player Text").GetComponent<Text>());
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().MakeMouthOpen();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().MakeTalk();
        Invoke("ResetGame", 5f);

    }

    void ResetGame()
    {
        iTween.ScaleTo(GameObject.Find("TransitionCircle"), new Vector3(0, 0, 0), 5);

        PlayerPrefs.DeleteKey("Stages");
        Invoke("ShowText", 6);
    }

    void ShowText()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", GameObject.Find("FinishLine").GetComponent<Text>().color, "to", Color.white,"onupdate", "UpdateColour", "time", 2f));

        Invoke("LoadNextScene", 5);

    }

    public void UpdateColour(Color value)
    {
        GameObject.Find("FinishLine").GetComponent<Text>().color = value;
    }

    void LoadNextScene()
    {
        GameObject.Find("PersistentScript").GetComponent<PersistentScript>().LoadLevel("Garden");

    }

    void StartScene()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        player.GetComponent<NavMeshAgent>().enabled = false;

        //don't allow player input
        player.currentGamestate = PlayerManager.gamestate.inventory;

        //ball
        ball = GameObject.Find("Garden End").transform.Find("Ball").gameObject;

        //spawn portal after 1 second
        Invoke("SpawnPortal", 1f);
    }

    void SpawnPortal()
    {
        //show portal
        GameObject.Find("Garden End").transform.Find("Portal").gameObject.SetActive(true);

        //shake screen
        iTween.ShakePosition(Camera.main.gameObject, iTween.Hash("amount", new Vector3(0.3f, 0.3f, 0), "time", 1f, "looptype", iTween.LoopType.pingPong));

        Invoke("DropBall", 1f);
        LookAtPlayer();

    }

    void DropBall()
    {
        //spin ball
        iTween.RotateBy(ball, iTween.Hash("amount", new Vector3(0, 0, 1), "time", 2f, "easetype", iTween.EaseType.easeOutBounce));

        //drop ball to floor
        iTween.MoveTo(ball, iTween.Hash("position", GameObject.Find("BallFloorPosition").transform.position, "time", 2.5f, "easetype", iTween.EaseType.easeOutBounce));

        //move player in
        Invoke("BringPlayerIn", 1f);
    }

    void BringPlayerIn()
    {
        //grow player
        iTween.ScaleTo(player.gameObject, iTween.Hash("scale", new Vector3(0.8f, 0.8f, 0.8f), "time", 5f, "name", "scaler", "easetype", iTween.EaseType.linear));

        //spin player
        iTween.RotateAdd(player.gameObject, iTween.Hash("amount", new Vector3(0, 1440, 0), "time", 5, "easetype", iTween.EaseType.linear, "name", "rotator"));

        Invoke("StopPortal", 4);

    }

    void StopPortal()
    {
        GameObject.Find("Portal").transform.GetChild(0).GetComponent<ParticleSystem>().loop = false;
        GameObject.Find("Portal").transform.GetChild(1).GetComponent<ParticleSystem>().loop = false;
        GameObject.Find("Portal").GetComponent<ParticleSystem>().loop = false;

        // Destroy(Camera.main.GetComponent<iTween>());
        Camera.main.GetComponent<iTween>().loopType = iTween.LoopType.none;

        //start conversation between player and son
        Invoke("StartFirstConversation", 1f);
    }

    void LookAtPlayer()
    {
        GameObject.Find("Actual Player").transform.Find("Eye Holder").GetComponent<Animator>().enabled = true;
    }

    void StartFirstConversation()
    {
        GameObject.Find("Conversations").transform.Find("Conversation 1").gameObject.SetActive(true);
    }

    public void LoadBallScene()
    {
        GameObject.Find("PersistentScript").GetComponent<PersistentScript>().LoadLevel("BallPush");
    }

    void ShakeBoy()
    {
        print("shaking boy");
        iTween.ShakePosition(GameObject.Find("Actual Player").gameObject, iTween.Hash("amount", new Vector3(0.02f, 0.02f, 0.02f), "time", 3, "looptype", iTween.LoopType.pingPong));
        Invoke("ShowConversation", 2f);
    }

    void ShowConversation()
    {
        GameObject.Find("Conversations").transform.Find("Conversation 1").gameObject.SetActive(true);
        Invoke("ThrowBallOver", 7);
    }
    
    void ThrowBallOver()
    {
        GameObject.Find("Actual Player").transform.Find("Player").GetComponent<Animator>().enabled = true;
        Invoke("LoadFinalScene", 3);
    }

    void LoadFinalScene()
    {
        GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().LoadLevel("FinalScene");
    }
}
