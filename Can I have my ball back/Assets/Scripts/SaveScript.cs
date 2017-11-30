using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveScript : MonoBehaviour {

    public List<string> gameStages;

    PlayerManager player;

    ItemDatabase iDatabase;

	void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        iDatabase = GameObject.Find("Item Database").GetComponent<ItemDatabase>();
        gameStages = new List<string>();

        if(PlayerPrefs.HasKey("Stages"))
        {
            if(PlayerPrefs.GetString("LevelName") != Application.loadedLevelName)
            {
                Application.LoadLevel(PlayerPrefs.GetString("LevelName"));
                return;
            }
            Invoke("LoadGame", 0.01f);
        }
        else
        {
            //start new game
            gameStages.Add("Rock");
            PlayerPrefs.SetString("LevelName", "Garden");
            PlayerPrefsX.SetStringArray("Stages", gameStages.ToArray());
            Invoke("LoadGame", 0.01f);
        }
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            PlayerPrefs.DeleteKey("Stages");
            
        }

    }

    public void AddGameStage(string stageName)
    {
        if (gameStages.Contains(stageName))
            return;

        gameStages.Add(stageName);
        PlayerPrefsX.SetStringArray("Stages", gameStages.ToArray());

    }

    void LoadGame()
    {
        gameStages = PlayerPrefsX.GetStringArray("Stages").ToList();


        player.AddItemToInventory(5);
        

        if(Application.loadedLevelName.Equals("Garden"))
        {
            //bird juice
            if (gameStages.Contains("Bird Juice"))
            {
                player.AddItemToInventory(1);
                Destroy(GameObject.Find("BirdPoo"));
            }

            //door knob interaction
            if (gameStages.Contains("Door Knob"))
            {
                GameObject.Find("Door Knob").GetComponent<Interactable>().wasAltered = true;
                GameObject.Find("Door Knob").GetComponent<Interactable>().inspectText = iDatabase.itemList[0].replacementDialogue;
                player.RemoveItem("Bird Juice");
            }
        }

        //rope
        if (gameStages.Contains("Rope"))
        {
            player.AddItemToInventory(2);
            Destroy(GameObject.Find("Rope"));
        }

        //rug
        if (gameStages.Contains("Rug") && Application.loadedLevelName.Equals("Shed"))
        {
            Transform shed = GameObject.Find("Shed").transform;         
            shed.Find("Rug").gameObject.SetActive(false);
            shed.Find("Trap Door").gameObject.SetActive(true);
            shed.Find("CrumpledRug").gameObject.SetActive(true);
        }

        //hook
        if (gameStages.Contains("Hook"))
        {
            if(GameObject.Find("Ceiling Hook"))
            {
                GameObject.Find("Ceiling Hook").GetComponent<Interactable>().wasAltered = true;
                GameObject.Find("Ceiling Hook").GetComponent<Interactable>().inspectText = iDatabase.itemList[1].replacementDialogue;

                Transform shed = GameObject.Find("Shed").transform;
                shed.Find("Rope Over").gameObject.SetActive(true);
                shed.Find("Rope Under").gameObject.SetActive(true);

            }

            player.RemoveItem("Rope");
        }

        //trap door
        if (gameStages.Contains("Trap Door"))
        {
            if (Application.loadedLevelName.Equals("Shed"))
            {
                Transform shed = GameObject.Find("Shed").transform;
                shed.Find("Rope Over").gameObject.SetActive(false);
                shed.Find("Rope Over Attached").gameObject.SetActive(true);
                shed.Find("Trap Door").Find("Trap Door").GetComponent<Interactable>().inspectText = iDatabase.itemList[2].replacementDialogue;

            }
        }

        //used jeremy to open trap door
        if (gameStages.Contains("Jeremy"))
        {
            if (Application.loadedLevelName.Equals("Shed"))
            {
                Transform shed = GameObject.Find("Shed").transform;
                shed.Find("Trap Door Broken").gameObject.SetActive(true);
                shed.Find("Rope Broken Door").gameObject.SetActive(true);
                shed.Find("Rope Under Finished").gameObject.SetActive(true);

                shed.Find("Trap Door").gameObject.SetActive(false);
                shed.Find("Rope Over Attached").gameObject.SetActive(false);
                shed.Find("Rope Under").gameObject.SetActive(false);

                //shed.Find("Trap Door").Find("Trap Door").GetComponent<Interactable>().inspectText = iDatabase.itemList[2].replacementDialogue;

            }
        }

        //sharp pliars
        if(gameStages.Contains("Sharp Pliars"))
        {
            player.AddItemToInventory(6);
            if(Application.loadedLevelName.Equals("Shed"))
            {
                Transform shed = GameObject.Find("Shed").transform;
                shed.Find("Sharp Pliars").gameObject.SetActive(false);
            }
        }
        
        //key chain - remove pliars, make key chain lootable
        if(gameStages.Contains("Key Chain"))
        {
            if (Application.loadedLevelName.Equals("Office"))
            {
                Transform scientist = GameObject.Find("Scientist").transform;
                scientist.Find("Key").Find("Keys").GetComponent<Interactable>().isLootable = true;
                scientist.Find("Key").Find("Keys").GetComponent<Interactable>().inspectText = "The key is now mine...";
            }
            player.RemoveItem("Sharp Pliars");
        }

        //old key
        if (gameStages.Contains("Old Key"))
        {
            if (Application.loadedLevelName.Equals("Office"))
            {
                Transform scientist = GameObject.Find("Scientist").transform;
                scientist.Find("Key").gameObject.SetActive(false);
            }
            player.AddItemToInventory(7);
        }

        //unlocked old locked door
        if(gameStages.Contains("Old Door"))
        {
            if (Application.loadedLevelName.Equals("Office"))
            {
                GameObject.Find("Old Door").GetComponent<Interactable>().wasAltered = true;
                GameObject.Find("Old Door").GetComponent<Interactable>().inspectText = "I miss the old key...";

            }
            player.RemoveItem("Old Key");
        }

        //element a
        if(gameStages.Contains("ELEMENT A"))
        {
            player.AddItemToInventory(10);
            if(Application.loadedLevelName.Equals("Lab"))
            {
                GameObject.Find("Lab").transform.Find("Element A").gameObject.SetActive(false);

            }
        }

        //middle drawer
        if (gameStages.Contains("Middle Cabinet"))
        {
            if (Application.loadedLevelName.Equals("Lab"))
            {
                GameObject.Find("Lab").transform.Find("MiddleDrawer").gameObject.SetActive(false);
                GameObject.Find("Lab").transform.Find("Element B").gameObject.SetActive(true);

            }
        }

        //element B
        if (gameStages.Contains("ELEMENT B"))
        {
            player.AddItemToInventory(11);
            if (Application.loadedLevelName.Equals("Lab"))
            {
                GameObject.Find("Lab").transform.Find("Element B").gameObject.SetActive(false);
            }
        }

        //element C
        if (gameStages.Contains("ELEMENT C"))
        {
            player.AddItemToInventory(12);
            if (Application.loadedLevelName.Equals("Shed"))
            {
                GameObject.Find("Shed").transform.Find("Element C").gameObject.SetActive(false);
            }
        }

        //whiteboard
        if(gameStages.Contains("Whiteboard"))
        {
            if (Application.loadedLevelName.Equals("Lab"))
            {
                GameObject.Find("Lab").transform.Find("Board Normal").gameObject.SetActive(false);
                GameObject.Find("Lab").transform.Find("Board Flipped").gameObject.SetActive(true);

            }
        }

        //acidic juice
        if (gameStages.Contains("Acidic Juice"))
        {
            player.AddItemToInventory(9);

            if (Application.loadedLevelName.Equals("Lab"))
            {
                GameObject.Find("Lab").transform.Find("TestTube").gameObject.SetActive(false);
              
            }
            player.RemoveItem("ELEMENT A");
            player.RemoveItem("ELEMENT B");
            player.RemoveItem("ELEMENT C");
        }


        //toolbox
        if (gameStages.Contains("Toolbox"))
        {
            if (Application.loadedLevelName.Equals("Shed"))
            {
                Transform shed = GameObject.Find("Shed").transform;
                shed.Find("Toolbox").gameObject.SetActive(false);

                shed.Find("Toolbox Broken").gameObject.SetActive(true);

                shed.Find("Screwdriver").gameObject.SetActive(true);
            }
            player.RemoveItem("Acidic Juice");
        }

        //screwdriver
        if (gameStages.Contains("Screwdriver"))
        {

            if (Application.loadedLevelName.Equals("Shed"))
            {
                Transform shed = GameObject.Find("Shed").transform;
                shed.Find("Screwdriver").gameObject.SetActive(false);
            }
            player.AddItemToInventory(8);
        }

        //vent in lab
        if(gameStages.Contains("Vent"))
        {
            if (Application.loadedLevelName.Equals("Lab"))
            {
                GameObject.Find("Lab").transform.Find("Vent").gameObject.SetActive(false);
                GameObject.Find("Lab").transform.Find("Vent Entrance").gameObject.SetActive(true);
                GameObject.Find("Lab").transform.Find("Dismantled Vent").gameObject.SetActive(true);

            }
            player.RemoveItem("Screwdriver");
        }

    }

}
