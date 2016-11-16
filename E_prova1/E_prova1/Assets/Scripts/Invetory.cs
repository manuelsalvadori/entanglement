using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Invetory : MonoBehaviour {

    private bool[] m_upgrades;
    private List<Item> m_items;
    private int m_ncollectables = 0;

    void Awake()
    {
        m_upgrades = new bool[4] { false, false, false, false };
        m_items = new List<Item>();
        m_ncollectables = 0;
    }

	void addItem(Item i)
    {
        m_items.Add(i);
    }

    List<Item> getItems()
    {
        return m_items;
    }

    Item takeItem(int n)
    {
        if (n < m_items.Count)
        {
            Item tmp = m_items.ToArray()[n];
            m_items.RemoveAt(n);
            return tmp;
        }
        return null;
    }

    void score()
    {
        m_ncollectables++;
    }

    int getScore()
    {
        return m_ncollectables;
    }

    bool hasUpgrade(int n)
    {
        if(n < 4)
        {
            return m_upgrades[n];
        }
        return false;
    }

    bool[] getUpgrades() { return m_upgrades; }




}
