using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float _spawnRate;
    [SerializeField] ListOfPools _poolsList;
    [SerializeField] List<Transform> _spawnTran;
    [SerializeField] List<UnitData> _unitsToSpawn;
    [SerializeField] List<int> _spawnNumber;
    [SerializeField] Transform _unitsOriginaltarget;
    private float _time = 0;
    public void Spawn()
    {
        int _spawnIndex = 0;
        
        for(int i=0;i< _unitsToSpawn.Count;i++)
        {
            int _poolIndex = _poolsList.Pools.FindIndex(x => x.SpawnUnitData == _unitsToSpawn[i]);
            for (int j=0;j<_spawnNumber[i];j++) 
            {
                Unit unit = _poolsList.Pools[_poolIndex].GetUnit();
                unit.transform.position = _spawnTran[_spawnIndex].position;
                unit.SetOriginaltarget(_unitsOriginaltarget);
                _spawnIndex++;
                if (_spawnIndex >= _spawnTran.Count)
                {
                    _spawnIndex = 0;
                }
            }

        }
    }
    private void Update()
    {
        _time += Time.deltaTime;
        if(_time > _spawnRate) 
        {
            Spawn();
            _time= 0;
        }
    }
}
