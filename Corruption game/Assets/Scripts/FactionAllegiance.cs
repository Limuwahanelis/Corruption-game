using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionAllegiance : MonoBehaviour
{
    public Allegiance Allegiance => _allegiance;
    [SerializeField] Allegiance _allegiance;

    public void SetAllegiance(Allegiance allegiance)
    {
        _allegiance = allegiance;
    }

}
