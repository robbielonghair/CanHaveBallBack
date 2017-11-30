using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerSelector : MonoBehaviour {

    SpriteRenderer head, body, leftArm, rightArm, leftFoot, rightFoot, leftEye, rightEye, leftEyeball, rightEyeball, mouth, shadow;
    SpriteRenderer spriteSizer;

    bool isPlayer;

    Sprite normalEyes, blinkEyes;

    Color hidden = new Color(1, 1, 1, 0);
    Color visible = new Color(1, 1, 1, 1);

    Color shadowStartCol;

	// Use this for initialization
	void Start ()
    {
        SetLayer();
        GetVariables();
        Invoke("Blink", 2f);
        FluctuateMouth();
    }

    public void FadePlayerOut()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", visible, "to", hidden, "time", 0.5f, "onupdate", "UpdateColour"));
        shadowStartCol = shadow.color;
        Invoke("CheckIfLoadRoom", 0.6f);
        iTween.ColorTo(shadow.gameObject, hidden, 0.5f);
    }

    public void CheckIfLoadRoom()
    {
        if(GetComponent<PlayerManager>().GetRoomToLoad() != "")
        {
            GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentScript>().LoadLevel(GetComponent<PlayerManager>().GetRoomToLoad());
        }
    }

    public void UpdateColour(Color tempCol)
    {
        head.color = tempCol;
        body.color = tempCol;
        leftArm.color = tempCol;
        rightArm.color = tempCol;
        leftFoot.color = tempCol;
        rightFoot.color = tempCol;
        leftEye.color = tempCol;
        rightEye.color = tempCol;
        leftEyeball.color = tempCol;
        rightEyeball.color = tempCol;
        mouth.color = tempCol;
    }

    public void FadePlayerIn()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", hidden, "to", visible, "time", 1f, "onupdate", "UpdateColour"));
        iTween.ColorTo(shadow.gameObject, shadowStartCol, 0.5f);

    }

    public void Blink()
    {
        leftEyeball.enabled = false;
        rightEyeball.enabled = false;

        leftEye.sprite = blinkEyes;
        rightEye.sprite = blinkEyes;
        Invoke("OpenEyes", 0.3f);

    }

    public void BlinkIndefinitely()
    {
        leftEyeball.enabled = false;
        rightEyeball.enabled = false;

        leftEye.sprite = blinkEyes;
        rightEye.sprite = blinkEyes;

        leftEye.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        rightEye.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        Destroy(leftEye.transform.Find("Mask").gameObject);
        Destroy(rightEye.transform.Find("Mask").gameObject);

        //make arms rotatedupwards
        leftArm.transform.parent.parent.Find("Raised Arms").gameObject.SetActive(true);
        leftArm.transform.parent.gameObject.SetActive(false);
        rightArm.transform.parent.gameObject.SetActive(false);

        GameObject.Find("Player").GetComponent<PlayerManager>().MakeMouthOpen();
    }

    void OpenEyes()
    {
        leftEyeball.enabled = true;
        rightEyeball.enabled = true;

        leftEye.sprite = normalEyes;
        rightEye.sprite = normalEyes;
        Invoke("Blink", 5f);
    }

    void FluctuateMouth()
    {
        if (!isPlayer)
            return;

        iTween.ScaleTo(mouth.gameObject, iTween.Hash("scale", new Vector3(1.2f, 1, 1), "time", 2f, "looptype", iTween.LoopType.pingPong));
    }

    void SetLayer()
    {
        if (!GetComponent<SpriteRenderer>())
            return;

        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.z * 100f) * -1;
    }

    void GetVariables()
    {
        if (!transform.tag.Equals("Player"))
        {
            Destroy(GetComponent<SpriteLayerSelector>());
            return;
        }

        isPlayer = true;
        head = transform.Find("Head").GetComponent<SpriteRenderer>();

        body = transform.Find("Body").GetComponent<SpriteRenderer>();

        mouth = transform.Find("Mouth").GetComponent<SpriteRenderer>();

        leftEye = transform.Find("Eye1").GetComponent<SpriteRenderer>();
        leftEyeball = leftEye.transform.Find("Eyeball").GetComponent<SpriteRenderer>();

        rightEye = transform.Find("Eye2").GetComponent<SpriteRenderer>();
        rightEyeball = rightEye.transform.Find("Eyeball").GetComponent<SpriteRenderer>();

        leftArm = transform.Find("ArmLeft").Find("Arm").GetComponent<SpriteRenderer>();
        rightArm = transform.Find("ArmRight").Find("Arm").GetComponent<SpriteRenderer>();

        leftFoot = transform.Find("FootLeft").Find("Foot").GetComponent<SpriteRenderer>();
        rightFoot = transform.Find("FootRight").Find("Foot").GetComponent<SpriteRenderer>();
        spriteSizer = transform.Find("PlayerSpriteSizer").GetComponent<SpriteRenderer>();

        shadow = transform.Find("Shadow").GetComponent<SpriteRenderer>();

        normalEyes = rightEye.sprite;

        foreach(Sprite current in Resources.LoadAll("Art/Garden/Player", typeof(Sprite)))
        {
            if (current.name.Equals("eyeclosed"))
                blinkEyes = current;
        }
    }

    //head above body, arms and feet below body, eyes and mouth above head
    void GetPlayerLayers()
    {
        body.sortingOrder = Mathf.RoundToInt(spriteSizer.transform.position.z * 100f) * -1;
        head.sortingOrder = body.sortingOrder + 1;
        leftEye.sortingOrder = head.sortingOrder + 1;
        leftEyeball.sortingOrder = leftEye.sortingOrder + 1;
        rightEye.sortingOrder = leftEye.sortingOrder;
        rightEyeball.sortingOrder = leftEyeball.sortingOrder;
        leftArm.sortingOrder = body.sortingOrder - 1;
        rightArm.sortingOrder = leftArm.sortingOrder;
        leftFoot.sortingOrder = leftArm.sortingOrder;
        rightFoot.sortingOrder = leftArm.sortingOrder;
        mouth.sortingOrder = head.sortingOrder + 1;
    }

    // Update is called once per frame
    void Update () {
        if (!isPlayer)
            return;

        GetPlayerLayers();
	}
}
