using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPlay : MonoBehaviour
{
    public GameObject BtnESC;

    public GameObject SelectSquare;

    public GameObject StageIcons;

    public GameObject SelectedStageImg;
    public GameObject SelectedStageTitle;
    public GameObject SelectedStageContents;

    public GameObject BtnPlay;
    
    int playStage = 4; // From Save Data

    int selectedStage = 0;

    RectTransform rect;

    void Awake()
    {
        rect = SelectSquare.GetComponent<RectTransform>();
    }

    void Start()
    {
        for (int i = 0; i < playStage; i++)
        {
            StageIcons.transform.GetChild(i).gameObject.GetComponent<Image>().sprite = Resources.Load("UIImages/nayeon", typeof(Sprite)) as Sprite;
            StageIcons.transform.GetChild(i).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            StageIcons.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.SetActive(false);

            // for문에 람다식은 closure problem이 발생함. (같은 변수 i를 계속 주었기 때문에 발생) temp를 이용해서 해결
            int temp = i;
            StageIcons.transform.GetChild(i).gameObject.GetComponent<MenuButton>().evt.AddListener(() => EventBtnIcon(temp));

            BtnPlay.GetComponent<MenuButton>().evt.AddListener(EventBtnPlay);
        }
    }

    void OnEnable()
    {
        EventBtnIcon(0);
        UpdateMenuPlay(0);
    }

    void EventBtnIcon(int _num)
    {
        selectedStage = _num;
        rect.anchoredPosition = new Vector2(-711 + (158 * selectedStage), rect.anchoredPosition.y);
        UpdateMenuPlay(selectedStage);
    }

    void UpdateMenuPlay(int _num)
    {
        SelectedStageImg.GetComponent<Image>().sprite = Resources.Load("UIImages/nayeon", typeof(Sprite)) as Sprite;
        SelectedStageTitle.GetComponent<TextMeshProUGUI>().text = "TEXT";
        SelectedStageContents.GetComponent<TextMeshProUGUI>().text = "TEXT TEXT";        
    }

    void EventBtnPlay()
    {
        LoadingSceneManager.LoadScene("Stage1Opening");
    }

}