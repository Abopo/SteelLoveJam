using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Scene/GameSceneSO")]
public class GameSceneSO : DescriptionBaseSO
{
    public string levelName;
    public AssetReference sceneReference;
    public AudioClip sceneMusic;
    public float volume;
}
