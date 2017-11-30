using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    public NavMeshAgent playerAgent;

    float blinkTime;

    Texture2D mouseDown, mouseUp, handCursor;

    public Interactable itemClicked;

    SpriteRenderer mouthSprite;

    List<Sprite> mouthSprites;

    float mouthChangeDelay = 0.15f;

    Transform grabbingArm;

    public List<Item> inventory;

    GameObject inventoryScreen;

    public gamestate currentGamestate = gamestate.game;

    //the inventory screen hand pivot
    Transform inventoryArmPivot;

    //inventory screen arm start position
    Vector3 inventoryArmStartPos;
    Vector3 inventoryArmStartRot;
    Vector3 inventoryHandStartPos;

    public Item selectedItem;

    GameObject footLeft, footRight;
    Vector3 footLeftStartPos, footRightStartPos;
    GameObject body;
    GameObject shadow;

    movestate currentMoveState = movestate.idle;

    public bool releasedMouse = true;

    //used to check which order the player has entered doors in the corridor (certain concurrent numbers leads you to different rooms)
    string roomNumbers;

    string roomToLoad="";

    public enum gamestate
    {
        inventory,
        game
    }

    enum movestate
    {
        walking,
        idle
    }

    // Use this for initialization
    void Start()
    {
        GetVariables();
    }

    public void AddRoomNumber(int roomNum)
    {
        roomNumbers += roomNum.ToString();

        //check if a specific code is present in room numbers
        //1221 - leads to lab
        if(roomNumbers.Contains("1221"))
        {
            print("room 1221 has been found");
            roomToLoad = "Lab";
        }
    }

    public void StopMoving()
    {
        currentGamestate = gamestate.inventory;
        playerAgent.isStopped = true;
        currentMoveState = movestate.idle;
    }

    public string GetRoomToLoad()
    {
        return roomToLoad;
    }

    public void MoveToPosition(Transform tempPos)
    {
        playerAgent.Warp(tempPos.position);
       
    }

    public void MoveLeftFoot()
    {
        if (!playerAgent)
            return;

        if (currentMoveState == movestate.walking)
            return;

        currentMoveState = movestate.walking;

        iTween.MoveTo(footLeft, iTween.Hash("position", new Vector3(footLeft.transform.localPosition.x + 0.07f, 0, footLeft.transform.localPosition.z + 0.07f),"islocal", true, "time", 0.3f,  "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear, "oncomplete", "CheckIdleLeft", "oncompletetarget", gameObject));
        Invoke("MoveRightFoot", 0.3f);
    }

    public void MoveRightFoot()
    {
        if (!playerAgent)
            return;

        iTween.MoveTo(footRight, iTween.Hash("position", new Vector3(footRight.transform.localPosition.x + 0.07f, 0, footRight.transform.localPosition.z + 0.07f), "islocal", true, "time", 0.3f, "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear, "oncomplete", "CheckIdleRight", "oncompletetarget", gameObject));
    }

    void GetVariables()
    {
        if(GetComponent<NavMeshAgent>())
        {
            playerAgent = GetComponent<NavMeshAgent>();
            playerAgent.updateRotation = false;
        }
      
        mouseDown = Resources.Load("Art/Utility/CursorDown", typeof(Texture2D)) as Texture2D;
        mouseUp = Resources.Load("Art/Utility/Cursor", typeof(Texture2D)) as Texture2D;
        handCursor = Resources.Load("Art/Utility/UsingCursor", typeof(Texture2D)) as Texture2D;
        roomNumbers = "";

        mouthSprites = new List<Sprite>();
        mouthSprite = transform.Find("Mouth").GetComponent<SpriteRenderer>();

        //normal mouth
        mouthSprites.Add(mouthSprite.sprite);

        footLeft = transform.Find("FootLeft").gameObject;
        footRight = transform.Find("FootRight").gameObject;
        footLeftStartPos = footLeft.transform.localPosition;
        footRightStartPos = footRight.transform.localPosition;
        body = transform.Find("Body").gameObject;
        shadow = transform.Find("Shadow").gameObject;

        iTween.ScaleTo(body, iTween.Hash("scale", new Vector3(1.05f,1,1.05f), "islocal", true, "time", 1f, "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear));

        iTween.ScaleTo(shadow, iTween.Hash("scale", shadow.transform.localScale * 1.05f, "islocal", true, "time", 1f, "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear));

        //talking mouth states
        foreach (Sprite current in Resources.LoadAll("Art/Garden/Mouths", typeof(Sprite)))
        {
            mouthSprites.Add(current);
        }

        grabbingArm = GameObject.Find("ArmRight").transform;

        //grab saved inventory at later stage
        //IMPORTANT!!
        inventory = new List<Item>();

        inventoryScreen = GameObject.Find("World Canvas").transform.Find("Inventory Screen").gameObject;

        inventoryArmPivot = inventoryScreen.transform.Find("Mask").Find("Arm Pivot");
        inventoryArmStartPos = inventoryArmPivot.localPosition;
        inventoryArmStartRot = inventoryArmPivot.eulerAngles;
        inventoryHandStartPos = inventoryArmPivot.GetChild(0).localPosition;

        iTween.RotateTo(inventoryArmPivot.gameObject, iTween.Hash("islocal", true,"rotation", new Vector3(0, 0, -7), "time", 6, "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear));

        inventory = new List<Item>();
        for(int i=0; i<6; i++)
        {
            inventory.Add(new Item());
        }

    }

    public void RemoveItem(string itemName)
    {
        for(int i=0; i<inventory.Count; i++)
        {
            if(inventory[i].itemName == itemName)
            {
                inventory[i] = new Item();
                return;
            }
        }
    }

    public Item GetItemFromDatabase(int itemID)
    {
        return GameObject.Find("Item Database").GetComponent<ItemDatabase>().itemList[itemID];
    }

    public void SelectItem(int slotNum)
    {
        if (inventoryArmPivot.GetComponent<iTween>())
            Destroy(inventoryArmPivot.GetComponent<iTween>());

        GameObject.Find("Mouse Text").GetComponent<Text>().text = "";

        //move inventory hand to the item taken
        inventoryArmPivot.LookAt(GameObject.Find("Item Holder").transform.GetChild(slotNum));
        inventoryArmPivot.localEulerAngles = new Vector3(0, 0, -inventoryArmPivot.localEulerAngles.x);
        iTween.MoveTo(GameObject.Find("Hand Centre"), GameObject.Find("Item Holder").transform.GetChild(slotNum).position, 2);
        Invoke("CloseInventory", 1f);
       
        Invoke("ResetArm", 2);
        Invoke("SetHandCursor",0.1f);

        selectedItem = inventory[slotNum];

    }

    public void SetHandCursor()
    {
        Cursor.SetCursor(handCursor, new Vector2(0, 0), CursorMode.Auto);

    }

    void ResetArm()
    {
        Destroy(inventoryArmPivot.Find("Hand Centre").GetComponent<iTween>());

        inventoryArmPivot.localPosition = inventoryArmStartPos;
        inventoryArmPivot.eulerAngles = inventoryArmStartRot;
        inventoryArmPivot.GetChild(0).localPosition = inventoryHandStartPos;
        iTween.RotateTo(inventoryArmPivot.gameObject, iTween.Hash("islocal", true, "rotation", new Vector3(0, 0, -7), "time", 6, "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.linear));

        // inventoryArmPivot.localEulerAngles = new Vector3(0, 0, 26);
        // GameObject.Find("Hand Centre").transform.localPosition = new Vector3(69.6f, -7.5f, 0);
    }

    public void MakeGrab()
    {
        iTween.PunchRotation(grabbingArm.gameObject, new Vector3(0, -120), 2);
    }

    public void AddItemToInventory(int itemNum)
    {
        //0 is blank
        if (itemNum == 0)
            return;

        GameObject.Find("SaverScript").GetComponent<SaveScript>().AddGameStage(GameObject.Find("Item Database").GetComponent<ItemDatabase>().itemList[itemNum - 1].itemName);


        for (int i=0; i<inventory.Count; i++)
        {
            if(inventory[i].itemID == 0)
            {
                inventory[i] = (GameObject.Find("Item Database").GetComponent<ItemDatabase>().itemList[itemNum - 1]);
                return;
            }
        }



    }

    public void MakeMouthOpen()
    {
        mouthSprite.sprite = mouthSprites[2];
    }

    public void MakeTalk()
    {
       if(mouthSprite.sprite == mouthSprites[0] || mouthSprite.sprite == mouthSprites[1])
        {
            mouthSprite.sprite = mouthSprites[2];
            Invoke("MakeTalk", mouthChangeDelay);
            return; 
        }

       if(mouthSprite.sprite == mouthSprites[2])
        {
            mouthSprite.sprite = mouthSprites[1];
            Invoke("MakeTalk", mouthChangeDelay);
            return;
        }
    }

    public void StopTalking()
    {
        CancelInvoke("MakeTalk");
        mouthSprite.sprite = mouthSprites[0];
    }

    public void MakeMouseReleased()
    {
        releasedMouse = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!itemClicked && currentGamestate == gamestate.game || releasedMouse == true && currentGamestate == gamestate.game)
            {
                MovePlayer(Input.mousePosition);
                selectedItem = null;
            }
            Cursor.SetCursor(mouseUp, new Vector2(0, 0), CursorMode.Auto);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(mouseDown, new Vector2(0, 0), CursorMode.Auto);
        }

        if(itemClicked)
        {
            if (Vector2.Distance(new Vector2(itemClicked.interactPosition.position.x, itemClicked.interactPosition.position.z), new Vector2(transform.position.x, transform.position.z)) < 0.2f)
            {
                MakeTalk();
                itemClicked.StartInspection();
                itemClicked = null;
            }
        }

        if (!footLeft && !footLeft.GetComponent<iTween>() && !footRight.GetComponent<iTween>() || currentMoveState == movestate.idle)
            return;

        if (playerAgent && !playerAgent.pathPending)
        {
            if (playerAgent.remainingDistance <= playerAgent.stoppingDistance)
            {
                if (!playerAgent.hasPath || playerAgent.velocity.sqrMagnitude == 0f)
                {
                    CancelInvoke("MoveRightFoot");

                   /* //stop walking
                    if(footLeft.GetComponent<iTween>())
                        footLeft.GetComponent<iTween>().on

                    if(footRight.GetComponent<iTween>())
                        footRight.GetComponent<iTween>().loopType = iTween.LoopType.none;*/

                    currentMoveState = movestate.idle;
                }
            }
        }

    }

    public void CheckIdleLeft()
    {
        if (currentMoveState == movestate.walking)
            return;
        footLeft.transform.localPosition = footLeftStartPos;

        Destroy(footLeft.GetComponent<iTween>());
    }

    public void CheckIdleRight()
    {
        if (currentMoveState == movestate.walking)
            return;

        footRight.transform.localPosition = footRightStartPos;

        Destroy(footRight.GetComponent<iTween>());
    }

    public void OpenInventory()
    {
        currentGamestate = gamestate.inventory;
        inventoryScreen.SetActive(true);

        for(int i=0; i<inventoryScreen.transform.Find("Item Holder").childCount; i++)
        {
            if(i<inventory.Count && inventory[i].itemID>0)
            {
                inventoryScreen.transform.Find("Item Holder").GetChild(i).gameObject.SetActive(true);
                inventoryScreen.transform.Find("Item Holder").GetChild(i).GetComponent<Image>().sprite = inventory[i].itemSprite;
            }
            else
            {
                inventoryScreen.transform.Find("Item Holder").GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void IdentifyInventoryItem(int slotNum)
    {
        GameObject.Find("Mouse Text").GetComponent<Text>().text = inventory[slotNum].itemName;
    }

    public void HideInventoryItem(int slotNum)
    {
        GameObject.Find("Mouse Text").GetComponent<Text>().text = "";
    }

    public void CloseInventory()
    {
        currentGamestate = gamestate.game;
        inventoryScreen.SetActive(false);
        GameObject.Find("Mouse Text").GetComponent<Text>().text = "";

    }

    public void MovePlayer(Vector3 destination)
    {
        MoveLeftFoot();
        destination.z = 10;
        destination = Camera.main.ScreenToWorldPoint(destination);

        if (!playerAgent)
            return;
        playerAgent.SetDestination(new Vector3(destination.x, 0, destination.z));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrassTuft>())
            other.GetComponent<GrassTuft>().ShakeTuft();        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GrassTuft>())
            other.GetComponent<GrassTuft>().ShakeTuft();
    }
}
