using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/TrackSceneSO Event Channel")]
public class TrackSceneEventChannelSO : DescriptionBaseSO
{
    public UnityAction<TrackSceneSO> OnEventRaised;

    public void RaiseEvent(TrackSceneSO value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
