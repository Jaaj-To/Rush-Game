using State_Machine_Cube;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private float rotAngle = 90f;
    [SerializeField] private int obstacleWaitTicks = 2;
    [SerializeField] private int conveyorEndWaitTicks = 1;

    #endregion Serialized Fields

    #region Private Variables

    private Vector3 gridOrigin;
    private Vector3 baseScale;

    private Vector3Int gridPosition;
    private Vector3Int previousPosition;
    private Vector3Int nextPosition;
    private Vector3Int direction;
    private Vector3Int teleportDestination;

    private IStateCube nextState;
    private int waitTicksLeft;
    private bool isActionDone;
    private bool isComingFromConveyor;
    private bool isTeleporting;
    private bool isArrived;
    private bool isFinished;
    private bool isLost;

    public Vector3 BaseScale
    {
        get
        {
            return baseScale;
        }
    }

    public float RotAngle
    {
        get
        {
            return rotAngle;
        }
    }

    public Vector3Int GridPosition
    {
        get
        {
            return gridPosition;
        }
    }

    public Vector3Int PreviousPosition
    {
        get
        {
            return previousPosition;
        }
    }

    public Vector3Int NextPosition
    {
        get
        {
            return nextPosition;
        }
    }

    public bool IsArrived
    {
        get
        {
            return isArrived;
        }
    }

    public bool IsFinished
    {
        get
        {
            return isFinished;
        }
    }

    public bool IsLost
    {
        get
        {
            return isLost;
        }
    }

    #endregion Private Variables

    #region State Machine

    private CubeStateMachine stateMachine;

    private VoidState voidState;
    private MoveState moveState;
    private FallState fallState;
    private ConveyState conveyState;
    private AppearState appearState;
    private DisappearState disappearState;

    #endregion State Machine

    #region Tick

    public void Init(Vector3 pGridOrigin, Vector3Int pGridPosition, Vector3Int pDirection)
    {
        gridOrigin = pGridOrigin;
        gridPosition = pGridPosition;
        previousPosition = pGridPosition;
        direction = pDirection;
        baseScale = transform.localScale;
        transform.localScale = Vector3.zero;

        stateMachine = new CubeStateMachine();
        voidState = new VoidState();
        moveState = new MoveState(this);
        fallState = new FallState(this);
        conveyState = new ConveyState(this);
        appearState = new AppearState(this);
        disappearState = new DisappearState(this);

        stateMachine.ChangeState(voidState);
        PlanMovement(pGridPosition, appearState);
    }

    public void ApplyTick()
    {
        previousPosition = gridPosition;
        gridPosition = nextPosition;
        stateMachine.ChangeState(nextState);
    }

    public void UpdateAnimation(float pRatio)
    {
        stateMachine.UpdateState(pRatio);
    }

    public Vector3 GridToWorld(Vector3Int pGridPosition)
    {
        return gridOrigin + (Vector3)pGridPosition;
    }

    #endregion Tick

    #region Actions

    public void SetDirection(Vector3Int pDirection)
    {
        direction = pDirection;
    }

    public void TurnRight()
    {
        direction = new Vector3Int(direction.z, 0, -direction.x);
    }

    public void TurnLeft()
    {
        direction = new Vector3Int(-direction.z, 0, direction.x);
    }

    public void Wait(int pTicks)
    {
        waitTicksLeft = Mathf.Max(pTicks - 1, 0);
        PlanStay(voidState);
    }

    public void Convey(Vector3Int pDirection, bool pIsDestinationBlocked)
    {
        if (pIsDestinationBlocked)
        {
            isActionDone = false;
            PlanStay(voidState);
            return;
        }

        isComingFromConveyor = true;
        PlanMovement(gridPosition + pDirection, conveyState);
    }

    public void Teleport(Vector3Int pDestination)
    {
        teleportDestination = pDestination;
        isTeleporting = true;
        PlanStay(disappearState);
    }

    #endregion Actions

    #region Implementation

    private bool PreparePendingAction()
    {
        if (isArrived)
        {
            isFinished = true;
            return true;
        }

        if (waitTicksLeft > 0)
        {
            waitTicksLeft--;
            PlanStay(voidState);
            return true;
        }

        if (isTeleporting)
        {
            isTeleporting = false;
            PlanMovement(teleportDestination, appearState);
            isActionDone = true;
            return true;
        }

        return false;
    }

    private bool TryStartAction()
    {
        if (isActionDone)
        {
            return false;
        }

        isActionDone = true;
        isComingFromConveyor = false;
        return true;
    }

    private void RollOnGround(bool pIsFrontBlocked)
    {
        if (isComingFromConveyor)
        {
            isComingFromConveyor = false;

            if (conveyorEndWaitTicks > 0)
            {
                Wait(conveyorEndWaitTicks);
                return;
            }
        }

        Roll(pIsFrontBlocked);
    }

    private void Roll(bool pIsFrontBlocked)
    {
        if (pIsFrontBlocked)
        {
            TurnRight();
            Wait(obstacleWaitTicks);
            return;
        }

        PlanMovement(gridPosition + direction, moveState);
    }

    private void Fall()
    {
        isComingFromConveyor = false;
        PlanMovement(gridPosition + Vector3Int.down, fallState);
    }

    private void Arrive()
    {
        isArrived = true;
        PlanStay(disappearState);
    }

    private void Lose()
    {
        isLost = true;
        PlanStay(voidState);
    }

    private void PlanMovement(Vector3Int pNextPosition, IStateCube pNextState)
    {
        nextPosition = pNextPosition;
        nextState = pNextState;
        isActionDone = false;
    }

    private void PlanStay(IStateCube pNextState)
    {
        nextPosition = gridPosition;
        nextState = pNextState;
    }

    #endregion Implementation
}