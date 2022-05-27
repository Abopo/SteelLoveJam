using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/GameSceneSO Event Channel")]
public class GameSceneEventChannelSO : DescriptionBaseSO
{
    public UnityAction<GameSceneSO> OnEventRaised;

    public void RaiseEvent(GameSceneSO value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
