using UnityEngine;
using State_Machine_Cube;
using Tiles;

public class CubeController : MonoBehaviour
{
    private const float ROT_ANGLE = 90f;

    private const int TWO_WAIT_TICK = 2;

    private const int ONE_WAIT_TICK = 1;

    private Vector3 gridOrigin;
    private Vector3 baseScale;

    private Vector3Int gridPos;
    private Vector3Int prevPos;
    private Vector3Int nextPos;
    private Vector3Int direction;
    private Vector3Int tpToDest;
    private CubeColor colorType = CubeColor.Red;

    private IStateCube nextState;
    private bool hasNextAction = false;
    private int waitTicksLeft = 0;
    private bool isActionDone = false;
    private bool isOutConveyor = false;
    private bool isTeleporting = false;
    private bool isArrived = false;
    private bool isFinished = false;
    private bool isLost = false;

    public Vector3 BaseScale => baseScale;
    public float RotAngle => ROT_ANGLE;
    public Vector3Int GridPosition => gridPos;
    public Vector3Int PrevPos => prevPos;
    public Vector3Int NextPosition => nextPos;
    public CubeColor ColorType => colorType;
    public bool IsArrived => isArrived;
    public bool IsFinished => isFinished;
    public bool IsLost => isLost;

    private CubeStateMachine stateMachine;

    private VoidState voidState;
    private MoveState moveState;
    private FallState fallState;
    private ConveyState conveyState;
    private SpawnState appearState;
    private DespawnState despawnState;

    public void Init(Vector3 pGridOrigin, Vector3Int pGridPos, Vector3Int pDirection, CubeColor pColor)
    {
        gridOrigin = pGridOrigin;
        gridPos = pGridPos;
        prevPos = pGridPos;
        direction = pDirection;
        colorType = pColor;
        CubeColors.Paint(gameObject, CubeColors.ToColor(pColor));
        baseScale = transform.localScale;
        transform.localScale = Vector3.zero;

        stateMachine = new CubeStateMachine();
        voidState = new VoidState();
        moveState = new MoveState(this);
        fallState = new FallState(this);
        conveyState = new ConveyState(this);
        appearState = new SpawnState(this);
        despawnState = new DespawnState(this);

        stateMachine.ChangeState(voidState);
        NextMove(pGridPos, appearState);
    }

    public void TickPrep(Level pLevel)
    {
        hasNextAction = false;

        if (PreparePendingAction())
        {
            return;
        }

        Tile lUnderTile = pLevel.GetTile(gridPos + Vector3Int.down);
        if (lUnderTile == null)
        {
            Fall(pLevel.LowestHeight);
            return;
        }

        if (TryStartAction())
        {
            lUnderTile.ApplyAction(this);
            if (hasNextAction)
            {
                return;
            }
        }

        Roll(pLevel.IsObstacle(gridPos + direction));
    }

    public void OnTick()
    {
        isOutConveyor = nextState == conveyState;
        prevPos = gridPos;
        gridPos = nextPos;
        stateMachine.ChangeState(nextState);
    }

    public void UpdateState(float pRatio)
    {
        stateMachine.UpdateState(pRatio);
    }

    public Vector3 GridToWorld(Vector3Int pGridPos)
    {
        return gridOrigin + (Vector3)pGridPos;
    }

    public void SetDirection(Vector3Int pDirection)
    {
        direction = pDirection;
    }

    public void TurnRight()
    {
        Vector3 lNewDirection = Quaternion.AngleAxis(ROT_ANGLE, Vector3.up) * (Vector3)direction;
        direction = Vector3Int.RoundToInt(lNewDirection);
    }

    public void TurnLeft()
    {
        Vector3 lNewDirection = Quaternion.AngleAxis(-ROT_ANGLE, Vector3.up) * (Vector3)direction;
        direction = Vector3Int.RoundToInt(lNewDirection);
    }

    public void Wait(int pTicks)
    {
        waitTicksLeft = pTicks - 1;
        if (waitTicksLeft < 0)
        {
            waitTicksLeft = 0;
        }
        NextStay(voidState);
    }

    public void StopAfterConvey()
    {
        if (isOutConveyor && ONE_WAIT_TICK > 0)
        {
            Wait(ONE_WAIT_TICK);
        }
    }

    public void Convey(Vector3Int pDirection, bool pIsDestinationBlocked)
    {
        if (pIsDestinationBlocked)
        {
            isActionDone = false;
            NextStay(voidState);
        }
        else
        {
            NextMove(gridPos + pDirection, conveyState);
        }
    }

    public void Teleport(Vector3Int pDestination)
    {
        tpToDest = pDestination;
        isTeleporting = true;
        NextStay(despawnState);
    }

    private bool PreparePendingAction()
    {
        if (isArrived)
        {
            isFinished = true;
            return true;
        }
        else if (waitTicksLeft > 0)
        {
            waitTicksLeft--;
            NextStay(voidState);
            return true;
        }
        else if (isTeleporting)
        {
            isTeleporting = false;
            NextMove(tpToDest, appearState);
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
        return true;
    }

    private void Roll(bool pIsFrontBlocked)
    {
        if (pIsFrontBlocked)
        {
            TurnRight();
            Wait(TWO_WAIT_TICK);
        }
        else
        {
            NextMove(gridPos + direction, moveState);
        }
    }

    public void Arrive()
    {
        isArrived = true;
        NextStay(despawnState);
    }

    private void Fall(int pLowestHeight)
    {
        NextMove(gridPos + Vector3Int.down, fallState);
        if (nextPos.y < pLowestHeight)
        {
            isLost = true;
        }
    }

    private void NextMove(Vector3Int pNextPos, IStateCube pNextState)
    {
        nextPos = pNextPos;
        nextState = pNextState;
        isActionDone = false;
        hasNextAction = true;
    }

    private void NextStay(IStateCube pNextState)
    {
        nextPos = gridPos;
        nextState = pNextState;
        hasNextAction = true;
    }
}
