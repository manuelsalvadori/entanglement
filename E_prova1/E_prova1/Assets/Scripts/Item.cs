using UnityEngine;
using System.Collections;

public class Item
{

    private string m_name;
    public GameObject m_this;
    public GameObject m_target;

    public bool testTarget(GameObject other)
    {
        return m_target == other;
    }
}
