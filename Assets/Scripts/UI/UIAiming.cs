using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAiming : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    [SerializeField]
    private GameObject _aim;
    [SerializeField]
    private IPlayer _player;

    private float _alpha = 0f;

    void Update()
    {
        if (_player.OnPistol && _player.OnAiming)
        {
            _alpha += 2f * Time.deltaTime;
        }
        else
        {
            _alpha -= 2f * Time.deltaTime;
        }

        if (_alpha > 0.999f)
        {
            _aim.SetActive(true);
        }
        else if (_aim.activeSelf)
        {
            _aim.SetActive(false);
        }

        _alpha = Mathf.Clamp(_alpha, 0f, 1f);
        _canvasGroup.alpha = _alpha;
    }

}
