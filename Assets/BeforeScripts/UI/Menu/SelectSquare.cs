using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectSquare : MonoBehaviour
{
    RawImage img;

    void OnEnable()
    {
        img = GetComponent<RawImage>();
        StartCoroutine(Coroutine()); // OnDisable 상태가 되면 자동으로 코루틴이 종료된다.
    }

    IEnumerator Coroutine()
    {
        bool _sw = false;
        byte _alpha = 0;

        while (true)
        {
            if (_sw) // 255 -> 0
            {
                _alpha--;
                if (_alpha == 100)
                {
                    _sw = false;
                }
            }
            else
            {
                _alpha++;
                if (_alpha == 255)
                {
                    _sw = true;
                }
            }

            img.color = new Color32(0, 255, 255, _alpha);

            yield return new WaitForSeconds(0.005f);
        }
    }

}