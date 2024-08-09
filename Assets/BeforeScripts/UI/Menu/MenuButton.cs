using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent evt;

    public void OnPointerDown(PointerEventData eventData)
    {
        evt.Invoke();
    }
}
