using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaMuerte : MonoBehaviour {

    private Texture2D[] frames;
    private Texture2D[] frames_emit;
    private Texture2D[] frames_normal;
    public float framesPerSecond = 120f;

    private void Awake()
    {
        frames = new Texture2D[300];
        frames_emit = new Texture2D[300];
        frames_normal = new Texture2D[300];

        for (int i = 0; i < 300; i++)
        {
            string tmp = "LaMuerte_Final/LaMuerte_" + i.ToString("00000");
            string tmp2 = "LaMuerteFinalEmit/LaMuerteFinalEmit_" + i.ToString("00000");

            //Debug.Log(tmp);
            Texture2D tmp_albedo = Resources.Load(tmp) as Texture2D;
            Texture2D tmp_emit = Resources.Load(tmp2) as Texture2D;


            frames[i] = tmp_albedo;
            frames_emit[i] = tmp_emit;

            //frames_emit[i] = Resources.Load("LaMuerteFinalEmit\\LaMuerteFinalEmit_" + i.ToString("00000")) as Texture2D;
        }
    }

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
        GetComponent<Renderer>().material.EnableKeyword("_SPECGLOSSMAP");
        GetComponent<Renderer>().material.EnableKeyword("_PARALLAXMAP");
    }

    // Update is called once per frame
    void Update () {
        int index = (int)(Time.time * framesPerSecond) % frames.Length;

        GetComponent<Renderer>().material.mainTexture = frames[index];
        GetComponent<Renderer>().material.SetTexture("_EmissionMap", frames_emit[index]);
        GetComponent<Renderer>().material.SetTexture("_SpecGlossMap", frames_emit[index]);
        GetComponent<Renderer>().material.SetTexture("_ParallaxMap", frames_emit[index]);

    }
}
