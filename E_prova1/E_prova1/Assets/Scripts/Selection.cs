using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{

    public GameObject bottoni;
    public GameObject loading;
    public GameObject creditz;
    public Transform loadingbar;
    public GameObject em, loadUI;
    public Text percento;

    public void sceneselection()
    {
        SceneManager.LoadScene("SelectionScene");
    }

    public void startbutton()
    {
        bottoni.SetActive(false);
        loading.SetActive(true);
        DestroyImmediate(em);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("cp1", 0);
        PlayerPrefs.SetInt("cp2", 0);
        StartCoroutine(loadAsync("introduction"));
    }

    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void exit()
    {
        Application.Quit();
    }

    private IEnumerator loadAsync(string levelName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
        while(!operation.isDone)
        {
            yield return operation.isDone;
            loadingbar.GetComponent<Image>().fillAmount = operation.progress;
            percento.text = ((int)(operation.progress * 100f)).ToString() + "%";
        }
        loadingbar.GetComponent<Image>().fillAmount = 1f;
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator loadAsyncint(int l)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(l);
        while(!operation.isDone)
        {
            yield return operation.isDone;
            loadingbar.GetComponent<Image>().fillAmount = operation.progress;
            percento.text = ((int)(operation.progress * 100f)).ToString() + "%";
        }
        loadingbar.GetComponent<Image>().fillAmount = 1f;
        yield return new WaitForSeconds(0.2f);
    }

    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void loadGame()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            int level = PlayerPrefs.GetInt("Level");
            if (level == 1)
                StartCoroutine(notLoaded());
            else
            {
                bottoni.SetActive(false);
                loading.SetActive(true);
                DestroyImmediate(em);
                StartCoroutine(loadAsyncint(level));
            }
        }
        else
        {
            StartCoroutine(notLoaded());
        }
    }

    IEnumerator notLoaded()
    {
        yield return new WaitForSeconds(0.2f);
        loadUI.SetActive(true);
        loadUI.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(2f);
        loadUI.SetActive(false);
    }
}
