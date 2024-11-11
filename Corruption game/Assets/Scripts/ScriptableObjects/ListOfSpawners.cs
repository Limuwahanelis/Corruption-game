using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName ="ListOfSpawners")]
public class ListOfSpawners : ScriptableObject
{
    public List<Spawner> Spawners => _spawners;
    [SerializeField] List<Spawner> _spawners;

    public void AddSpawnerToList(Spawner spawner)
    {
        _spawners.Add(spawner);
    }
    public void RemoveSpawner(Spawner spawner)
    {
        _spawners.Remove(spawner);
    }
}