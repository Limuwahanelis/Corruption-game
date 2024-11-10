using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologyPointSpawner : MonoBehaviour
{
    [SerializeField] TechnologyPointsPool _pool;
    public void Spawn(int technologyValue,Vector3 position)
    {
        TechnologyPoint point= _pool.GetPoint();
        position.z = -0.5f;
        point.transform.position = position;
        point.SetUp(technologyValue);

    }
}
