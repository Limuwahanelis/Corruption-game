using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spawner : MonoBehaviour,IMouseInteractable,IPointerEnterHandler,IPointerExitHandler
{
    public Transform LinePoint => _linePoint;

    [Header("Spawner settings")]
    [SerializeField] bool _spawnOnStart;
    [SerializeField] bool _spawn;
    [SerializeField] float _spawnRate;
    [SerializeField] bool _playerInteractable;
    [SerializeField] int _firstTimeCorruptionTechnologyValue;
    [SerializeField] ListOfPools _poolsList;
    [SerializeField] ListOfSpawners _spawners;
    [Header("Spawning")]
    [SerializeField] List<Transform> _spawnTran;
    [SerializeField] List<UnitData> _unitsToSpawn;
    [SerializeField] List<int> _spawnNumber;
    [SerializeField] List<int> _corruptedUnitsSpawnNumber;
    [Header("Components")]
    
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _linePoint;
    [SerializeField] Transform _unitsOriginaltarget;
    [SerializeField] FactionAllegiance _factionAllegiance;
    [SerializeField] CorruptionComponent _corruptionComponent;
    [SerializeField] SpriteColor _spriteColor;
    [SerializeField] MyColor _corruptionColor;
    [Header("Mouse interactions")]
    [SerializeField] GameObject _hoverBorder;
    [SerializeField] GameObject _selectedBorder;
    private bool _isSelected = false;
    private float _time = 0;
    private float _index = 0;
   
    private void Awake()
    {
        _spawners.AddSpawnerToList(this);
        _corruptionComponent.SetTechnologyPointsvalue(_firstTimeCorruptionTechnologyValue);
    }
    private void Start()
    {
        ChangeOriginaltarget();
        if (_spawnOnStart)
        {
            Spawn();
        }
    }
    public void Spawn()
    {
        int _spawnIndex = 0;
        
        for(int i=0;i< (_unitsToSpawn.Count);i++)
        {
            int _poolIndex = _poolsList.Pools.FindIndex(x => x.SpawnUnitData == _unitsToSpawn[i]);
            for (int j = 0; j < (_corruptionComponent.IsCorrupted ? _corruptedUnitsSpawnNumber[i] : _spawnNumber[i]);j++) 
            {
                Unit unit = _poolsList.Pools[_poolIndex].GetUnit();
                unit.gameObject.name = $"{_index} {_corruptionComponent.IsCorrupted}";
                unit.SetUp();
                unit.transform.position = _spawnTran[_spawnIndex].position;
                unit.SetOriginaltarget(_unitsOriginaltarget, _unitsOriginaltarget.GetComponent<IDamagable>(), _unitsOriginaltarget.GetComponent<CorruptionComponent>());
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

    private void Update()
    {
        if(PauseSettings.IsGamePaused) return;
        if (!_spawn) return;
        _time += Time.deltaTime;
        if(_time > _spawnRate) 
        {
            Spawn();
            _time= 0;
        }
    }
    public void Interact()
    {
        _isSelected = true;
        _selectedBorder.SetActive(true);
        _hoverBorder.SetActive(false);
        _lineRenderer.SetPosition(0, _linePoint.position);
        _lineRenderer.SetPosition(1, _unitsOriginaltarget.GetComponent<Spawner>().LinePoint.position);

    }

    public void Deselect()
    {
        _isSelected= false;
        _selectedBorder.SetActive(false);
        _lineRenderer.SetPosition(0, Vector3.zero);
        _lineRenderer.SetPosition(1, Vector3.zero);
    }

    public void RBMPress(Transform tran,bool isCorrupted)
    {
        if (isCorrupted) return;
        if (!_corruptionComponent.IsCorrupted) return;
        if (_isSelected)
        {
            _unitsOriginaltarget = tran;
            _lineRenderer.SetPosition(0, _linePoint.position);
            _lineRenderer.SetPosition(1, _unitsOriginaltarget.GetComponent<Spawner>().LinePoint.position);
        }

    }
    private void OnOriginalTargetCorrupted(CorruptionComponent corruptionComponent)
    {
        corruptionComponent.OnCorrupted.RemoveListener(OnOriginalTargetCorrupted);
        ChangeOriginaltarget();
    }
    private void ChangeOriginaltarget()
    {
        List<Spawner> spawners = _spawners.Spawners.FindAll(x => x.GetComponent<FactionAllegiance>().Allegiance != _factionAllegiance.Allegiance);
        if (spawners != null && spawners.Count > 0)
        {
            Spawner closestSpawner = spawners[0];
            float shortestDist = Vector3.Distance(transform.position, closestSpawner.transform.position);
            for (int i = 1; i < spawners.Count; i++)
            {
                float dist = Vector3.Distance(spawners[i].transform.position, transform.position);
                if (dist < shortestDist)
                {
                    closestSpawner = spawners[i];
                    shortestDist = dist;
                }
            }
            _unitsOriginaltarget = closestSpawner.transform;
            closestSpawner.GetComponent<CorruptionComponent>().OnCorrupted.AddListener(OnOriginalTargetCorrupted);
            if (_isSelected) Interact();
        }
        else _spawn = false;
    }
    public void CorruptSpawner(CorruptionComponent corruptionComponent)
    {
        ChangeOriginaltarget();
        _spriteColor.ChangeColor(_corruptionColor.Color);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelected) return;
        _hoverBorder.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelected) return;
        _hoverBorder.SetActive(true);
    }
    private void OnDestroy()
    {
        _spawners.RemoveSpawner(this);
    }
}
