using System;
using UnityEngine;

namespace Tiles
{
    [Serializable]
    public class AltTileStock
    {
        [SerializeField] private AltTile prefab;
        [Min(0)] [SerializeField] private int count;

        public AltTile Prefab => prefab;
        public int Count => count;
    }
}
