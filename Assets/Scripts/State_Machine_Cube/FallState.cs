using UnityEngine;

namespace State_Machine_Cube
{
    public class FallState : IStateCube
    {
        private readonly CubeController cube;

        private Vector3 fromPosition;
        private Vector3 toPosition;

        public FallState(CubeController pCube)
        {
            cube = pCube;
        }

        public void EnterState()
        {
            fromPosition = cube.GridToWorld(cube.PrevPos);
            toPosition = cube.GridToWorld(cube.GridPosition);
        }

        public void UpdateState(float pRatio)
        {
            cube.transform.position = Vector3.Lerp(fromPosition, toPosition, pRatio);
        }

        public void ExitState()
        {
        }
    }
}