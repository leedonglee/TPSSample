using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuMain : MonoBehaviour
{
    public GameObject BtnESC;

    public GameObject BtnPlay;
    public GameObject BtnCredits;
    public GameObject BtnQuit;

    string BtnPlayText;
    string BtnCreditsText;
    string BtnQuitText;

    void Awake()
    {
        TextInit();
    }

    void Start()
    {
        BtnESC.GetComponent<MenuButton>().evt.AddListener(EventBtnESC);

        BtnPlay.GetComponent<MenuButton>().evt.AddListener(EventBtnPlay);
        BtnCredits.GetComponent<MenuButton>().evt.AddListener(EventBtnCredits);
        BtnQuit.GetComponent<MenuButton>().evt.AddListener(EventBtnQuit);
    }

    public void TextInit()
    {
        BtnPlayText = GameManager.s_GM.GetComponent<AllText>().GetText(BtnPlay.GetComponent<TextMeshProUGUI>(), 0);
        BtnCreditsText = GameManager.s_GM.GetComponent<AllText>().GetText(BtnCredits.GetComponent<TextMeshProUGUI>(), 1);
        BtnQuitText = GameManager.s_GM.GetComponent<AllText>().GetText(BtnQuit.GetComponent<TextMeshProUGUI>(), 2);
    }

    void OnEnable()
    {
        StartCoroutine(EventStart(BtnPlay.GetComponent<TextMeshProUGUI>(), BtnPlayText));
        StartCoroutine(EventStart(BtnCredits.GetComponent<TextMeshProUGUI>(), BtnCreditsText));
        StartCoroutine(EventStart(BtnQuit.GetComponent<TextMeshProUGUI>(), BtnQuitText));

        BtnPlay.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        BtnCredits.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        BtnQuit.GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
    }

    // Update is called once per frame
    void OnDisable()
    {
        BtnPlay.GetComponent<TextMeshProUGUI>().text = "";
        BtnCredits.GetComponent<TextMeshProUGUI>().text = "";
        BtnQuit.GetComponent<TextMeshProUGUI>().text = "";
    }

    void EventBtnESC()
    {
        Menu.instance.MenuChange(Menu.instance.MenuSettings);
    }

    void EventBtnPlay()
    {
        Menu.instance.MenuChange(Menu.instance.MenuPlay);
    }

    void EventBtnCredits()
    {
        Menu.instance.MenuChange(Menu.instance.MenuCredits);
    }

    void EventBtnQuit()
    {
        // 종료
    }

    IEnumerator EventStart(TextMeshProUGUI _tmp, string _text)
    {
        int _i = 1;

        while (true)
        {
            _tmp.text = _text.Substring(0, _i);
            _i++;
            
            if (_i > _text.Length)
            {
                yield break;
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

}