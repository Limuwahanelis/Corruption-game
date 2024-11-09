using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetDetector : MonoBehaviour
{
    [SerializeField] FactionAllegiance _factionAllegiance;

    [Serializable]
    public class Target
    {
        public Transform tran;
        public CorruptionComponent corruptionComponent;
        public IDamagable damagable;
        public FactionAllegiance factionAllegiance;
        private DamageInfo dmgInfo = new DamageInfo();
        public void DealDamage(int damage,int corruptionForce,Vector3 position)
        {
            dmgInfo.dmg = damage;
            dmgInfo.dmgPosition = position;
            if (damagable != null) damagable.TakeDamage(dmgInfo);
            if (corruptionComponent != null) corruptionComponent.IncreseCorruption(corruptionForce);
        }
    }
    List<Target> targets= new List<Target>();
    List<Target> _allPossibletargets = new List<Target>();
    public UnityEvent<Target> OnTargetDetected;
    public UnityEvent OnTargetLeft;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable tmp = collision.attachedRigidbody.GetComponent<IDamagable>();
        CorruptionComponent tmp2 = collision.attachedRigidbody.GetComponent<CorruptionComponent>();
        FactionAllegiance factionAllegiance = collision.attachedRigidbody.GetComponent<FactionAllegiance>();

        Target newtarget = new Target()
        {
            tran = collision.transform,
            corruptionComponent = tmp2,
            damagable = tmp,
        };
        if(factionAllegiance != null)
        {
            newtarget.factionAllegiance = factionAllegiance;
        }
        _allPossibletargets.Add(newtarget);
        if (factionAllegiance != null)
        {
            if (factionAllegiance.Allegiance == _factionAllegiance.Allegiance) return;
        }
        if (tmp != null)
        {
            if (targets.Find(x => x.damagable == tmp) != null) return;
            targets.Add(newtarget);
            tmp.OnDeath += RemoveTarget;
            OnTargetDetected?.Invoke(newtarget);
        }
        if (tmp2 != null)
        {
            if (!tmp2.IsCorrupted)
            {
                if (targets.Find(x => x.corruptionComponent == tmp2) != null) return;
                targets.Add(newtarget);
                OnTargetDetected?.Invoke(newtarget);
            }
        }
    }
    public Target GetClosestTarget(Transform tran)
    {
        if (targets.Count == 0) return null;
        Target closestTarget = targets[0];
        float lowestDistance=Vector2.Distance(tran.position,closestTarget.tran.position);
        float dist = 0;
        for(int i=0; i<targets.Count; i++) 
        {
            dist = Vector2.Distance(tran.position, targets[i].tran.position);
            if (dist < lowestDistance)
            {
                lowestDistance = dist;
                closestTarget = targets[i];
            }
        }
        return closestTarget;
    }
    public void UpdateTargetList()
    {
        targets.Clear();
        for(int i=0;i<_allPossibletargets.Count;i++)
        {
            if (_allPossibletargets[i].factionAllegiance==null || _allPossibletargets[i].factionAllegiance.Allegiance!=_factionAllegiance.Allegiance)
            {
                targets.Add(_allPossibletargets[i]);
            }
        }
    }
    private void RemoveTarget(IDamagable target)
    {
        Target tmp = targets.Find(x=>x.damagable == target);
        if (tmp != null)
        {
            targets.Remove(tmp);
            tmp.damagable.OnDeath -= RemoveTarget;
        }
    }
    private void OnDestroy()
    {
        for(int i=0;i<targets.Count;i++) 
        {
            IDamagable tmp = targets[i].damagable;
            if(tmp != null) tmp.OnDeath -= RemoveTarget;
        }
    }
}
