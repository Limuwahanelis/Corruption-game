using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ListOfGameobjects")]
public class ListOfGameobjects : ScriptableObject
{
    public List<GameObject> GameObjects=>_gameObjects;
    [SerializeField]private List<GameObject> _gameObjects=new List<GameObject>();

    public void AddGameobject(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
    }
    public void RemoveGameobject(GameObject gameObject)
    {
        _gameObjects.Remove(gameObject);
    }

}