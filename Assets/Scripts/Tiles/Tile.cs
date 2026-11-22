using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        private Vector3Int gridPos;
        private Level level;

        public Vector3Int GridPos => gridPos;
        public Level Level => level;
        public virtual bool CanHoldAltTile => true;

        public void Init(Vector3Int pGridPos, Level pLevel)
        {
            gridPos = pGridPos;
            level = pLevel;
        }

        public virtual void ApplyAction(CubeController pCube)
        {
            pCube.StopAfterConvey();
        }
    }
}
