using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class backControlsUI : MonoBehaviour
{
    public GameObject menu;

    void OnEnable()
    {
        GameManager.Instance.isControlOver = true;
    }
    public void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            GameManager.Instance.isControlOver = false;
            menu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
