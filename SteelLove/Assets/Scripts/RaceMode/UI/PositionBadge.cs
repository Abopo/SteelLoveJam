using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionBadge : MonoBehaviour
{
    [SerializeField] private Image _portraitImage;
    [SerializeField] private GameObject _destroyedMarker;
    [SerializeField] private TMPro.TMP_Text _positionText;

    public void Init(Sprite portrait, int position, bool destroyed)
    {
        _portraitImage.sprite = portrait;
        _positionText.text = GetPositionString(position);
        _destroyedMarker.SetActive(destroyed);
        if(destroyed)
        {
            _portraitImage.color = new Color(.2f, .2f, .2f);
        }
    }

    string GetPositionString(int position)
    {
        string posString;
        if (position == 1)
        {
            posString = position.ToString() + "st";
        }
        else if (position == 2)
        {
            posString = position.ToString() + "nd";
        }
        else if (position == 3)
        {
            posString = position.ToString() + "rd";
        }
        else
        {
            posString = position.ToString() + "th";
        }

        return posString;
    }
}
