using UnityEngine;
using UnityEditor;
using System.Collections;

public class PickableObject : MonoBehaviour{

    public bool isCollectable = false;
    public bool isUpgrade = false;
    public Inventory.Gadgets m_gadget;
    private string m_name;
    public GameObject m_target;

    private Item m_this = new Item();

    public Sprite m_spriteTmp;

    public Item getItem()
    {
        return m_this;
    }

    void Awake()
    {
        m_this.m_this = m_spriteTmp;
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

[CustomEditor(typeof(PickableObject))]
[CanEditMultipleObjects]
public class PickableObjectEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as PickableObject;


        myScript.isCollectable = GUILayout.Toggle(myScript.isCollectable, "Is Collectable");


        if (!myScript.isCollectable)
        {
            myScript.isUpgrade = GUILayout.Toggle(myScript.isUpgrade, "Is Upgrade");
            if (myScript.isUpgrade)
                myScript.m_gadget = (Inventory.Gadgets)EditorGUILayout.EnumPopup("Which Gadget:", myScript.m_gadget);
            else
            {
                myScript.m_target = (GameObject)EditorGUILayout.ObjectField("Target", myScript.m_target, typeof(GameObject), true);
                myScript.m_spriteTmp = (Sprite)EditorGUILayout.ObjectField("Inventory View", myScript.m_spriteTmp, typeof(Sprite), true, new GUILayoutOption[] { GUILayout.MaxHeight(15), GUILayout.MinWidth(100) });
            }
        }
        else
        {
            myScript.isUpgrade = false;
        }

        if (GUILayout.Button("Reset"))
        {
            myScript.isCollectable = false;
            myScript.isUpgrade = false;
            myScript.m_target = null;
            myScript.m_spriteTmp = null;
        }
    }
}
