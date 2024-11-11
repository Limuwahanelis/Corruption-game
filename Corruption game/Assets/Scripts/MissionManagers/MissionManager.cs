using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionManager : MonoBehaviour
{
    [SerializeField] GameEventSO _onMissionCompleted;

    public void CompleteMission()
    {
        _onMissionCompleted?.Raise();
    }
}
