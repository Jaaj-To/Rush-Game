using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class HUD : BaseMenu
    {
        [SerializeField] private AltTilesStock stock;
        [SerializeField] private HUDTile hudTilePrefab;
        [SerializeField] private List<RectTransform> points = new List<RectTransform>();

        private List<int> counts = new List<int>();
        private List<HUDTile> hudTiles = new List<HUDTile>();

        private void Start()
        {
            if (stock != null && counts.Count == 0) SetStock(stock);
        }

        public void SetStock(AltTilesStock pStock)
        {
            stock = pStock;
            counts.Clear();

            for (int i = 0; i < stock.AltStockList.Count; i++)
            {
                HUDTile lHudTile = Instantiate(hudTilePrefab, points[i], false);
                hudTiles.Add(lHudTile);
                counts.Add(stock.AltStockList[i].Count);
            }
            
            Refresh();
        }

        public void UseTile(AltStock pAltStock)
        {
            if (stock == null) return;

            int lIndex = stock.AltStockList.IndexOf(pAltStock);
            if (lIndex < 0 || lIndex >= counts.Count || counts[lIndex] <= 0) return;

            counts[lIndex]--;
            Refresh();
        }
        
        private void Refresh()
        {
            for (int i = 0; i < hudTiles.Count; i++)
            {
                hudTiles[i].SetTile(stock.AltStockList[i], counts[i]);
                hudTiles[i].gameObject.SetActive(counts[i] > 0);
            }
        }
        
        private void Clear()
        {
            foreach (HUDTile lHudTile in hudTiles)
            {
                Destroy(lHudTile.gameObject);
            }

            hudTiles.Clear();
            counts.Clear();
        }
    }
}
