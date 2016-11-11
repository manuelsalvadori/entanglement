using UnityEngine;
using System.Collections;

public class SmoothSwitch : MonoBehaviour {

    Vector3 target;                                                     //Posizione di Arrivo
    public float smoothTime = 0.3F;                                     //Amount of smooth

    //Puntators for SmoothDump function
    private Vector3 velocity = Vector3.zero;                            
    private Vector3 velocity2 = Vector3.zero;
    private Vector3 velocity3 = Vector3.zero;

    //Camera Set
    private Vector3 rotation = Vector3.zero;
    private Vector3 level3D_init_position, level3D_final_position, level2D_position, level_target;
    private bool mode_Transition = false, current_level = true, previous_level = true, treD_Mode = false;

    public float m_zDouble = -29f;
    public float m_zSingle = -20f;
    public float m_y3D = 20f;
    public float m_l1z = 10.5f;
    public float m_l1y = -5f;
    public Transform m_l1, m_l2;
    public Transform m_p1, m_p2;

    private Vector3 camera_Target = Vector3.zero;

    private Vector3 m_p1_init_position, m_p2_init_position;

    bool s = true;
    bool t = true;

    void Start()
    {
        target = transform.position;
        level2D_position = m_l1.transform.position;
        level3D_init_position = m_l1.position;
        level3D_init_position.z = m_l1z;
        level3D_final_position = m_l1.position;
        level3D_final_position.z = m_l1z;
        level3D_final_position.y = -5.0f;
        level_target = m_l1.position;

        //Take player position
        m_p1_init_position = m_p1.position;
        m_p2_init_position = m_p2.position;

    }


    private void select_doubleView()
    {
        if (previous_level)
        {
            GameManager.Instance.m_double_mode = true;
            GameManager.Instance.m_single_mode = false;
            target.y = 0f;
            target.z = m_zDouble;
            previous_level = false;
        }
        else
        {
            GameManager.Instance.m_double_mode = false;
            GameManager.Instance.m_single_mode = true;
            target.z = m_zSingle;
            target.y = current_level ? m_l1.position.y : m_l2.position.y;
            previous_level = true;
        }
        mode_Transition = true;
    }

    private void select_singleView()
    {
        GameManager.Instance.m_single_mode = true;
        GameManager.Instance.m_double_mode = false;

        if (current_level)
        {
            target.y = m_l2.position.y;
            current_level = false;
        }
        else
        {
            target.y = m_l1.position.y;
            current_level = true;
        }
        target.z = m_zSingle;
        mode_Transition = true;
    }

    private void select_treD_View()
    {
        if (!treD_Mode)
        {
            target.y = m_y3D;
            target.z = 0f;
            rotation = new Vector3(30f, 90f, 0f);
            level_target = level3D_init_position;
            s = true;
            t = true;
            GameManager.Instance.m_3D_mode = true;
            GameManager.Instance.m_double_mode = false;
            GameManager.Instance.m_single_mode = false;
        }
        else
        {
            target.y = 0f;
            target.z = m_zDouble;
            rotation = Vector3.zero;
            level_target = level3D_init_position;
            t = false;
            s = false;
            GameManager.Instance.m_3D_mode = false;
            GameManager.Instance.m_double_mode = true;

        }
        mode_Transition = true;
        treD_Mode = !treD_Mode;
    }



    void Update()
    {
        if(Input.GetKeyDown("x") && !treD_Mode) //double_view
        {
            select_doubleView();
        }

        if (Input.GetKeyDown("c") && !treD_Mode) //single_view
        {
            select_singleView();
        }

        if (Input.GetKeyDown("z")) //3D_view
        {
            select_treD_View();
        }
              

        //Move the view
        if (mode_Transition)
        {

            //Move the level (select the target for 3 point movement)
            if (Mathf.Approximately(m_l1.position.z , level3D_init_position.z) && s)
            {
                //Debug.Log("da l3d1 a l3df");
                level_target = level3D_final_position;
                s = false;
            }
            if (Mathf.Approximately(m_l1.position.y , level3D_init_position.y) && !t)
            {
                //Debug.Log("da l3d1 a l2d");
                level_target = level2D_position;
                t = true;
            }


            //Move the camera (SmoothDamp version)
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime); 
            transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, rotation, ref velocity2, smoothTime));

            //Move the level - in Action
            m_l1.position = Vector3.SmoothDamp(m_l1.position, level_target, ref velocity3, (smoothTime/4f));


            //Check if the camera is stil moving
            mode_Transition = !(Mathf.Approximately(transform.position.y, target.y) && Mathf.Approximately(transform.position.z, target.z));
            Debug.Log(mode_Transition.ToString());
        }

        if (!GameManager.Instance.m_3D_mode)
        {
            if (m_p1.position.x >= m_p1_init_position.x)
            {
                camera_Target = new Vector3(Camera.main.ViewportToScreenPoint(m_p1.position).x / 2, transform.position.y, transform.position.z); 
            }
        }
    }

    void LateUpdate()
    {
        /*
        if (!GameManager.Instance.m_3D_mode)
            transform.position = new Vector3(m_p1.position.x, transform.position.y, transform.position.z);
        */
        transform.position = camera_Target;
    }
}
