using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    /*
    각 Stage Scene에 위치
    */

    public static StageManager s_SM;

    public bool m_CanControl = false;

    public int m_SceneNum = 0;

    public GameObject m_Player;
    public GameObject m_CamRig;

    public GameObject m_UI;

    void Awake()
    {
        s_SM = this;
    }

    void Start()
    {

    }
    
}
