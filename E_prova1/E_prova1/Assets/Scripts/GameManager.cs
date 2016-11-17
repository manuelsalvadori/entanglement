﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;
    public bool m_camIsMoving = false;
    public bool m_3D_mode = false;
    public bool m_double_mode = false;
    public bool m_single_mode = true;
    public bool m_sel_pg = true;


    public GameObject[] m_players;
    public GameObject m_level1;
    public GameObject m_level2;

    public GameObject m_gameplay_UI_Canvas;

    //Struct to collect in GameManager elements of the GUI...trick to visualize it in the Inspector 
    [System.Serializable]
    public struct NamedElements
    {
        public string name;
        public GameObject element;
    }

    public NamedElements[] m_UI_Elements;

    //Real implementation
    public Dictionary<string, GameObject> m_UI = new Dictionary<string, GameObject>();

	void Awake ()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //Filling the pool
        foreach(NamedElements ne in m_UI_Elements)
        {
            m_UI.Add(ne.name, ne.element);
        }
	}
	
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Tab) || (Input.GetButton("L2") && Input.GetButtonDown("X"))) && m_double_mode)
        {
            GameManager.Instance.m_sel_pg = !GameManager.Instance.m_sel_pg;
        }
    }
        
    //Check if the two players are in the "same" x
    public bool isPlayersInline()
    {
        return (Mathf.Abs(m_players[0].transform.position.x - m_players[1].transform.position.x) < 5f);
    }

    public static void breakPlayer()
    {
        Destroy(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<FixedJoint>());
    }

    public static void setScript(bool s)
    {
        GameManager.Instance.m_players[!GameManager.Instance.m_sel_pg ? 0 : 1].GetComponent<PlayerController>().enabled = s;
    }

    public static IEnumerator activateChild()
    {
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => !GameManager.Instance.m_camIsMoving);
        GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].AddComponent<ConfigurableJoint>();
        GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<ConfigurableJoint>().connectedBody = GameManager.Instance.m_players[!(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<Rigidbody>();
        GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<ConfigurableJoint>().xMotion = ConfigurableJointMotion.Locked;
        GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<ConfigurableJoint>().zMotion = ConfigurableJointMotion.Locked;
        GameManager.Instance.m_players[!(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<MeshRenderer>().enabled = false;
        GameManager.Instance.m_players[!(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<PlayerController>().enabled = false;
        GameManager.Instance.m_players[!(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<JumpController>().enabled = true;


        //GameManager.Instance.m_players[1].transform.SetParent(GameManager.Instance.m_players[0].transform);
       // GameManager.Instance.m_players[1].GetComponent<PlayerController>().enabled = false;
    }

    public static void deactivateChild()
    {
        Destroy(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<ConfigurableJoint>());
        GameManager.Instance.m_players[!(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<MeshRenderer>().enabled = true;
        GameManager.Instance.m_players[!(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<PlayerController>().enabled = true;
        GameManager.Instance.m_players[!(GameManager.Instance.m_sel_pg) ? 0 : 1].GetComponent<JumpController>().enabled = false;


        //GameManager.Instance.m_players[1].transform.SetParent(GameManager.Instance.m_players[0].transform);
        // GameManager.Instance.m_players[1].GetComponent<PlayerController>().enabled = false;
    }
       
}
