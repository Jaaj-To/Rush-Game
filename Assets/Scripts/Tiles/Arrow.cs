using UnityEngine;

namespace Tiles
{
    public class Arrow : AltTile
    {
        public override void ApplyAction(CubeController pCube)
        {
            pCube.SetDirection(Vector3Int.RoundToInt(transform.forward));
        }
    }
}
