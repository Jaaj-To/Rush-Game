using UnityEngine;

namespace Tiles
{
    public class Splitter : AltTile
    {
        [SerializeField] private bool startWithLeft = true;

        private bool isNextLeft;

        private void Awake()
        {
            isNextLeft = startWithLeft;
        }

        public override void ApplyAction(CubeController pCube)
        {
            if (isNextLeft)
            {
                pCube.TurnLeft();
            }
            else
            {
                pCube.TurnRight();
            }

            isNextLeft = !isNextLeft;
        }
    }
}
