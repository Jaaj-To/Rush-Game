namespace Tiles
{
    public abstract class AltTile : Tile
    {
        private Tile coveredTile;
        private AltTile sourcePrefab;

        public Tile CoveredTile => coveredTile;
        public AltTile SourcePrefab => sourcePrefab;
        public override bool CanHoldAltTile => false;

        public void InitPlaced(Tile pCoveredTile, AltTile pSourcePrefab)
        {
            Init(pCoveredTile.GridPos, pCoveredTile.Level);
            coveredTile = pCoveredTile;
            sourcePrefab = pSourcePrefab;
        }

        public override abstract void ApplyAction(CubeController pCube);
    }
}
