using System;
using Tiles;
using UnityEngine;

[CreateAssetMenu(fileName = "AltTilesStock", menuName = "Scriptable Objects/AltTilesStock")]
[Serializable]
public class AltTilesStock : ScriptableObject
{
    [SerializeField] private AltTile prefab;
    [Min(0)] [SerializeField] private int count;

    public AltTile Prefab => prefab;
    public int Count => count;
}
