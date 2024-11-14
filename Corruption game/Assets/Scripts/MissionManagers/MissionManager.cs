using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionManager : MonoBehaviour
{
    [SerializeField] GameEventSO _onMissionCompleted;
    [SerializeField] GameEventSO _onMissionFailed;
    public void CompleteMission()
    {
        _onMissionCompleted?.Raise();
    }
    public void failMission()
    {
        _onMissionFailed?.Raise();
    }
}
