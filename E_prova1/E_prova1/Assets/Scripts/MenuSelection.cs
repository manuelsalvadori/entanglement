using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelection : MonoBehaviour {

    public EventSystem es;
    public GameObject go;

    private bool buttonSelected;

	void Start ()
    {
        Cursor.visible = false;
        es.SetSelectedGameObject(go);
	}
	
	void Update ()
    {
        if ((Input.GetAxisRaw("Vertical") != 0) && buttonSelected == false)
        {
            es.SetSelectedGameObject(go);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}
