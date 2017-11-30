using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item{

    public int itemID;
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public string badUseText;
    public string correctUseName;
    public string correctUseDialogue;
    public string replacementDialogue;
    public bool makesObjectLootableAfterUse;

    public Item(int id, string iName, string itemDesc, string badUseString, string correctItemName, string correctDialogue, string replacementDial, bool makesLootable)
    {
        itemID = id;
        itemName = iName;
        itemDescription = itemDesc;
        badUseText = badUseString;
        correctUseName = correctItemName;
        correctUseDialogue = correctDialogue;
        replacementDialogue = replacementDial;
        makesObjectLootableAfterUse = makesLootable;

        foreach(Sprite current in Resources.LoadAll("Art/Items/ObtainableItems", typeof(Sprite)))
        {
            if(current.name.Equals(itemName))
            {
                itemSprite = current;
                return;
            }
        }
    }

    public Item()
    {
    }
}
