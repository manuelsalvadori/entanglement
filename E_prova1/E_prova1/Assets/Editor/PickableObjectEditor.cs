using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PickableObject))]
[CanEditMultipleObjects]
public class PickableObjectEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as PickableObject;
        myScript.isCollectable = GUILayout.Toggle(myScript.isCollectable, "Is Collectable");
        myScript.m_rot = EditorGUILayout.Vector3Field("Rotation", myScript.m_rot);

        if (!myScript.isCollectable)
        {
            myScript.isUpgrade = GUILayout.Toggle(myScript.isUpgrade, "Is Upgrade");
            if (myScript.isUpgrade)
                myScript.m_gadget = (Inventory.Gadgets)EditorGUILayout.EnumPopup("Which Gadget:", myScript.m_gadget);
            else
            {
                myScript.m_target = (GameObject)EditorGUILayout.ObjectField("Target", myScript.m_target, typeof(GameObject), true);
                myScript.m_spriteTmp = (Sprite)EditorGUILayout.ObjectField("Inventory View", myScript.m_spriteTmp, typeof(Sprite), true, new GUILayoutOption[] { GUILayout.MaxHeight(15), GUILayout.MinWidth(100) });
                myScript.m_description = (string)EditorGUILayout.TextField("Description", myScript.m_description, new GUILayoutOption[] { GUILayout.MaxHeight(15), GUILayout.MinWidth(100) });
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
