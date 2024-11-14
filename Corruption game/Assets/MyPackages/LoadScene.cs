using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] int _sceneToLoad;
    [SerializeField] bool _loadOnStart;
    public void Load()
    {
        PauseSettings.SetPause(false);
        PauseSetter.ForceUnpause();
        SceneManager.LoadScene(_sceneToLoad);
    }
    public void SetSceneIndex(int index)
    {
        _sceneToLoad = index;
    }
    private void Start()
    {
        if(_loadOnStart) SceneManager.LoadScene(_sceneToLoad);
    }
    public void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
