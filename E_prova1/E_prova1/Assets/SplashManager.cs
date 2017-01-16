using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour {

    public GameObject wg, ps;
    private Animation wga, psa;
    public float s1,s2;

    void Start()
    {
        wga = wg.GetComponent<Animation>();
        psa = ps.GetComponent<Animation>();

        StartCoroutine(polimiSplash());
    }

    private IEnumerator polimiSplash()
    {
        yield return new WaitForSeconds(s1);
        ps.SetActive(true);
        psa.Play("POLIMISplash");
        yield return new WaitUntil(() => !psa.isPlaying);
        yield return new WaitForSeconds(s2);
        ps.SetActive(false);

        wg.SetActive(true);
        wga.Play("WGSplash");
        yield return new WaitUntil(() => !wga.isPlaying);
        yield return new WaitForSeconds(s2);

        SceneManager.LoadScene("SelectionScene");
    }
}
