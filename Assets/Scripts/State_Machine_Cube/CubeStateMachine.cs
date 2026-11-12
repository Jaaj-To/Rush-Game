using UnityEngine;

namespace State_Machine_Cube
{
    public class CubeStateMachine
    {
        private IStateCube currentState;

        public void UpdateState(float pRatio)
        {
            currentState.UpdateState(pRatio);
        }

        public void ChangeState(IStateCube pNewState)
        {
            Debug.Log(pNewState);
            currentState?.ExitState();
            currentState = pNewState;
            currentState.EnterState();
        }
    }
}
