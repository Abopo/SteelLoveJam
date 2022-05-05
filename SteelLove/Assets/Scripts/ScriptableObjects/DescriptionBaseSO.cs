using UnityEngine;

/// <summary>
/// Allows for scriptable objects with descriptions for documentation purposes
/// </summary>
public class DescriptionBaseSO : ScriptableObject
{
    [TextArea] public string description;
}
