using UnityEngine;

namespace Tiles
{
    public class Stopper : AltTile
    {
        [Range(1, 2)] [SerializeField] private int stopTicks = 2;

        public override void ApplyAction(CubeController pCube)
        {
            pCube.Wait(stopTicks);
        }
    }
}
