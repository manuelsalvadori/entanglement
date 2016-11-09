using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;
    public bool m_3D_mode = false;
    public bool m_double_mode = false;
    public bool m_single_mode = true;
    public bool m_sel_pg = true;

	void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	}
}
