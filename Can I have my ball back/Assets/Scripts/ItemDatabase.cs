using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {

    public List<Item> itemList;

    void Start()
    {
        itemList = new List<Item>();

        itemList.Add(new Item(1, "Bird Juice", "The wet juices from a flying birdie...", "That would be a waste of this bird juice...", "Door Knob", "The bird juices have made the door knob wobbly...", "A wet door knob...", false));
        itemList.Add(new Item(2, "Rope", "It burns when I rub my hands with it...", "It doesn't want to be tied to that...", "Hook", "It wanted to hang from the hook...", "The hook and the rope are friends...", false));
        itemList.Add(new Item(3, "Rope End", "It burns when I rub my hands with it...", "It doesn't want to be tied to that...", "Trap Door", "It wanted to latch onto the trap door...", "The trap door likes the rope...", false));
        itemList.Add(new Item(4, "Rope Back End", "It burns when I rub my hands with it...", "It doesn't want to be pulled by that...", "Jeremy", "We broke the door together...", "I am Jeremy...", false));
        itemList.Add(new Item(5, "Pet Rock", "It burns when I rub my hands with it...", "No...he's hiding...", "Misc", "We broke the door together...", "I am Jeremy...", false));
        itemList.Add(new Item(6, "Sharp Pliars", "It burns when I rub my hands with it...", "It doesn't want to bite that...", "Key Chain", "It freed the key...", "The key is now mine...", true));
        itemList.Add(new Item(7, "Old Key", "It burns when I rub my hands with it...", "It doesn't fit inside of that...", "Old Door", "The door will let me pass...", "I miss the old key...", false));
        itemList.Add(new Item(8, "Screwdriver", "It burns when I rub my hands with it...", "It doesn't want to loosen that...", "Vent", "The dark passage has been unlocked", "Into the darkness...", false));
        itemList.Add(new Item(9, "Acidic Juice", "It burns when I rub my hands with it...", "That would be a waste of this acidic juice...", "Toolbox", "It has unlocked the toolboxes secrets...", "I miss the old key...", false));
        itemList.Add(new Item(10, "ELEMENT A", "It burns when I rub my hands with it...", "I don't think it's for that...", "Test Tube", "It has unlocked the toolboxes secrets...", "I miss the old key...", false));
        itemList.Add(new Item(11, "ELEMENT B", "It burns when I rub my hands with it...", "I don't think it's for that...", "Test Tube", "It has unlocked the toolboxes secrets...", "I miss the old key...", false));
        itemList.Add(new Item(12, "ELEMENT C", "It burns when I rub my hands with it...", "I don't think it's for that...", "Test Tube", "It has unlocked the toolboxes secrets...", "I miss the old key...", false));

    }
}
