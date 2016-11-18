using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    private bool[] m_upgrades = new bool[4] { false, false, false, false };
    private List<Item> m_items = new List<Item>();
    private int m_ncollectables = 0;

    void Awake()
    {
        
    }

	public void addItem(Item i)
    {
        if(typeof(Item) == i.GetType() )
            m_items.Add(i);
    }

    public List<Item> getItems()
    {
        return m_items;
    }

    public Item takeItem(int n)
    {
        if (n < m_items.Count)
        {
            Item tmp = m_items.ToArray()[n];
            m_items.RemoveAt(n);
            return tmp;
        }
        return null;
    }

    public void score()
    {
        m_ncollectables++;
    }

    public int getScore()
    {
        return m_ncollectables;
    }

    public bool hasUpgrade(int n)
    {
        if(n < 4)
        {
            return m_upgrades[n];
        }
        return false;
    }

    public bool[] getUpgrades() { return m_upgrades; }




}
