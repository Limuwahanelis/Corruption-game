using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SpawnerData")]
public class SpawnerData : BuildingData
{
    [SerializeField] float _spawnRate;
    [SerializeField] ListOfPools _poolsList;
    [SerializeField] ListOfSpawners _spawners;

    public float SpawnRate { get => _spawnRate; }
    public ListOfPools PoolsList { get => _poolsList; }
    public ListOfSpawners Spawners { get => _spawners; }
}