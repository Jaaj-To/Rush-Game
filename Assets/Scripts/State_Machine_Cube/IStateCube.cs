namespace State_Machine_Cube
{
    public interface IStateCube
    {
        void EnterState();
        void UpdateState(float ratio);
        void ExitState();
    }
}
