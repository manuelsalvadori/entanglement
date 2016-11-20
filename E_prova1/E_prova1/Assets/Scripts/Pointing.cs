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
            transform.position = new Vector3(transform.position.x, (hit.collider.bounds.max.y + gameObject.GetComponent<MeshRenderer>().bounds.extents.y),transform.position.z);
        }

        transform.position += (new Vector3(ha, 0f, va).normalized * Time.deltaTime * speed);
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos = new Vector3(Mathf.Clamp(pos.x, 0.03f, 0.97f), pos.y, pos.z);
        pos = Camera.main.ViewportToWorldPoint(pos);
        transform.position = new Vector3(pos.x, transform.position.y, -4.6f);

        Debug.Log(Camera.main.WorldToViewportPoint(transform.position));
	}
}
