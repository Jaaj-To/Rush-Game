using UnityEngine;

namespace Tiles
{
    public class Conveyor : AltTile
    {
        public override void ApplyAction(CubeController pCube)
        {
            Vector3Int lDirection = Vector3Int.RoundToInt(transform.forward);
            pCube.Convey(lDirection, Level.IsObstacle(pCube.GridPosition + lDirection));
        }
    }
}
