using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class initControlsSprite : MonoBehaviour
{
    public Sprite joy, key;

	void OnEnable() 
    {
        if (Input.GetJoystickNames().Length > 0)
        {
            GetComponent<Image>().sprite = joy;
        }
        else
        {
            GetComponent<Image>().sprite = key;
        }
	}

}
