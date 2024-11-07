using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CorruptionComponent : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Logger.Log("fasf", this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Logger.Log("fsfsfsf", this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
