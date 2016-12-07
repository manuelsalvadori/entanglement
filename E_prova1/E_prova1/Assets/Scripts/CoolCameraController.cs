using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolCameraController : MonoBehaviour {

    //Constant Level and Cam Geometry
    public Vector3[] m_Level_Position;
    public Vector3[] m_Wall_Position;
    public Vector3[] m_View;
    public float m_BackOffset = 9f;
    public float m_TopOffset = 6f;
    public float m_WallOffset = -0.44f;
    //~


    private Vector3 m_CameraTarget;                                     //Posizione di Arrivo
    private Vector3 m_CameraRotation;                                   //Rotazione di Arrivo
    private Vector3 m_LevelTarget;                                      //Posizione di Arrivo del livello
    private Vector3 m_WallTarget;

    public enum Stato { First_Player, Second_Player, Doppia, TreD};
    private int m_oldState;

    //SmoothDump setting function
    //private Vector3 velocity = Vector3.zero;
    private Vector3 velocity2 = Vector3.zero;
    private Vector3 velocity3 = Vector3.zero;
    private Vector3 velocity4 = Vector3.zero;
    public float smoothTime = 0.3F;                                     //Amount of smooth
    public float followingSpeed = 1.5f;
    //~

    //Lerp Level1 setting function
    public float m_Level_speed = 10F;
    private float startTime;
    private float journeyLength;
    private float distCovered = 0;
    private float fracJourney = 0;

    private bool m_levelIsMoving = false;

    //Players and Levels infos
    //Editor view
    public Transform m_l1, m_l2;


    private Dictionary<int, Vector3> m_player_position = new Dictionary<int, Vector3>() //easy way to select player initial infos.
    {
        {0, Vector3.zero},
        {1, Vector3.zero},
    };
    //~



    private Vector3 m_offset_from_players;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        GameManager.Instance.m_Current_State = m_oldState = (int)Stato.First_Player;

        m_player_position[0] = GameManager.Instance.m_players[0].transform.position;
        m_player_position[1] = GameManager.Instance.m_players[1].transform.position;

        m_l1 = GameManager.Instance.m_level1.transform;
        m_l2 = GameManager.Instance.m_level2.transform;


        m_Level_Position = new Vector3[3];
        m_Level_Position[0] = m_Level_Position[1] = m_Level_Position[2] = GameManager.Instance.m_level1.transform.position;
        m_Level_Position[1].z = m_Level_Position[2].z = 10.5f;
        m_Level_Position[2].y = -5f;

        m_Wall_Position = new Vector3[2];
        m_Wall_Position[0] = GameObject.FindGameObjectsWithTag("Wall")[1].GetComponent<MeshFilter>().transform.localPosition;
        m_Wall_Position[1] = new Vector3(GameObject.FindGameObjectsWithTag("Wall")[1].transform.localPosition.x, m_WallOffset, GameObject.FindGameObjectsWithTag("Wall")[1].GetComponent<MeshFilter>().transform.localPosition.z);

        m_View = new Vector3[4];
        m_View[0] = new Vector3(0f, GameManager.Instance.m_level1.transform.position.y, -20f);
        m_View[1] = new Vector3(0f, GameManager.Instance.m_level2.transform.position.y, -20f);
        m_View[2] = new Vector3(0f, 0f, -29f);
        m_View[3] = new Vector3(m_player_position[1].x, m_player_position[1].y, 0f);


        Vector3 player_pos = Camera.main.WorldToViewportPoint(m_player_position[0]);
        player_pos.x += 0.15f;
        player_pos = Camera.main.ViewportToWorldPoint(player_pos);

        m_offset_from_players = m_View[3] - new Vector3(m_player_position[0].x, 0f, 0f) ;

        m_LevelTarget = m_l1.position;
        m_WallTarget = m_Wall_Position[0];

        m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
        m_CameraTarget.x = player_pos.x;
    }

	// Update is called once per frame
	void Update () {
        if ((Input.GetKeyDown("1") || (Input.GetButton("L2") && Input.GetButtonDown("Square"))) && !GameManager.Instance.m_inventoryIsOpen && !m_levelIsMoving) //single_view
        {
            select_singleView();
        }

        if ((Input.GetKeyDown("2") || (Input.GetButton("L2") && Input.GetButtonDown("Triangle"))) && !GameManager.Instance.m_3D_mode && !GameManager.Instance.m_inventoryIsOpen && !m_levelIsMoving) //double_view
        {
            select_doubleView();
        }

        if ((Input.GetKeyDown("3") || (Input.GetButton("L2") && Input.GetButtonDown("O"))) && !GameManager.Instance.m_inventoryIsOpen) //3D_view
        {
            if (GameManager.Instance.isPlayersInline() || GameManager.Instance.m_3D_mode) select_treD_View(); else UIGameplayManager.Instance.displayMessage("Non è stato possibile stabilire il contatto.");
        }


        if (GameManager.Instance.m_Current_State != (int)Stato.TreD)
        {
            Vector3 player_pos = Camera.main.WorldToViewportPoint(GameManager.Instance.m_players[GameManager.Instance.m_sel_pg ? 0 : 1].transform.position);
            player_pos.x += 0.15f;
            player_pos = Camera.main.ViewportToWorldPoint(player_pos);
            m_CameraTarget.x = player_pos.x;
        }
        else
            m_CameraTarget = m_offset_from_players + new Vector3(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.x, 0f, 0f);


        if (m_levelIsMoving)
        {
            //Move Level

            distCovered = (Time.time - startTime) * m_Level_speed;
            fracJourney = distCovered / journeyLength;
            fracJourney = (m_LevelTarget == m_Level_Position[1]) ? 1f - Mathf.Cos(fracJourney * Mathf.PI * 0.5f) : Mathf.Sin(fracJourney * Mathf.PI * 0.5f);
            m_l1.position = Vector3.Lerp(m_l1.position, m_LevelTarget, fracJourney);
            //Debug.Log("Level :" + fracJourney + " distance " + journeyLength);

            if (fracJourney >= 0.5f)
            {
                startTime = Time.time;
                m_LevelTarget = (m_oldState == (int)Stato.TreD) ? m_Level_Position[0] : m_Level_Position[2];
            }

            m_levelIsMoving = !(Mathf.Abs((m_l1.position - (m_oldState == (int)Stato.TreD ? m_Level_Position[0] : m_Level_Position[2])).magnitude) < 0.05f) ;
            GameManager.Instance.m_camIsMoving = m_levelIsMoving;

            GameObject.FindGameObjectsWithTag("Wall")[1].GetComponent<MeshFilter>().transform.localPosition = Vector3.SmoothDamp(GameObject.FindGameObjectsWithTag("Wall")[1].GetComponent<MeshFilter>().transform.localPosition, m_WallTarget, ref velocity3, (smoothTime));
        }
        //Debug.Log(m_levelIsMoving);
    }



    void LateUpdate()
    {
        //Debug.Log(m_CameraTarget + " e " + m_offset_from_players);
        //Move the camera (SmoothDamp version)
        transform.position = Vector3.SmoothDamp(transform.position, m_CameraTarget, ref velocity4, smoothTime);
        transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, m_CameraRotation, ref velocity2, smoothTime));


        if (transform.rotation.eulerAngles.y > 88f && transform.rotation.eulerAngles.y < 90f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 0.02f, transform.rotation.eulerAngles.z));
        }

        if (Mathf.Abs((transform.position - m_CameraTarget).magnitude) < 0.05f)
            transform.position = m_CameraTarget;
    }


    private void select_singleView()
    {
        switch (GameManager.Instance.m_Current_State)
        {
            case 0:
            case 1:
                m_oldState = GameManager.Instance.m_Current_State;
                GameManager.Instance.m_Current_State = 1 - GameManager.Instance.m_Current_State;
                GameManager.Instance.m_sel_pg = !GameManager.Instance.m_sel_pg;
                GameObject.Find("GadgetSelection_1").GetComponent<SwitchGadget>().switchSelectionUI();
                GameObject.Find("GadgetSelection_2").GetComponent<SwitchGadget>().switchSelectionUI();
                GameManager.Instance.mirino.GetComponent<Pointing>().resetPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
                GameManager.Instance.pistola.GetComponent<shoot>().resetShootPosition(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position);
                m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
                break;
            case 2:
                m_oldState = GameManager.Instance.m_Current_State;
                GameManager.Instance.m_Current_State = GameManager.Instance.m_sel_pg ? 0 : 1;
                m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
                break;
            case 3:
                return;
            default:
                return;
        }

    }

    private void select_doubleView()
    {
        //salva lo stato precedente ricordati
        switch (GameManager.Instance.m_Current_State)
        {
            case 0:
            case 1:
                m_oldState = GameManager.Instance.m_Current_State;
                GameManager.Instance.m_Current_State = (int)Stato.Doppia;
                m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
                break;
            case 2:
                GameManager.Instance.m_Current_State = GameManager.Instance.m_sel_pg ? 0 : 1;
                m_oldState = (int)Stato.Doppia;
                m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
                break;
            case 3:
                return;
            default:
                return;
        }
    }

    private void select_treD_View()
    {
        if(GameManager.Instance.m_Current_State != (int)Stato.TreD)
        {
            m_oldState =  GameManager.Instance.m_Current_State;
            GameManager.Instance.m_Current_State = (int)Stato.TreD;
            GameManager.Instance.m_3D_mode = true;
            GameManager.Instance.m_camIsMoving = true;
            m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
            m_CameraTarget.y = m_player_position[1].y + m_TopOffset;
            m_CameraTarget.x = GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.x - m_BackOffset;
            m_CameraRotation = new Vector3(18f, 90f, 0f);

            m_offset_from_players = m_CameraTarget - new Vector3(GameManager.Instance.m_players[(GameManager.Instance.m_sel_pg) ? 0 : 1].transform.position.x, 0f, 0f);

            startTime = Time.time;
            m_LevelTarget = m_Level_Position[1];
            journeyLength = Vector3.Distance(m_l1.position, m_LevelTarget);
            m_levelIsMoving = true;


            m_WallTarget = m_Wall_Position[1];


            GameManager.Instance.m_gadgetSelection[(GameManager.Instance.m_sel_pg) ? 0 : 1].hideSelectionUI();
            StartCoroutine(GameManager.activateChildMode());
            StartCoroutine(GameManager.alinePlayers());
        }
        else
        {
            int tmp = m_oldState;
            m_oldState = GameManager.Instance.m_Current_State;
            GameManager.Instance.m_Current_State = tmp;
            GameManager.Instance.m_3D_mode = false;
            GameManager.Instance.m_camIsMoving = true;
            m_CameraTarget = m_View[GameManager.Instance.m_Current_State];
            m_CameraRotation = Vector3.zero;

            startTime = Time.time;
            m_LevelTarget = m_Level_Position[1];
            journeyLength = Vector3.Distance(m_l1.position, m_LevelTarget);
            m_levelIsMoving = true;


            m_WallTarget = m_Wall_Position[0];


            GameManager.deactivateChildMode();
            GameManager.Instance.m_gadgetSelection[(GameManager.Instance.m_sel_pg) ? 0 : 1].unhideSelectionUI();
        }
    }


    public void changeOldState(int state)
    {
        m_oldState = state;
    }

}
