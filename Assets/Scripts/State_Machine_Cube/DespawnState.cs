using UnityEngine;

namespace State_Machine_Cube
{
    public class DespawnState : IStateCube
    {
        private readonly CubeController cube;

        public DespawnState(CubeController pCube)
        {
            cube = pCube;
        }

        public void EnterState()
        {
        }

        public void UpdateState(float pRatio)
        {
            cube.transform.localScale = Vector3.Lerp(cube.BaseScale, Vector3.zero, pRatio);
        }

        public void ExitState()
        {
        }
    }
}