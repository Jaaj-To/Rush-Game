using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AltTilesStock", menuName = "Scriptable Objects/AltTilesStock")]
[Serializable]
public class AltTilesStock : ScriptableObject
{
    [SerializeField] private List<AltStock> altStockList = new List<AltStock>();
    public List<AltStock> AltStockList => altStockList;
}
