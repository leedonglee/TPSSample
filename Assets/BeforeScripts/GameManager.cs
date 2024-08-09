using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
    AwakeScene에만 위치(임시로 MenuScene에)

    독립적인 오브젝트(스크립트)
    
    Save Data, Sound Control, ...
    */

    public static GameManager s_GM;

    public int lang = 0; // 임시
    public int testData = 0; // 임시    

    void Awake()
    {
         Screen.SetResolution(1920, 1080, true);

        DontDestroyOnLoad(gameObject);

        s_GM = this;
        // 데이터 불러오기
        testData = 0;
    }

}