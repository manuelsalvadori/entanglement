using UnityEngine;
using System.Collections;

public class Pointing : MonoBehaviour
{
    public float speed = 8f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        float va = Input.GetAxis("R_Vertical");
        float ha = Input.GetAxis("R_Horizontal");
        RaycastHit hit;
        Ray raggio = new Ray(transform.position + new Vector3(0f,10f,0f), Vector3.down);
        Physics.Raycast(raggio, out hit);
        if (hit.collider != null)
        {
            Debug.Log("hit"); 
            transform.position = new Vector3(transform.position.x, hit.collider.bounds.max.y,transform.position.z);
        }
        transform.position += (new Vector3(ha, 0f, va).normalized * Time.deltaTime * speed);

	
	}
}
