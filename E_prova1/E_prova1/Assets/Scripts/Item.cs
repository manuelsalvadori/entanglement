using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Item{

    private string m_name;
    public Sprite m_this;
    public GameObject m_target;

    public Item()
    {
    }

    public bool testTarget(GameObject other)
    {
        return this.m_target == other;
    }

}
