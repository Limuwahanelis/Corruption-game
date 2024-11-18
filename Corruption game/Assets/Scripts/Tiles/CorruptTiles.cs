using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CorruptTiles : MonoBehaviour
{

    [SerializeField] MyColor _corruptionColor;
    [SerializeField] Tilemap map;
    [SerializeField] float _corruptionDelay;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) CorruptTileRadius(Vector2.zero, 3);
        if (Input.GetKeyDown(KeyCode.U)) UncorruptTiles(Vector2.zero, 3);
    }
    public void CorruptTileRadius(Vector2 _pos,float radius)
    {
        map.WorldToCell(_pos);
        if (map.GetTile(map.WorldToCell(_pos)))
        {
            StartCoroutine(CorruptTilesLeft(map.WorldToCell(_pos),radius,true));
        }
        if (map.GetTile(map.WorldToCell(_pos)))
        {
            StartCoroutine(CorruptTilesRight(map.WorldToCell(_pos), radius, true));
        }
    }
    public void UncorruptTiles(Vector2 _pos, float radius)
    {
        map.WorldToCell(_pos);
        if (map.GetTile(map.WorldToCell(_pos)))
        {
            StartCoroutine(CorruptTilesLeft(map.WorldToCell(_pos), radius, false));
        }
        if (map.GetTile(map.WorldToCell(_pos)))
        {
            StartCoroutine(CorruptTilesRight(map.WorldToCell(_pos), radius, false));
        }
    }
    IEnumerator CorruptTilesRight(Vector3Int firstTileToDestroy, float radius, bool corrupt)
    {
        Vector3Int curTile = firstTileToDestroy;// firstTileToDestroy; //map.WorldToCell(new Vector3(firstTileToDestroy.x + 1.01f, firstTileToDestroy.y, 0));
        if (corrupt) SetTileColor(curTile, _corruptionColor.Color);
        else SetTileColor(curTile, Color.white);
        Vector3Int tileHigher = map.WorldToCell(new Vector3(curTile.x, curTile.y + 1.01f, 0));
        Vector3Int tileLower = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
        TileBase lower = map.GetTile(tileLower);
        TileBase upper = map.GetTile(tileHigher);
        yield return new WaitForSeconds(_corruptionDelay);
        curTile = map.WorldToCell(new Vector3(curTile.x + 1.01f, firstTileToDestroy.y, 0));
        if (CalculateTileDistance(curTile, firstTileToDestroy) <= radius) if (curTile != null) StartCoroutine(CorruptTilesRight(curTile, radius - 1,corrupt));

        while (lower || upper)
        {

            if (upper && CalculateTileDistance(tileHigher, firstTileToDestroy) <= radius)
            {
                if (corrupt) SetTileColor(tileHigher, _corruptionColor.Color);
                else SetTileColor(tileHigher, Color.white);
            }
            else break;
            if (lower && CalculateTileDistance(tileLower, firstTileToDestroy) <= radius)
            {
                if (corrupt) SetTileColor(tileLower, _corruptionColor.Color);
                else SetTileColor(tileLower, Color.white);
            }
            else break;
            tileHigher = map.WorldToCell(new Vector3(tileHigher.x, tileHigher.y + 1.01f, 0));
            tileLower = map.WorldToCell(new Vector3(tileLower.x, tileLower.y - 0.5f, 0));
            lower = map.GetTile(tileLower);
            upper = map.GetTile(tileHigher);
            yield return new WaitForSeconds(_corruptionDelay);
        }
    }
    IEnumerator CorruptTilesLeft(Vector3Int firstTileToDestroy, float radius, bool corrupt)
    {
        Vector3Int curTile = firstTileToDestroy; //map.WorldToCell(new Vector3(firstTileToDestroy.x + 1.01f, firstTileToDestroy.y, 0));
        if (corrupt) SetTileColor(curTile, _corruptionColor.Color);
        else SetTileColor(curTile, Color.white);
        Vector3Int tileHigher = map.WorldToCell(new Vector3(curTile.x, curTile.y + 1.01f, 0));
        Vector3Int tileLower = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
        TileBase lower = map.GetTile(tileLower);
        TileBase upper = map.GetTile(tileHigher);
        yield return new WaitForSeconds(_corruptionDelay);
        curTile = map.WorldToCell(new Vector3(curTile.x - 0.5f, firstTileToDestroy.y, 0));
        if (CalculateTileDistance(curTile, firstTileToDestroy) <= radius) if (curTile != null) StartCoroutine(CorruptTilesLeft(curTile, radius - 1, corrupt));
        while (lower || upper)
        {

            if (upper && CalculateTileDistance(tileHigher, firstTileToDestroy) <= radius)
            {
                if(corrupt) SetTileColor(tileHigher, _corruptionColor.Color);
                else SetTileColor(tileHigher,Color.white);
            }
            else break;
            if (lower && CalculateTileDistance(tileLower, firstTileToDestroy) <= radius)
            {
                if (corrupt) SetTileColor(tileLower,_corruptionColor.Color);
                else SetTileColor(tileLower,Color.white);
            }
            else break;
            tileHigher = map.WorldToCell(new Vector3(tileHigher.x, tileHigher.y + 1.01f, 0));
            tileLower = map.WorldToCell(new Vector3(tileLower.x, tileLower.y - 0.5f, 0));
            lower = map.GetTile(tileLower);
            upper = map.GetTile(tileHigher);
            yield return new WaitForSeconds(_corruptionDelay);
        }
    }
    private int CalculateTileDistance(Vector3Int tile1,Vector3Int tile2)
    {
        Vector3Int sub=tile1-tile2;
        return math.abs(sub.x) + math.abs(sub.y);
    }
    public void SetTileColor(Vector3Int pos,Color color)
    {
        map.SetColor(pos, color);
    }
}
