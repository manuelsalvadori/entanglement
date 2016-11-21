using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmoothSwitch : MonoBehaviour {

    private Vector3 target;                                                     //Posizione di Arrivo
    private Vector3 camera_Target = Vector3.zero;
    private Vector3 camera_final = Vector3.zero;

    //SmoothDump setting function
    private Vector3 velocity = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;
    private Vector3 velocity3 = Vector3.zero;
    private Vector3 velocity4 = Vector3.zero;
    public float smoothTime = 0.3F;                                     //Amount of smooth

    //~


    //Camera Set
    private Vector3 rotation = Vector3.zero;
    private Vector3 level3D_init_position, level3D_final_position, level2D_position, level_target;
    private bool current_level = true, previous_level = true, treD_Mode = false;

    public float followingSpeed = 1.5f;
    public float m_zDouble = -29f;
    public float m_zSingle = -20f;
    public float m_y3D = 20f;
    public float m_l1z = 10.5f;
    public float m_l1y = -5f;

    public Vector3 m_offset_from_players = Vector3.zero;
    //~

    //Players and Levels infos
    //Editor view
    public Transform m_l1, m_l2;


    private Dictionary<int, Vector3> m_player_position = new Dictionary<int, Vector3>() //easy way to select player initial infos.
    {
        {0, Vector3.zero},
        {1, Vector3.zero},
    };
    //~

    //Variables for buffering selected player's velocity
    private float buffer;
    private bool select = true;

    public float m_Turn_Tolerance = 100f;

    private Dictionary<int, float> amount = new Dictionary<int, float>()
    {
        {0, 0.25f},
        {1, -0.15f},
    };

    //Pop-up massage for players alignement
    public GameObject m_popup_message;

    bool s = true;
    bool t = true;

    void Start()
    {

        m_l1 = GameManager.Instance.m_level1.transform;
        m_l2 = GameManager.Instance.m_level2.transform;
        target = transform.position;
        level2D_position = m_l1.transform.position;
        level3D_init_position = m_l1.position;
        level3D_init_position.z = m_l1z;
        level3D_final_position = m_l1.position;
        level3D_final_position.z = m_l1z;
        level3D_final_position.y = -5.0f;
        level_target = m_l1.position;

        //Take player position

        m_player_position[0] = GameManager.Instance.m_players[0].transform.position;
        m_player_position[1] = GameManager.Instance.m_players[1].transform.position;

        //Initial position of the camera
        Vector3 player_pos = Camera.main.WorldToViewportPoint(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
        player_pos.x += 0.25f;
        player_pos = Camera.main.ViewportToWorldPoint(player_pos);
        camera_Target = new Vector3(player_pos.x, transform.position.y, transform.position.z);
        buffer = 0f;


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
        GameManager.Instance.m_camIsMoving = true;
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
        GameManager.Instance.m_camIsMoving = true;
    }

    private void select_treD_View()
    {
        if (!treD_Mode)
        {
            target.y = m_y3D;
            target.z = 0f;
            target.x = GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.x - 10f;
            rotation = new Vector3(30f, 90f, 0f);
            level_target = level3D_init_position;
            s = true;
            t = true;
            GameManager.Instance.m_3D_mode = true;
            GameManager.Instance.m_double_mode = false;
            GameManager.Instance.m_single_mode = false;
            m_offset_from_players = target - new Vector3(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.x , GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.y, 0f);


            GameManager.Instance.m_gadgetSelection[(GameManager.Instance.m_sel_pg) ? 0 : 1].hideSelectionUI();
            StartCoroutine(GameManager.activateChildMode());
            StartCoroutine(GameManager.alinePlayers());
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
            GameManager.deactivateChildMode();
            GameManager.Instance.m_gadgetSelection[(GameManager.Instance.m_sel_pg) ? 0 : 1].unhideSelectionUI();

        }
        GameManager.Instance.m_camIsMoving = true;
        treD_Mode = !treD_Mode;
    }

    //Display a popup message
    private void displayMessage()
    {
        if(!m_popup_message.activeSelf) Instantiate(m_popup_message, Vector3.zero, m_popup_message.transform.rotation);
        else
        {
            //Fade out last popup then show the next.
            m_popup_message =  Instantiate(m_popup_message, Vector3.zero, m_popup_message.transform.rotation) as GameObject;
        }
    }

    void Update()
    {

        if ((Input.GetKeyDown("1") || (Input.GetButton("L2") && Input.GetButtonDown("Square"))) && !treD_Mode && !GameManager.Instance.m_inventoryIsOpen) //single_view
        {
            select_singleView();
            if (!GameManager.Instance.m_playerswicth)
            {
                GameManager.Instance.m_sel_pg = !GameManager.Instance.m_sel_pg;
                GameObject.Find("GadgetSelection_1").GetComponent<SwitchGadget>().switchSelectionUI();
                GameObject.Find("GadgetSelection_2").GetComponent<SwitchGadget>().switchSelectionUI();
            }
            else
            {
                GameManager.Instance.m_playerswicth = false;
            }
            GameManager.Instance.mirino.GetComponent<Pointing>().resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);

        }

        if((Input.GetKeyDown("2") || (Input.GetButton("L2") && Input.GetButtonDown("Triangle"))) && !treD_Mode && !GameManager.Instance.m_inventoryIsOpen) //double_view
        {
            select_doubleView();
        }

        if ((Input.GetKeyDown("3") || (Input.GetButton("L2") && Input.GetButtonDown("O"))) && !GameManager.Instance.m_inventoryIsOpen) //3D_view
        {
            if (GameManager.Instance.isPlayersInline() || GameManager.Instance.m_3D_mode) select_treD_View(); else displayMessage();
        }

        //Move the view
        if (GameManager.Instance.m_camIsMoving)
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


            //Check if the camera and levels are stil moving [TO FIX]
            //mode_Transition = !(Mathf.Approximately(transform.position.y, target.y) && Mathf.Approximately(transform.position.z, target.z));
            GameManager.Instance.m_camIsMoving = !(Mathf.Abs(transform.position.y - target.y) < 0.05f && Mathf.Abs(transform.position.z - target.z) < 0.05f && Mathf.Abs(m_l1.position.y - level_target.y) < 0.05f);
            GameManager.Instance.m_levelIsMoving = !(Mathf.Abs(m_l1.position.y - level_target.y) < 0.05f);


            if (!GameManager.Instance.m_camIsMoving && GameManager.Instance.m_3D_mode)
            {
                transform.position = target;
                m_l1.position = level_target;
            }
        }

        if (!GameManager.Instance.m_3D_mode)
        {
            //Bufferize player's velocity in order to decide when to move the camera.
            buffer += GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg)? 0 : 1].GetComponent<Rigidbody>().velocity.x;

            if (buffer > m_Turn_Tolerance)
            {
                select = true;
                buffer = 0;
            }
            else if (buffer < -m_Turn_Tolerance)
            {
                select = false;
                buffer = 0;
            }

            Vector3 player_pos = Camera.main.WorldToViewportPoint(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
            player_pos.x += amount[(select)? 0 : 1];
            player_pos = Camera.main.ViewportToWorldPoint(player_pos);
            camera_Target = new Vector3(player_pos.x, target.y, target.z);
        }
        else
        {
            camera_Target = m_offset_from_players + new Vector3(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.x, GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.y, 0f);
        }

        camera_final = Vector3.SmoothDamp(transform.position, camera_Target, ref velocity4, smoothTime/followingSpeed);

    }

    void LateUpdate()
    {
        if (transform.rotation.eulerAngles.y > 88f && transform.rotation.eulerAngles.y < 90f)
        {
            Debug.Log(transform.rotation.eulerAngles.y);
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 0.02f, transform.rotation.eulerAngles.z));
        }

        if (!GameManager.Instance.m_3D_mode)
            transform.position = new Vector3(camera_final.x, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(camera_final.x, transform.position.y, camera_final.z);

    }
}
