using UnityEngine;
using System.Collections;

public class PickableObject : MonoBehaviour{

    public bool isCollectable = false;
    private string m_name;
    public GameObject m_target;

    private Item m_this;

    void Awake()
    {
        if (!isCollectable)
        {
            m_this = new Item(gameObject);
            m_this.m_target = m_target;
        }
    }

    public Item getItem()
    {
        return m_this;
    }

    public bool testTarget(GameObject other)
    {
        return m_target == other;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

}
