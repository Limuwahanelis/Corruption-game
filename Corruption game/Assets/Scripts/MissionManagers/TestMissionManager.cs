using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMissionManager : MissionManager
{
    [SerializeField] ListOfSpawners _listOfSpawners;
    int _corruptedSpawners = 0;
    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i< _listOfSpawners.Spawners.Count; i++) 
        {
            CorruptionComponent corruptionCom = _listOfSpawners.Spawners[i].GetComponent<CorruptionComponent>();
            if(corruptionCom.IsCorrupted) _corruptedSpawners++;
            corruptionCom.OnCorrupted.AddListener(OnSpawnerCorrupted);
            corruptionCom.OnUnCorrupted.AddListener(OnSpawnerUnCorrupted);
        }
    }
    private void OnSpawnerCorrupted(CorruptionComponent corruptionCom)
    {
        _corruptedSpawners++;
        if (_corruptedSpawners == _listOfSpawners.Spawners.Count) CompleteMission();
    }
    private void OnSpawnerUnCorrupted(CorruptionComponent corruptionCom)
    {
        _corruptedSpawners--;
        if(_corruptedSpawners <= 0) failMission();
    }
}
