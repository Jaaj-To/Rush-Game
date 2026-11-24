using System;
using Tiles;
using UnityEngine;

[Serializable]
public class AltStock
{
    [SerializeField] private AltTile prefab;
    [SerializeField] private Vector3Int globalDir;
    [Min(0)] [SerializeField] private int count;
    
    public AltTile Prefab => prefab;
    public int Count => count;
    public Vector3 GlobalDir => globalDir;
}
