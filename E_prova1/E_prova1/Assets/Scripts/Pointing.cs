using UnityEngine;
using System.Collections;

public class Pointing : MonoBehaviour
{
    public float speed = 8f;
	
	void Update ()
    {
        float va = Input.GetAxis("R_Vertical");
        float ha = Input.GetAxis("R_Horizontal");
        RaycastHit hit;
        Ray raggio = new Ray(transform.position + new Vector3(0f,10f,0f), Vector3.down);
        Physics.Raycast(raggio, out hit);
        if (hit.collider != null)
        {
            transform.position = new Vector3(transform.position.x, hit.collider.bounds.max.y,transform.position.z);
        }
        transform.position += (new Vector3(ha, 0f, va).normalized * Time.deltaTime * speed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -4.6f);
	}
}
