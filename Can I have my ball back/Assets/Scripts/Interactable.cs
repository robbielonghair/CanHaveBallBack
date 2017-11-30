using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {
    Text mouseText;
    Text playerTalkText;
    public string itemName;
    public string inspectText;
    public string rejectText;

    public Transform interactPosition;

    //if the player can put this item in their inventory
    public bool isLootable;

    PlayerManager playerManager;
    
    //the id of the item that can be looted
    public int itemID;

    //the name of the item destroyed if an inventory item is used on it
    public string destroyedItemName;

    //if this object makes a noise when used (after being interacted with another object
    public bool makesNoiseAfterAltered;
    public bool wasAltered = false;

    public string transitionSceneName;

    //the list of gameobjects to activate when this object is looted/or used/moved
    public List<GameObject> activatedOnLoot;
    //opbejcts activated when an item is used on this object
    public List<GameObject> activatedOnAltered;
    //objects turned off when altered
    public List<GameObject> deactivatedOnAltered;

    public float loadSceneDelay = 1f;

    //if we interact with this object, assign a temporary item to the selected item of the player (e.g. rope end, set selected item to rope end so that we can use it on an object)
    public int tempItemId;

    //object required to be in the scene for the interaction to work
    public GameObject requiredObjectShowing;

    //assign a room number to this if it needs one
    public int roomNumber;

    //the room we should be place outside of 
    public string otherSideRoomName;

    //test tube variables
    string elementsAdded = "";

    //if this interactable is a button in the eye room, list the eye walls to toggle when this is pressed
    public List<GameObject> eyeWallToggles;
    public SpriteRenderer button;

    void Start()
    {
        if(button)
            button.color = Color.red;
        mouseText = GameObject.Find("Mouse Text").GetComponent<Text>();
        playerTalkText = GameObject.Find("Player Text").GetComponent<Text>();

        if(interactPosition == null)
            interactPosition = transform.GetChild(0);
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }
	
    void OnMouseEnter()
    {
        if (playerManager.currentGamestate == PlayerManager.gamestate.inventory)
            return;

        mouseText.text = "";

        if (playerManager.selectedItem != null )
        {
            if( playerManager.selectedItem.itemID > 0)
            {
                mouseText.text = "Use " + playerManager.selectedItem.itemName + " on ";
            }
        }

        mouseText.text += itemName;
    }

    void OnMouseOver()
    {
        if (!mouseText.text.Contains(itemName))
            return;

        if (playerManager.currentGamestate == PlayerManager.gamestate.inventory)
            return;

        if (Input.GetMouseButtonUp(0))
        {
            playerManager.releasedMouse = false;
            playerManager.Invoke("MakeMouseReleased", 0.1f);
            //mouseText.text = "";
            playerManager.itemClicked = this;
            mouseText.text = itemName;

            //if the item clicked was jeremy himself, open the inventory screen
            if (itemName == "Jeremy")
            {

                if (playerManager.selectedItem != null && playerManager.selectedItem.itemID >0)
                {
                    return;
                }

                playerManager.Invoke("OpenInventory", 1f);
                return;
            }
            playerManager.MovePlayer(Camera.main.WorldToScreenPoint(interactPosition.position));
        }
    }

    void AddItemToPlayerInventory()
    {
        playerManager.MakeGrab();
        playerManager.AddItemToInventory(itemID);
        GetComponent<AudioSource>().Play();
    }

    public void StartInspection()
    {
        if(playerManager.selectedItem != null && playerManager.selectedItem.itemID >0)
        {
            if(requiredObjectShowing == null || requiredObjectShowing != null && GameObject.Find(requiredObjectShowing.name))
            {
                StartCombine();
                return;
            }
            else
            {
                if(playerManager.selectedItem.correctUseName != itemName)
                {
                    StartCombine();
                    return;
                }
                GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine(rejectText, playerTalkText);
                playerManager.selectedItem = null;
                mouseText.text = "";
                return;
            }

        }

        if(eyeWallToggles!= null && eyeWallToggles.Count >0)
        {
            for(int i=0; i<eyeWallToggles.Count; i++)
            {
                eyeWallToggles[i].GetComponent<EyeWall>().ToggleWall();
            }
            if(button.color == Color.red)
            {
                button.color = Color.green;
            }else button.color = Color.red;
            playerManager.MakeGrab();
        }

        GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine(inspectText, playerTalkText);

        if (makesNoiseAfterAltered && wasAltered)
        {
            if(GetComponent<AudioSource>())
                GetComponent<AudioSource>().Play();

            playerManager.MakeGrab();

            if(transitionSceneName != "")
            {
                Invoke("LoadScene", loadSceneDelay);
            }
            else
            {
                //move object in scene
                if(activatedOnLoot.Count >0)
                {
                    for(int i=0;i <activatedOnLoot.Count; i++)
                    {
                        activatedOnLoot[i].SetActive(true);
                    }
                    transform.parent.gameObject.SetActive(false);
                    GameObject.Find("SaverScript").GetComponent<SaveScript>().AddGameStage(itemName);

                }


            }
        }

        if(tempItemId >0)
        {
            playerManager.MakeGrab();
            playerManager.selectedItem = playerManager.GetItemFromDatabase(tempItemId-1);
            playerManager.Invoke("SetHandCursor", 0.1f);
        }

        if(roomNumber >0)
        {
            playerManager.AddRoomNumber(roomNumber);
            //make it so player can do nothing temporarily
            playerManager.GetComponent<SpriteLayerSelector>().FadePlayerOut();
            Invoke("MovePlayerAndFade", 0.75f);
            playerManager.currentGamestate = PlayerManager.gamestate.inventory;
        }

        if (!isLootable)
            return;

        AddItemToPlayerInventory();
        mouseText.text = "";

        //if this is the test tube / remove all elements from inventory
        if(itemName.Equals("Test Tube"))
        {
            playerManager.RemoveItem("ELEMENT A");
            playerManager.RemoveItem("ELEMENT B");
            playerManager.RemoveItem("ELEMENT C");
        }
        HideObject();
    }

    void MovePlayerAndFade()
    {
        playerTalkText.text = "";
        playerManager.MoveToPosition(GameObject.Find(otherSideRoomName).transform.GetChild(0));
        playerManager.GetComponent<SpriteLayerSelector>().Invoke("FadePlayerIn", 0.1f);
        playerManager.currentGamestate = PlayerManager.gamestate.game;
    }

    void LoadScene()
    {
        GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().LoadLevel(transitionSceneName);
        //Application.LoadLevel(transitionSceneName);
    }

    void HideObject()
    {
        transform.parent.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        this.enabled = false;
    }

    public void CombineTestTube()
    {
        if(playerManager.selectedItem.correctUseName == itemName)
        {
            if(playerManager.selectedItem.itemName.Equals("ELEMENT A"))
            {
                elementsAdded += "A";
                playerManager.MakeGrab();
            }
            if (playerManager.selectedItem.itemName.Equals("ELEMENT B"))
            {
                elementsAdded += "B";
                playerManager.MakeGrab();

            }
            if (playerManager.selectedItem.itemName.Equals("ELEMENT C"))
            {
                elementsAdded += "C";
                playerManager.MakeGrab();
            }

            if (elementsAdded.Length == 1)
            {
                inspectText = "It has an element inside...";
            }
            if (elementsAdded.Length == 2)
            {
                inspectText = "It has two elements inside...";
            }

            if (elementsAdded.Length ==3)
            {
                if(elementsAdded == "CBA")
                {
                    print("elements correct");
                    GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine("The sauce in the tube is bubbly...", playerTalkText);
                    isLootable = true;
                    inspectText = "This acidic juice is mine now...";
                    playerManager.selectedItem = null;
                    return;
                }
                else
                {
                    print("elements incorrect");
                    elementsAdded = "";
                    GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine("That combination didn't do anything...", playerTalkText);
                    inspectText = "I wonder how many reactions it has witnessed...";
                    playerManager.selectedItem = null;
                    return;
                }
            }

            playerManager.selectedItem = null;
            GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine("It's made the tube sizzle...", playerTalkText);
        }
    }

    void StartCombine()
    {
        //just for the test tube object
        if(itemName.Equals("Test Tube"))
        {
            CombineTestTube();
            return;
        }

        if (playerManager.selectedItem.correctUseName == itemName)
        {
            inspectText = playerManager.selectedItem.replacementDialogue;
            GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine(playerManager.selectedItem.correctUseDialogue, playerTalkText);
            if (playerManager.selectedItem.itemName == destroyedItemName)
            {
                playerManager.RemoveItem(playerManager.selectedItem.itemName);
                GameObject.Find("SaverScript").GetComponent<SaveScript>().AddGameStage(itemName);
                if (playerManager.selectedItem.makesObjectLootableAfterUse)
                    isLootable = true;
            }

            if (GetComponent<AudioSource>())
                GetComponent<AudioSource>().Play();

            playerManager.MakeGrab();
            wasAltered = true;

            //activate object in scene
            if (activatedOnAltered.Count > 0)
            {
                for (int i = 0; i < activatedOnAltered.Count; i++)
                {
                    activatedOnAltered[i].SetActive(true);
                }
            }

            if (deactivatedOnAltered.Count > 0)
            {
                for (int i = 0; i < deactivatedOnAltered.Count; i++)
                {
                    deactivatedOnAltered[i].SetActive(false);
                }
            }

        }
        else GameObject.Find("Line Manager").GetComponent<LineManager>().StartLine(playerManager.selectedItem.badUseText, playerTalkText);
        playerManager.selectedItem = null;
    }

    void OnMouseExit()
    {
        if (mouseText.text.Contains(itemName))
            mouseText.text = "";
    }
}
