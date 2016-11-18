using UnityEngine;
using System.Collections;

public class Item{

    private string m_name;
    public GameObject m_this;
    public GameObject m_target;

    public Item(GameObject go)
    {
        this.m_this = go;
    }

    public bool testTarget(GameObject other)
    {
        return this.m_target == other;
    }

}
