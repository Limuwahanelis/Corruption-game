using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ListOfPools")]
public class ListOfPools : ScriptableObject
{
    public List<UnitPool> Pools=>_pools;
    private List<UnitPool> _pools = new List<UnitPool>();

    public void AddPoolToList(UnitPool pool)
    {
        _pools.Add(pool);
    }
    public void RemovePool(UnitPool pool)
    {
        _pools.Remove(pool);
    }
}