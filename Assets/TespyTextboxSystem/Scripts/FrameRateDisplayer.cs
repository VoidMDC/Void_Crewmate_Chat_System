using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class FrameRateDisplayer : MonoBehaviour {

    public Text textObj;
    
    private void Start()
    {
        textObj = GetComponent<Text>();
    }

    void Update () {
        int frameRate = (int)(1.0f / Time.smoothDeltaTime);
        textObj.text = "FPS: " + frameRate.ToString();
	
	}
}
