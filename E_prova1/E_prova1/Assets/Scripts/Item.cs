﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Item{

    private string m_name;
    public Sprite m_this;
    public GameObject m_target;

    public Item(string name)
    {
        this.m_name = name;
    }

    public void use()
    {
        Vector3 p_pos = GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.position;
        if((p_pos - m_target.transform.position).magnitude < 3f)
        {
            //m_target.GetComponent<ActivableObject>().activate();
        }
        else
        {
            UIGameplayManager.Instance.displayMessage("C'è luogo e momento per ogni cosa ma non ora!");
        }
    }

    public bool testTarget(GameObject other)
    {
        return this.m_target == other;
    }

    public override string ToString()
    {
        return m_name;
    }

}
