using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MapObstacle:MonoBehaviour
{
    public Transform ObstacleTran => _obstacle;
    public float Radius => _obstaclerange;
    [SerializeField] Transform _obstacle;
    [SerializeField] float _obstaclerange;
    [SerializeField] ListOfGameobjects _listOfMapObstacles;
    private void Reset()
    {
        _obstacle = transform;
    }
    private void Awake()
    {
        if (_listOfMapObstacles == null) Logger.Error("List of map obstacles was not assigned !", this);
        _listOfMapObstacles.AddGameobject(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_obstacle.position, _obstaclerange);
    }
    private void OnDestroy()
    {
        _listOfMapObstacles.RemoveGameobject(gameObject);
    }
}

