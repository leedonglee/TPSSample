using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AllText : MonoBehaviour
{
    // 텍스트 코드 별 언어보다 언어 별로 텍스트 코드가 관리가 더 용이하다고 생각한다.
    // 빠른 번역 가능.

    TMP_FontAsset lang0Font; // ENG, KOR

    void Awake()
    {
        lang0Font = Resources.Load("Fonts/DefaultFont", typeof(TMP_FontAsset)) as TMP_FontAsset;
    }

    public string GetText(TextMeshProUGUI _tmp, int _code)
    {
        switch (GameManager.s_GM.lang)
        {
            default :
                _tmp.font = lang0Font;
                return ENG(_code);
            case 1 :
                _tmp.font = lang0Font;
                return KOR(_code);
        }
    }

    public string GetOption1Text(TextMeshProUGUI _tmp, int _num)
    {
        switch (_num)
        {
            default :
                return GetText(_tmp, 3);
            case 1 :
                return "BBB";
            case 2 :
                return "CCC";
            case 3 :
                return "DDD";
        }
    }

    string ENG(int _code)
    {
        string[] _return = 
        {
            "Single Play",
            "Credits",
            "Quit",
            "Option A"
        };
        return _return[_code];
    }

    string KOR(int _code)
    {
        string[] _return = 
        {
            "싱글 플레이",
            "크레딧",
            "종료",
            "옵션 A"
        };
        return _return[_code];
    }
    
}