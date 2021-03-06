﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSelection : MonoBehaviour {

    public EventSystem es;
    public GameObject go;
    public AudioSource src;
    public Animation logoAnim;
    public GameObject bottoni;
    public bool isMenu = true;

    private bool buttonSelected;

	void Start ()
    {
        Cursor.visible = false;
        es.SetSelectedGameObject(go);
        src.PlayDelayed(0.5f);
        if (isMenu)
        {
            StartCoroutine(animationMenu());
        }
        Inventory.m_ncollectables = new int[6] { 0, 0, 0, 0, 0, 0 };
	}

	void Update ()
    {
        if ((Input.GetAxisRaw("Vertical") != 0) && buttonSelected == false)
        {
            es.SetSelectedGameObject(go);
            buttonSelected = true;
        }

        if (Input.GetButtonDown("MenuHorizontal") || Input.GetButtonDown("MenuVertical"))
        {
            if(!GetComponents<AudioSource>()[0].isPlaying)
                GetComponents<AudioSource>()[0].Play();
        }

        if (Input.GetButton("Submit"))
        {
            if(!GetComponents<AudioSource>()[1].isPlaying)
                GetComponents<AudioSource>()[1].Play();

        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }

    private IEnumerator animationMenu()
    {
        yield return new WaitForSeconds(0.2f);

        logoAnim.Play("LogoAnimation");

        yield return new WaitUntil(() => !logoAnim.isPlaying);
        yield return new WaitForSeconds(1f);

        logoAnim.Play("LogoAnimationAlpha");

        yield return new WaitUntil(() => !logoAnim.isPlaying);

        bottoni.SetActive(true);
        es.SetSelectedGameObject(go);
    }
}
