using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UnitData")]
public class UnitData : ScriptableObject
{
    public Unit UnitPrefab => _unitPrefab;
    [SerializeField] Unit _unitPrefab;
}