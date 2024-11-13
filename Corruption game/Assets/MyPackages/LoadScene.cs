using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] int _sceneToLoad;
    public void Load()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }
    public void SetSceneIndex(int index)
    {
        _sceneToLoad = index;
    }
    //private void Start()
    //{
    //    LoadNextScene();
    //}
    public void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
