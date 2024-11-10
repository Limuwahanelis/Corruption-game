using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TechnologyPointsPool : MonoBehaviour
{
    public TechnologyPoint TechnologyPointPrefab => _technologyPointPrefab;
    [SerializeField] TechnologyPoint _technologyPointPrefab;
    [SerializeField] TechnologyManager _technologyManager;
    [SerializeField] RectTransform _technologyPointIconTran;
    private ObjectPool<TechnologyPoint> _technologyPointsPool;

    // Start is called before the first frame update
    void Awake()
    {
        _technologyPointsPool = new ObjectPool<TechnologyPoint>(CratePoint, OnTakePointFromPool, OnReturnPointToPool);
    }
    private void Start()
    {

    }
    public TechnologyPoint GetPoint()
    {
        return _technologyPointsPool.Get();
    }
    TechnologyPoint CratePoint()
    {
        TechnologyPoint point = Instantiate(_technologyPointPrefab);
        point.SetPool(_technologyPointsPool);
        point.Initialize(_technologyManager, _technologyPointIconTran);
        return point;

    }
    public void OnTakePointFromPool(TechnologyPoint point)
    {

        point.gameObject.SetActive(true);
    }
    public void OnReturnPointToPool(TechnologyPoint point)
    {
        point.gameObject.SetActive(false);
        point.ResetPoint();
    }
}
