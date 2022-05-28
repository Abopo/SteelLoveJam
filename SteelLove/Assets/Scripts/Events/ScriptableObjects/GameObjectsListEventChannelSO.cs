using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/GameObjectList Event Channel")]
public class GameObjectsListEventChannelSO : DescriptionBaseSO
{
    public UnityAction<List<GameObject>> OnEventRaised;

    public void RaiseEvent(List<GameObject> value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
