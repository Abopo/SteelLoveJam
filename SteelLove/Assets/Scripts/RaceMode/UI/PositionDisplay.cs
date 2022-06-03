using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _positionNumberText;
    [SerializeField] private TMPro.TMP_Text _positionSuperScript;

    [Header("Listening To")]
    [SerializeField] private GameObjectsListEventChannelSO _onRacePositionsUpdated;

    private void OnEnable()
    {
        _onRacePositionsUpdated.OnEventRaised += CheckPosition;
    }

    private void OnDisable()
    {
        _onRacePositionsUpdated.OnEventRaised -= CheckPosition;
    }

    private void CheckPosition(List<GameObject> positions)
    {
        int displayPosition = positions.FindIndex(x => x.GetComponent<PlayerShipSetup>() != null) + 1;

        _positionNumberText.text = displayPosition.ToString();

        if (displayPosition == 1)
        {
            _positionSuperScript.text = "st";
        }
        else if (displayPosition == 2)
        {
            _positionSuperScript.text = "nd";
        }
        else if (displayPosition == 3)
        {
            _positionSuperScript.text = "rd";
        }
        else
        {
            _positionSuperScript.text = "th";
        }
    }
}
