using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName ="ListOfObstacles1")]
public class ListOfObstacles1 : ScriptableObject
{
    [SerializeField] public List<MapObstacle> _obstacles;

}