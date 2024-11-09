using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour,IMouseInteractable
{
    [SerializeField] bool _spawnOnStart;
    [SerializeField] float _spawnRate;
    [SerializeField] bool _playerInteractable;
    [SerializeField] ListOfPools _poolsList;
    [SerializeField] List<Transform> _spawnTran;
    [SerializeField] List<UnitData> _unitsToSpawn;
    [SerializeField] List<int> _spawnNumber;
    [SerializeField] Transform _unitsOriginaltarget;
    [SerializeField] CorruptionComponent _unitsOriginalTargetCoruption;
    [SerializeField] CorruptionComponent _corruptionComponent;
    [SerializeField] GameObject _hoverBorder;
    [SerializeField] GameObject _selectedBorder;
    private bool _isSelected = false;
    private float _time = 0;
    private float _index = 0;
    public void Spawn()
    {
        int _spawnIndex = 0;
        
        for(int i=0;i< _unitsToSpawn.Count;i++)
        {
            int _poolIndex = _poolsList.Pools.FindIndex(x => x.SpawnUnitData == _unitsToSpawn[i]);
            for (int j=0;j<_spawnNumber[i];j++) 
            {
                Unit unit = _poolsList.Pools[_poolIndex].GetUnit();
                unit.gameObject.name = $"{_index} {_corruptionComponent.IsCorrupted}";
                unit.transform.position = _spawnTran[_spawnIndex].position;
                unit.SetOriginaltarget(_unitsOriginaltarget);
                if (_corruptionComponent.IsCorrupted) unit.Corrupt();
                _spawnIndex++;
                if (_spawnIndex >= _spawnTran.Count)
                {
                    _spawnIndex = 0;
                }
                _index++;
            }

        }
    }
    private void Start()
    {
        if(_spawnOnStart)
        {
            Spawn();
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
    private void OnMouseEnter()
    {
        if (_isSelected) return;
        _hoverBorder.SetActive(true);
    }
    private void OnMouseExit()
    {
        if (_isSelected) return;
        _hoverBorder.SetActive(false);
    }
    public void Interact()
    {
        _isSelected = true;
        _selectedBorder.SetActive(true);
        _hoverBorder.SetActive(false);
    }

    public void Deselect()
    {
        _isSelected= false;
        _selectedBorder.SetActive(false);
    }

    public void RBMPress(Transform tran,bool isCorrupted)
    {
        if (isCorrupted) return;
        if (!_corruptionComponent.IsCorrupted) return;
        if(_isSelected) _unitsOriginaltarget = tran;

    }
}
