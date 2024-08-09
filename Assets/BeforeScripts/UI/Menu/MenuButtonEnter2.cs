using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonEnter2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{    
    Color32 enterColor;
    Color32 exitColor;

    void Start()
    {
        enterColor = new Color32(130, 255, 157, 255);
        exitColor = new Color32(255, 255, 255, 255);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<RawImage>().color = enterColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<RawImage>().color = exitColor;
    }
    
}
