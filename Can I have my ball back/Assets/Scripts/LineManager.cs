using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineManager : MonoBehaviour {

    Text currentTextBox;

    int letterNumber = 0;

    string currentLine;

    bool isUpdatingLine;

    float letterSpeed = 0.05f;

    float lineEndSpeed = 2;

    public void StartLine(string line, Text textBox)
    {
        // if we get another line straight away, clear last text box and cancel all future functions
        if(currentTextBox !=null)
        {
            CancelInvoke();
            EndLine();
        }

        currentLine = line;
        currentTextBox = textBox;
        letterNumber = 0;
        currentTextBox.text = "";
        isUpdatingLine = true;
        UpdateString();
    }
	
    void UpdateString()
    {
        letterNumber++;
        if (letterNumber > currentLine.Length)
        {
            Invoke("EndLine", lineEndSpeed);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().StopTalking();
            return;
        }

        currentTextBox.text = currentLine.Substring(0, letterNumber);
        Invoke("UpdateString", letterSpeed);
    }

    void EndLine()
    {
        currentTextBox.text = "";
    }

}
