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
    public void CorruptTileRadius(Vector2 _pos,float radius)
    {
        map.WorldToCell(_pos);
        Debug.Log("Dsadsada");
        if (map.GetTile(map.WorldToCell(_pos)))
        {
            StartCoroutine(DestroyTilesLeft(map.WorldToCell(_pos),radius));
        }
        if (map.GetTile(map.WorldToCell(_pos)))
        {
            StartCoroutine(DestroyTilesRight(map.WorldToCell(_pos), radius));
        }
    }
    IEnumerator DestroyTilesRight(Vector3Int firstTileToDestroy, float radius)
    {
        Vector3Int curTile = firstTileToDestroy;// firstTileToDestroy; //map.WorldToCell(new Vector3(firstTileToDestroy.x + 1.01f, firstTileToDestroy.y, 0));
        CorruptTile(curTile);
        Vector3Int tileHigher = map.WorldToCell(new Vector3(curTile.x, curTile.y + 1.01f, 0));
        Vector3Int tileLower = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
        TileBase lower = map.GetTile(tileLower);
        TileBase upper = map.GetTile(tileHigher);
        yield return new WaitForSeconds(_corruptionDelay);
        curTile = map.WorldToCell(new Vector3(curTile.x + 1.01f, firstTileToDestroy.y, 0));
        if (CalculateTileDistance(curTile, firstTileToDestroy) <= radius) if (curTile != null) StartCoroutine(DestroyTilesRight(curTile, radius - 1));

        while (lower || upper)
        {

            if (upper && CalculateTileDistance(tileHigher, firstTileToDestroy) <= radius) CorruptTile(tileHigher);
            else break;
            if (lower && CalculateTileDistance(tileLower, firstTileToDestroy) <= radius) CorruptTile(tileLower);
            else break;
            tileHigher = map.WorldToCell(new Vector3(tileHigher.x, tileHigher.y + 1.01f, 0));
            tileLower = map.WorldToCell(new Vector3(tileLower.x, tileLower.y - 0.5f, 0));
            lower = map.GetTile(tileLower);
            upper = map.GetTile(tileHigher);
            yield return new WaitForSeconds(_corruptionDelay);
        }
    }
    IEnumerator DestroyTilesLeft(Vector3Int firstTileToDestroy, float radius)
    {
        Vector3Int curTile = firstTileToDestroy; //map.WorldToCell(new Vector3(firstTileToDestroy.x + 1.01f, firstTileToDestroy.y, 0));
            CorruptTile(curTile);
            Vector3Int tileHigher = map.WorldToCell(new Vector3(curTile.x, curTile.y + 1.01f, 0));
            Vector3Int tileLower = map.WorldToCell(new Vector3(curTile.x, curTile.y - 0.5f, 0));
            TileBase lower = map.GetTile(tileLower);
            TileBase upper = map.GetTile(tileHigher);
        yield return new WaitForSeconds(_corruptionDelay);
        curTile = map.WorldToCell(new Vector3(curTile.x - 0.5f, firstTileToDestroy.y, 0));
        if (CalculateTileDistance(curTile, firstTileToDestroy) <= radius) if (curTile != null) StartCoroutine(DestroyTilesLeft(curTile, radius - 1));
        while (lower || upper)
            {
               
                if (upper && CalculateTileDistance(tileHigher,firstTileToDestroy)<= radius) CorruptTile(tileHigher);
                else break;
                if (lower && CalculateTileDistance(tileLower, firstTileToDestroy) <= radius) CorruptTile(tileLower);
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
    public void CorruptTile(Vector3Int pos)
    {
        map.SetColor(pos, _corruptionColor.Color);
    }
}
