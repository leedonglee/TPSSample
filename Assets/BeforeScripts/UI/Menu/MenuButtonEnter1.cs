using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtonEnter1 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Color32 enterColor;
    Color32 exitColor;

    void Start()
    {
        enterColor = new Color32(130, 240, 255, 255);
        exitColor = new Color32(255, 255, 255, 255);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<TextMeshProUGUI>().color = enterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<TextMeshProUGUI>().color = exitColor;
    }

}