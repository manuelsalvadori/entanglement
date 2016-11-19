using UnityEngine;
using System.Collections;

public class SwitchGadget : MonoBehaviour
{
    bool fw;
    bool primo = true;

	void Update ()
    {
        if (Input.GetButtonDown("Next") && !transform.GetChild(0).GetComponent<Animation>().isPlaying)
        {
            transform.GetChild(0).GetComponent<Animation>()["RigthToCenter"].speed = 1f;
            transform.GetChild(0).GetComponent<Animation>()["RigthToCenter"].time = 0f;
            transform.GetChild(1).GetComponent<Animation>()["CenterToLeft"].speed = 1f;
            transform.GetChild(1).GetComponent<Animation>()["CenterToLeft"].time = 0f;
            transform.GetChild(2).GetComponent<Animation>()["LeftToRigth"].speed = 1f;
            transform.GetChild(2).GetComponent<Animation>()["LeftToRigth"].time = 0f;
            transform.GetChild(0).GetComponent<Animation>().Play("RigthToCenter");
            transform.GetChild(1).GetComponent<Animation>().Play("CenterToLeft");
            transform.GetChild(2).GetComponent<Animation>().Play("LeftToRigth");
        }

        if (Input.GetButtonDown("Previous") && !transform.GetChild(0).GetComponent<Animation>().isPlaying)
        {
            transform.GetChild(0).GetComponent<Animation>()["RigthToCenter"].speed = -1f;
            transform.GetChild(0).GetComponent<Animation>()["RigthToCenter"].time = transform.GetChild(0).GetComponent<Animation>()["RigthToCenter"].length;
            transform.GetChild(1).GetComponent<Animation>()["CenterToLeft"].speed = -1f;
            transform.GetChild(1).GetComponent<Animation>()["CenterToLeft"].time = transform.GetChild(1).GetComponent<Animation>()["CenterToLeft"].length;
            transform.GetChild(2).GetComponent<Animation>()["LeftToRigth"].speed = -1f;
            transform.GetChild(2).GetComponent<Animation>()["LeftToRigth"].time = transform.GetChild(2).GetComponent<Animation>()["LeftToRigth"].length;
            transform.GetChild(0).GetComponent<Animation>().Play("RigthToCenter");
            transform.GetChild(1).GetComponent<Animation>().Play("CenterToLeft");
            transform.GetChild(2).GetComponent<Animation>().Play("LeftToRigth");
        }
            
	}
}
