using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnitPool : MonoBehaviour
{
    public UnitData SpawnUnitData => _unitData;
    [SerializeField] ListOfPools _poolsList;
    [SerializeField] UnitData _unitData;
    private ObjectPool<Unit> _unitPool;

    // Start is called before the first frame update
    void Awake()
    {
        _unitPool = new ObjectPool<Unit>(CrateUnit, OnTakeUnitFromPool, OnReturnUnitToPool);
        _poolsList.AddPoolToList(this);
    }
    private void Start()
    {
       
    }
    public Unit GetUnit()
    {
        return _unitPool.Get();
    }
    Unit CrateUnit()
    {
        Unit unit = Instantiate(_unitData.UnitPrefab);
        unit.SetPool(_unitPool);
        return unit;

    }
    public void OnTakeUnitFromPool(Unit unit)
    {

        unit.gameObject.SetActive(true);
    }
    public void OnReturnUnitToPool(Unit powerUp)
    {
        powerUp.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        _poolsList.RemovePool(this);
    }
}
