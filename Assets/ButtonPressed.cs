using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressed : MonoBehaviour {

    public bool isPressed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	public void OnClick()
    {
        isPressed = true;
        Debug.Log("pressed");
    }

}
