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
    public GameObject gUI_1, gUI_2;
    public GameObject savedUI;



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
        if ((Input.GetAxisRaw("MenuVertical") != 0) && buttonSelected == false)
        {

            es.SetSelectedGameObject(go);
            buttonSelected = true;
        }


        if ((Input.GetAxisRaw("MenuVertical") != 0))
        {
            if(!GetComponents<AudioSource>()[0].isPlaying)
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
        if (!GameManager.Instance.m_3D_mode && gUI_1 != null)
        {
            gUI_1.SetActive(true);
            gUI_2.SetActive(true);
        }
        gameObject.SetActive(false);
        GameManager.Instance.m_IsWindowOver = false;
    }

    public void saveGame()
    {
        GameManager.Instance.SaveGame();
        StartCoroutine(saved());
        resume();
    }

    IEnumerator saved()
    {
        yield return new WaitForSeconds(0.2f);
        savedUI.SetActive(true);
        savedUI.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(2f);
        savedUI.SetActive(false);
    }

    public void resumeLS()
    {
        Camera.main.GetComponent<BlurOptimized>().enabled = false;
        Time.timeScale = 1;
        StartCoroutine(spettaNamenLS());
    }

    IEnumerator spettaNamenLS()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

    public void saveLS()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        resumeLS();
    }
}
