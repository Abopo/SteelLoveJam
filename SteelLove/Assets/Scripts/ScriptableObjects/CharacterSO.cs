using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character")]
public class CharacterSO : DescriptionBaseSO
{
    public GameObject ShipPrefab => _shipPrefab;
    [SerializeField] private GameObject _shipPrefab;
    [SerializeField] private int _seasonPoints;
}
