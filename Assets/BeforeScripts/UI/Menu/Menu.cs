using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    /*
    Menu Controller
    주로 Menu 화면 관리(이동, 전환 등)
    */

    public static Menu instance;

    private bool canMove;

    public GameObject MainCamera;
    
    public GameObject Menus;

    public GameObject MenuMain;
    public GameObject MenuSettings;
    public GameObject MenuPlay;
    public GameObject MenuCredits;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Init (CoroutineA)
        // 1. Menus의 CanvasGroup는 0, 모든 Menu들 비활성화 상태에서 시작(Menu들 OnEnable, OnDisable로 관리)
        // 2. MenuMain외 다른 Menu들 비활성화
        // 3. Menus의 CanvasGroup 0에서 1로 페이드
        
        canMove = false;
        StartCoroutine(CoroutineA());
    }

    void Update()
    {
        // Menu Change (CoroutineB)
        // 1. Menus의 CanvasGroup 1에서 0로 페이드
        // 2. 이동할 Menu외 다른 Menu들 비활성화
        // 3. Menus의 CanvasGroup 0에서 1로 페이드

        if (MenuMain.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            MenuChange(MenuSettings);
        }
        else if (MenuSettings.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            MenuChange(MenuMain);
        }
        else if (MenuPlay.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            MenuChange(MenuMain);
        }
        else if (MenuCredits.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            MenuChange(MenuMain);
        }
    }

    public void MenuChange(GameObject _GoMenu)
    {
        if (canMove)
        {
            canMove = false;
            StartCoroutine(CoroutineB(_GoMenu));

            // Camera Blur (CoroutineC)
            // 1. 이동 전 MenuMain이 활성화 되어있으면 Blur on
            // 2. 이동 후 Menu(_GoMenu)가 MenuMain이면 Blur off
            
            if (MenuMain.activeSelf)
            {
                StartCoroutine(CoroutineC(true));
            }
            else if (_GoMenu == MenuMain)
            {
                StartCoroutine(CoroutineC(false));
            }
        }
    }

    IEnumerator CoroutineA()
    {
        MenuMain.SetActive(true);

        CanvasGroup _canvasGroup = Menus.GetComponent<CanvasGroup>();
        float _alpha = 0f;

        while (true)
        {
            if (_alpha < 1f)
            {
                _alpha += 0.02f;
                _canvasGroup.alpha = _alpha;
                yield return new WaitForSeconds(0.01f);
            }
            else
            {
                canMove = true;
                yield break;
            }
        }
    }

    IEnumerator CoroutineB(GameObject _GoMenu)
    {
        bool _sw = false; // alpha 1 -> 0 sw 0 -> 1

        CanvasGroup _canvasGroup = Menus.GetComponent<CanvasGroup>();
        float _alpha = 1f;

        while (true)
        {
            if (!_sw)
            {
                if (_alpha > 0)
                {
                    _alpha -= 0.02f;
                    _canvasGroup.alpha = _alpha;
                }
                else
                {
                    for (int i = 0; i < Menus.transform.childCount; i++)
                    {
                        Menus.transform.GetChild(i).gameObject.SetActive(false);
                    }

                    _GoMenu.SetActive(true);

                    _sw = true;
                }
            }
            else
            {
                if (_alpha < 1f)
                {
                    _alpha += 0.02f;
                    _canvasGroup.alpha = _alpha;
                }
                else
                {
                    canMove = true;
                    yield break;
                }
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator CoroutineC(bool _isBlur)
    {
        float _alpha = 0;

        if (!_isBlur)
            _alpha = 1f;

        UnityEngine.Rendering.Volume volume = MainCamera.GetComponent<UnityEngine.Rendering.Volume>();

        while (true)
        {
            if (!_isBlur)
            {
                if (_alpha > 0)
                    _alpha -= 0.02f;
                else
                    yield break;
            }
            else
            {
                if (_alpha < 1f)
                    _alpha += 0.02f;
                else
                    yield break;
            }

            volume.weight = _alpha;

            yield return new WaitForSeconds(0.01f);
        }
    }

}