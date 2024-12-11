using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
[SelectionBase]
public class Spawner : MonoBehaviour,IMouseInteractable,IPointerEnterHandler,IPointerExitHandler
{
    public Transform LinePoint => _linePoint;

    [Header("Spawner settings")]
    [SerializeField] SpawnerData _data;
    [SerializeField] bool _spawnOnStart;
    [SerializeField] bool _spawn;
    [Header("Spawning")]
    [SerializeField] List<Transform> _spawnTran;
    [SerializeField] List<UnitData> _unitsToSpawn;
    [SerializeField] List<int> _spawnNumber;
    [SerializeField] List<int> _corruptedUnitsSpawnNumber;
    [Header("Components")]
    [SerializeField] HealthSystem _healthSystem;
    [SerializeField] Transform _linePoint;
    [SerializeField] FactionAllegiance _factionAllegiance;
    [SerializeField] CorruptionComponent _corruptionComponent;
    [SerializeField] SpriteColor _spriteColor;
    [SerializeField] MyColor _corruptionColor;
    [Header("Assigned from scene")]
    [SerializeField] AudioSourcePool _sourcePool;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] CorruptTiles _corruptTiles;
    [SerializeField] CorutineHolder _corutineHolder;
    [Header("Mouse interactions")]
    [SerializeField] GameObject _hoverBorder;
    [SerializeField] GameObject _destroyedHoverBorder;
    [SerializeField] GameObject _selectedBorder;
    private Transform _unitsOriginaltarget;
    private bool _isSelected = false;
    private float _time = 0;
    private float _index = 0;
    private bool _isPointedAt=false;
    
    private void Awake()
    {
        
        _data.Spawners.AddSpawnerToList(this);
        _corruptionComponent.SetUp(_data.CorrutptionReduceInterval,_data.CorruptionDecrease,_corruptionComponent.IsCorrupted,_data.FirstTimeCorruptionTechnologyValue,_data.MaxCorruption);
        _healthSystem.OnDeath += DestroySpawner;
    }
    private void Start()
    {
        //if (_corruptionComponent.IsCorrupted) _corruptTiles.CorruptTileRadius(transform.position, _corruptionRadius);
        ChangeOriginaltarget();
        if (_spawnOnStart)
        {
            Spawn();
        }
    }
    private void Reset()
    {
        _sourcePool= FindObjectOfType<AudioSourcePool>();
        _lineRenderer = FindObjectOfType<LineRenderer>();
        _corruptTiles = FindObjectOfType<CorruptTiles>();
        _corutineHolder = FindObjectOfType<CorutineHolder>();
    }
    public void Spawn()
    {
        int _spawnIndex = 0;
        
        for(int i=0;i< (_unitsToSpawn.Count);i++)
        {
            int _poolIndex = _data.PoolsList.Pools.FindIndex(x => x.SpawnUnitData == _unitsToSpawn[i]);
            for (int j = 0; j < (_corruptionComponent.IsCorrupted ? _corruptedUnitsSpawnNumber[i] : _spawnNumber[i]);j++) 
            {
                Unit unit = _data.PoolsList.Pools[_poolIndex].GetUnit();
                unit.gameObject.name = $"{_index} {_corruptionComponent.IsCorrupted}";
                unit.SetUp(_sourcePool,_corruptionComponent.IsCorrupted,_corutineHolder);
                unit.transform.position = _spawnTran[_spawnIndex].position;
                unit.SetOriginaltarget(_unitsOriginaltarget, _unitsOriginaltarget.GetComponent<IDamagable>(), _unitsOriginaltarget.GetComponent<CorruptionComponent>());
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
        if(_time > _data.SpawnRate) 
        {
            Spawn();
            _time= 0;
        }
    }
    public void Interact()
    {
        if (!_healthSystem.IsAlive) return;
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

    // this function is called on currently selected spawner
    public void RBMPress(Transform tran,bool isCorrupted)
    {
        if (isCorrupted) return;
        if (!_corruptionComponent.IsCorrupted) return;
        if (_isSelected)
        {
            _unitsOriginaltarget = tran;
            _unitsOriginaltarget.GetComponent<CorruptionComponent>().OnCorrupted.AddListener(OnOriginalTargetCorrupted);
            _lineRenderer.SetPosition(0, _linePoint.position);
            _lineRenderer.SetPosition(1, _unitsOriginaltarget.GetComponent<Spawner>().LinePoint.position);
        }

    }
    private void OnOriginaltargetDestroyed(IDamagable damagable)
    {
        damagable.OnDeath -= OnOriginaltargetDestroyed;
        ChangeOriginaltarget();
    }
    private void OnOriginalTargetCorrupted(CorruptionComponent corruptionComponent)
    {
        corruptionComponent.OnCorrupted.RemoveListener(OnOriginalTargetCorrupted);
        ChangeOriginaltarget();
    }
    private void ChangeOriginaltarget()
    {
        List<Spawner> spawners = _data.Spawners.Spawners.FindAll(x => x.GetComponent<FactionAllegiance>().Allegiance != _factionAllegiance.Allegiance);
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
            closestSpawner.GetComponent<IDamagable>().OnDeath += OnOriginaltargetDestroyed;
            if (_isSelected) Interact();
        }
        else _spawn = false;
    }
    private void DestroySpawner(IDamagable damagable)
    {
        _spawn = false;
        _time = 0;
        _corruptionComponent.UnCorrupt();
        _spriteColor.ChangeColor(new Color(0.6886792f, 0.6886792f, 0.6886792f));
        if (_isPointedAt)
        {
            Deselect();
            _hoverBorder.SetActive(false);
            _destroyedHoverBorder.SetActive(true);
        }
    }
    // used by corruption component
    public void CorruptSpawner(CorruptionComponent corruptionComponent)
    {
        _time = 0;
        _spawn = true;
        _healthSystem.Heal(_healthSystem.MaxHP);
        _corruptTiles.CorruptTileRadius(transform.position,_data.CorruptionRadius);
        ChangeOriginaltarget();
        _spriteColor.ChangeColor(_corruptionColor.Color);
        if(_destroyedHoverBorder.activeSelf)
        {
            _destroyedHoverBorder.SetActive(false);
            _hoverBorder.SetActive(true);
        }

    }
    public void UnCorruptSpawner()
    {
        _corruptTiles.UncorruptTiles(transform.position, _data.CorruptionRadius);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelected) return;
        if (_healthSystem.IsAlive) _hoverBorder.SetActive(false);
        else _destroyedHoverBorder.SetActive(false);
        _isPointedAt = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelected) return;
        if (_healthSystem.IsAlive) _hoverBorder.SetActive(true);
        else _destroyedHoverBorder.SetActive(true);
        _isPointedAt = true;

    }
    private void OnDestroy()
    {
        _data.Spawners.RemoveSpawner(this);
    }
}
