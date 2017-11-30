using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTuft : MonoBehaviour {

    public void ShakeTuft()
    {
        if (GetComponent<iTween>())
            return;

        iTween.ShakePosition(gameObject, new Vector3(0.05f, 0, 0), 0.5f);
    }
}
