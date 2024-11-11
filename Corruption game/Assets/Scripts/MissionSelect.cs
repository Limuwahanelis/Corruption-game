using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionSelect : MonoBehaviour
{
    [SerializeField] TMP_Text _missionTextField;
    [SerializeField] string _missionDescription;

    public void SelectMission()
    {
        _missionTextField.text = _missionDescription;
    }
}
