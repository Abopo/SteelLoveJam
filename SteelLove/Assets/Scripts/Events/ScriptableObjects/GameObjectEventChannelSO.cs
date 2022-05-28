using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/GameObject Event Channel")]
public class GameObjectEventChannelSO : DescriptionBaseSO
{
    public UnityAction<GameObject> OnEventRaised;

    public void RaiseEvent(GameObject value)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(value);
    }
}
