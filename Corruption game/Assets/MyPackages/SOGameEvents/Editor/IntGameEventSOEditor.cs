using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IntGameEventSO))]
public class IntGameEventSOEditor : Editor
{
    int testValue;
    Vector3 testVector;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
         testValue = EditorGUILayout.IntField("Test value", testValue);
         testVector = EditorGUILayout.Vector3Field("Test vector", testVector);
        if (GUILayout.Button("Raise event"))
        {
            (target as IntGameEventSO).Raise(testValue, testVector);
        }
        
    }
}
