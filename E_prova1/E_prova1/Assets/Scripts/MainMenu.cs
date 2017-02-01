using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public EventSystem es;
    public GameObject go;
    public GameObject contr;

    private bool buttonSelected;

	void Start ()
    {
        Cursor.visible = false;
        es.SetSelectedGameObject(go);
	}

    void OnEnabled()
    {
        es.SetSelectedGameObject(go);

    }

	void Update ()
    {
        if ((Input.GetAxisRaw("Vertical") != 0) && buttonSelected == false)
        {

            es.SetSelectedGameObject(go);
            buttonSelected = true;
        }

        if (Input.GetButtonDown("Menu"))
        {
            //if(!GetComponents<AudioSource>()[0].isPlaying)
                GetComponents<AudioSource>()[0].Play();
        }

        if (Input.GetButtonDown("Submit"))
        {
            GetComponents<AudioSource>()[1].Play();
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }

    public void resume()
    {
        Camera.main.GetComponent<BlurOptimized>().enabled = false;
        Time.timeScale = 1;
        StartCoroutine(spettaNamen());
    }

    public void exitLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelection");
    }

    public void exitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void controls()
    {
        contr.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator spettaNamen()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
        GameManager.Instance.m_IsWindowOver = false;
    }
}
