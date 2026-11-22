using UnityEngine;

namespace State_Machine_Cube
{
    public class SpawnState : IStateCube
    {
        private readonly CubeController cube;

        public SpawnState(CubeController pCube)
        {
            cube = pCube;
        }

        public void EnterState()
        {
            cube.transform.position = cube.GridToWorld(cube.GridPosition);
            cube.transform.localScale = Vector3.zero;
        }

        public void UpdateState(float pRatio)
        {
            cube.transform.localScale = Vector3.Lerp(Vector3.zero, cube.BaseScale, pRatio);
        }

        public void ExitState()
        {
        }
    }
}