using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TechnologyManager : MonoBehaviour
{
    [SerializeField] TMP_Text _technologyPointText;
    [SerializeField] int _technologyPoints;
    public  void IncreaseTechnologyPoints(int value)
    {
        _technologyPoints += value;
        _technologyPointText.text = _technologyPoints.ToString();
    }
}
