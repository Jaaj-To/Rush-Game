using System.Collections.Generic;
using Tiles;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<AltTileStock> altTileInventory = new List<AltTileStock>();

    private Dictionary<Vector3Int, Tile> tiles = new Dictionary<Vector3Int, Tile>();
    private Dictionary<Vector3Int, AltTile> placedAltTiles = new Dictionary<Vector3Int, AltTile>();
    private Dictionary<AltTile, int> remainingAltTiles = new Dictionary<AltTile, int>();
    private List<CubeSpawner> spawners = new List<CubeSpawner>();
    private Vector3 gridOrigin;
    private int lowestHeight = 0;

    public Vector3 GridOrigin => gridOrigin;
    public List<AltTileStock> AltTileInventory => altTileInventory;
    public List<CubeSpawner> Spawners => spawners;
    public int LowestHeight => lowestHeight;

    private void Awake()
    {
        InitLevel();
    }

    private void InitLevel()
    {
        tiles.Clear();
        placedAltTiles.Clear();
        remainingAltTiles.Clear();
        spawners.Clear();
        lowestHeight = 0;

        Tile[] lTiles = GetComponentsInChildren<Tile>();
        gridOrigin = transform.position;
        if (lTiles.Length > 0)
        {
            gridOrigin = lTiles[0].transform.position;
        }

        foreach (Tile lTile in lTiles)
        {
            if (!(lTile is AltTile))
            {
                RegisterTile(lTile);
            }
        }

        foreach (Tile lTile in lTiles)
        {
            if (lTile is AltTile lAltTile)
            {
                RegisterAltTile(lAltTile);
            }
        }

        spawners.AddRange(GetComponentsInChildren<CubeSpawner>());

        foreach (AltTileStock lStock in altTileInventory)
        {
            if (lStock.Prefab == null)
            {
                Debug.LogWarning(name + " : une ligne de l'inventaire n'a pas de prefab");
                continue;
            }

            remainingAltTiles.TryGetValue(lStock.Prefab, out int lCount);
            remainingAltTiles[lStock.Prefab] = lCount + lStock.Count;
        }

        Debug.Log(name + " construit avec " + tiles.Count + " tiles et " + spawners.Count + " spawners");
    }

    private void RegisterTile(Tile pTile)
    {
        Vector3Int lGridPos = WorldToGrid(pTile.transform.position);
        if (tiles.ContainsKey(lGridPos))
        {
            Debug.LogWarning("Deux tiles sur la case " + lGridPos + " : " + tiles[lGridPos].name + " et " + pTile.name);
            return;
        }

        pTile.Init(lGridPos, this);
        AddTile(lGridPos, pTile);
    }

    private void RegisterAltTile(AltTile pAltTile)
    {
        Vector3Int lGridPos = WorldToGrid(pAltTile.transform.position);
        if (!tiles.ContainsKey(lGridPos))
        {
            pAltTile.Init(lGridPos, this);
            AddTile(lGridPos, pAltTile);
            return;
        }

        Tile lFloorTile = tiles[lGridPos];
        if (lFloorTile is AltTile)
        {
            return;
        }

        pAltTile.InitPlaced(lFloorTile, null);
        tiles[lGridPos] = pAltTile;
    }

    private void AddTile(Vector3Int pGridPos, Tile pTile)
    {
        tiles.Add(pGridPos, pTile);
        if (pGridPos.y < lowestHeight)
        {
            lowestHeight = pGridPos.y;
        }
    }

    public Vector3Int WorldToGrid(Vector3 pWorldPos)
    {
        return Vector3Int.RoundToInt(pWorldPos - gridOrigin);
    }

    public Vector3 GridToWorld(Vector3Int pGridPos)
    {
        return gridOrigin + (Vector3)pGridPos;
    }

    public Tile GetTile(Vector3Int pGridPos)
    {
        if (tiles.ContainsKey(pGridPos))
        {
            return tiles[pGridPos];
        }
        return null;
    }

    public bool IsObstacle(Vector3Int pGridPos)
    {
        return tiles.ContainsKey(pGridPos);
    }

    public int GetRemainingAltTiles(AltTile pPrefab)
    {
        if (pPrefab != null && remainingAltTiles.ContainsKey(pPrefab))
        {
            return remainingAltTiles[pPrefab];
        }
        return 0;
    }

    public bool TryPlaceAltTile(AltTile pPrefab, Vector3Int pGridPos, Vector3Int pDirection)
    {
        if (GetRemainingAltTiles(pPrefab) <= 0)
        {
            return false;
        }

        Tile lTile = GetTile(pGridPos);
        if (lTile == null || !lTile.CanHoldAltTile || IsObstacle(pGridPos + Vector3Int.up))
        {
            return false;
        }

        AltTile lAltTile = Instantiate(pPrefab, GridToWorld(pGridPos), Quaternion.LookRotation(pDirection), transform);
        lAltTile.InitPlaced(lTile, pPrefab);

        tiles[pGridPos] = lAltTile;
        placedAltTiles.Add(pGridPos, lAltTile);
        remainingAltTiles[pPrefab]--;
        return true;
    }

    public bool TryRemoveAltTile(Vector3Int pGridPos)
    {
        if (!placedAltTiles.ContainsKey(pGridPos))
        {
            return false;
        }

        AltTile lAltTile = placedAltTiles[pGridPos];
        tiles[pGridPos] = lAltTile.CoveredTile;
        placedAltTiles.Remove(pGridPos);
        remainingAltTiles[lAltTile.SourcePrefab]++;
        Destroy(lAltTile.gameObject);
        return true;
    }
}
