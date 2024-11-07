using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperClass : MonoBehaviour
{
    public static Vector3 MousePos => _mousePos;
    private static Vector2 _mousePos;
    public static void SetMousePos(Vector2 pos)
    {
        _mousePos = pos;
    }
    public static IEnumerator DelayedFunction(float timeToWait, Action function)
    {
        yield return new WaitForSeconds(timeToWait);
        function();
    }
    public static IEnumerator WaitFrame(Action function)
    {
        yield return null;
        function();
    }
    public static bool CheckIfObjectIsBehind(Vector2 gameObjectPos, Vector2 gameObjectToCheckPos, GlobalEnums.HorizontalDirections gameObjectLookingDirection)
    {
        // sub result - <0 means gameObjectToCheckPos is on right, else its on left. Mult result - <0 gameObjectToCheckPos is in front, else gameObjectToCheckPos is behind
        if ((gameObjectPos.x - gameObjectToCheckPos.x) * ((int)gameObjectLookingDirection) <= 0) return false;
        else return true;
    }
}
