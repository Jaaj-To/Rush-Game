using System;
using Tiles;
using UnityEngine;

[Serializable]
public class AltStock
{
    [SerializeField] private AltTile prefab;
    [SerializeField] private Vector3Int globalDir;
    [SerializeField] private Sprite icon;
    [Min(0)] [SerializeField] private int count;
    
    public AltTile Prefab => prefab;
    public Sprite Icon => icon;
    public int Count => count;
    public Vector3Int GlobalDir => globalDir;
}
