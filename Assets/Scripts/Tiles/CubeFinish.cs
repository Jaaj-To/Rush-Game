using UnityEngine;

namespace Tiles
{
    public class CubeFinish : AltTile
    {
        [SerializeField] private CubeColor color = CubeColor.Red;

        public CubeColor ColorType => color;

        private void Start()
        {
            CubeColors.Paint(gameObject, CubeColors.ToColor(color));
        }

        public override void ApplyAction(CubeController pCube)
        {
            if (pCube.ColorType == color)
            {
                pCube.Arrive();
                return;
            }

            pCube.StopAfterConvey();
        }
    }
}
