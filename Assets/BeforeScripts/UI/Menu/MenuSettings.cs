using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuSettings : MonoBehaviour
{   
    public GameObject BtnESC;
    // public TextMeshProUGUI TMPSettName;
    public GameObject BtnSettLeft;
    public GameObject BtnSettRight;
    int sett = 0;

    public GameObject Settings;

    public GameObject Sett1;
    public GameObject Sett2;

    public TextMeshProUGUI TMPOption1;
    public GameObject BtnOption1L;
    public GameObject BtnOption1R;
    int option1 = 0;

    void Awake()
    {
        TextInit();
    }

    void Start()
    {
        BtnSettLeft.GetComponent<MenuButton>().evt.AddListener(EventBtnSettL);
        BtnSettRight.GetComponent<MenuButton>().evt.AddListener(EventBtnSettR);

        BtnOption1L.GetComponent<MenuButton>().evt.AddListener(EventBtnOption1L);
        BtnOption1R.GetComponent<MenuButton>().evt.AddListener(EventBtnOption1R);
    }

    void OnEnable()
    {
        
    }

    void OnDisable()
    {

    }

    public void TextInit()
    {
        TMPOption1.text = GameManager.s_GM.GetComponent<AllText>().GetOption1Text(TMPOption1, option1);
    }

    public void UpdateSett()
    {
        for (int i = 0; i < Settings.transform.childCount; i++)
        {
            Settings.transform.GetChild(i).gameObject.SetActive(false);
        }
        Settings.transform.GetChild(sett).gameObject.SetActive(true);
    }

    void EventBtnSettL()
    {
        if (sett == 0)
            sett = 1;
        else
            sett--;
        UpdateSett();
    }

    void EventBtnSettR()
    {
        if (sett == 1)
            sett = 0;
        else
            sett++;
        UpdateSett();
    }

    void EventBtnOption1L()
    {
        if (option1 == 0)
            option1 = 3;
        else
            option1--;
        TextInit();
    }

    void EventBtnOption1R()
    {
        if (option1 == 3)
            option1 = 0;
        else
            option1++;
        TextInit();
    }

}