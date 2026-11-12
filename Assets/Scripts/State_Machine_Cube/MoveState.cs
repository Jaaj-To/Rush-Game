using UnityEngine;

namespace State_Machine_Cube
{
    public class MoveState : IStateCube
    {
        private readonly CubeController cube;

        private Vector3 fromPosition;
        private Vector3 toPosition;
        private Quaternion fromRotation;
        private Quaternion toRotation;
        private float rotationOffsetY;

        public MoveState(CubeController pCube)
        {
            cube = pCube;
        }

        public void EnterState()
        {
            rotationOffsetY = (cube.BaseScale.x * Mathf.Sqrt(2f)) / 2f
                              - cube.BaseScale.x / 2f;

            fromPosition = cube.GridToWorld(cube.PreviousPosition);
            toPosition = cube.GridToWorld(cube.GridPosition);
            fromRotation = cube.transform.rotation;

            Vector3 lAxis = Vector3.Cross(Vector3.up, toPosition - fromPosition);
            toRotation = Quaternion.AngleAxis(cube.RotAngle, lAxis) * fromRotation;
        }

        public void UpdateState(float pRatio)
        {
            cube.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, pRatio);
            cube.transform.position = Vector3.Lerp(fromPosition, toPosition, pRatio) 
                                      + Vector3.up 
                                      * (rotationOffsetY * Mathf.Abs(Mathf.Sin(Mathf.PI * pRatio)));
        }

        public void ExitState()
        {
        }
    }
}