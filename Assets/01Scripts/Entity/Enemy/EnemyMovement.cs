using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour, IMovement
{
    [field: SerializeField] public MovementData movementData { get; protected set; }
    public Vector3 Velocity { get; }

    public NavMeshAgent NavMeshAgent => _navMeshAgent;

    public bool IsJumpFalling => throw new System.NotImplementedException();

    private Enemy _enemy;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody RB;

    public bool IsFacingRight { get; protected set; }

    private void Start()
    {
        IsFacingRight = true;
    }

    private void Update()
    {
        _navMeshAgent.updateRotation = false;

        bool doNotTurn = false;
        if (!doNotTurn)
            CheckDirectionToFace(_navMeshAgent.desiredVelocity.x);
    }

    public void Initialize(Entity entity)
    {
        _enemy = entity as Enemy;

        RB = GetComponent<Rigidbody>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.speed = movementData.runMaxSpeed;
    }

    public void DisableNavMeshAgent()
    {
        _navMeshAgent.enabled = false;
    }

    public void SetDestination(Vector3 destination)
    {
        if (!_navMeshAgent.enabled)
            return;

        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(destination);
    }

    public void SetVelocity(float x, float y, float z, bool doNotTurn = false) { }

    public void StopImmediately()
    {
        if (!_navMeshAgent.enabled)
            return;

        _navMeshAgent.isStopped = true;
    }

    public void StopImmediately(bool withAxisY, bool withAxisZ)
    {
        if (!_navMeshAgent.enabled)
            return;

        //if (withAxisY && withAxisZ)
        //    RB.velocity = Vector3.zero;
        //else if (withAxisY)
        //    RB.velocity = new Vector3(0, 0, RB.velocity.z);
        //else if (withAxisZ)
        //    RB.velocity = new Vector3(0, RB.velocity.y, 0);
        //else
        //    RB.velocity = new Vector3(RB.velocity.x, 0, 0);

        _navMeshAgent.isStopped = true;
    }

    public void SetJump()
    {
    }

    public bool DetectGround()
    {
        return true;
    }

    private void CheckDirectionToFace(float x)
    {
        if (Mathf.Abs(x) > Mathf.Epsilon)
        {
            bool isMovingRight = x > Mathf.Epsilon;
            if (isMovingRight != IsFacingRight)
                Turn();
        }
    }

    private void Turn()
    {
        float yRotation = IsFacingRight ? 180f : 0;
        transform.rotation = Quaternion.Euler(transform.rotation.x, yRotation, transform.rotation.z);
        IsFacingRight = !IsFacingRight;
    }
}
