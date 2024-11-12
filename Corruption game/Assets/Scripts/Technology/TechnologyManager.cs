using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TechnologyManager : MonoBehaviour
{
    public int TechnologyPoints => _technologyPoints;
    [SerializeField] TMP_Text _technologyPointText;
    [SerializeField] int _technologyPoints;
    public  void IncreaseTechnologyPoints(int value)
    {
        _technologyPoints += value;
        _technologyPointText.text = _technologyPoints.ToString();
    }
    public bool TryUnlockTechnology(int cost)
    {
        if (cost <= _technologyPoints)
        {
            _technologyPoints -= cost;
            _technologyPointText.text = _technologyPoints.ToString();
            return true;
        }
        return false;
    }
}
