using UnityEngine;
using UnityEngine.AI;

public class AIPatrol : MonoBehaviour
{
    private NavMeshAgent agent;

    private AIBattle AIBattle;

    private int destinationPoint;

    [SerializeField] private Transform[] patrolPointsTransforms;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        AIBattle = GetComponent<AIBattle>();

        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;

        GotoNextPoint();
    }

    private void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f /* && !AIBattle.isBattle*/)
        {
            GotoNextPoint();
        }
    }

    private void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (patrolPointsTransforms.Length == 0)
        {
            return;
        }

        // Set the agent to go to the currently selected destination.
        agent.destination = patrolPointsTransforms[destinationPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destinationPoint = (destinationPoint + 1) % patrolPointsTransforms.Length;
    }
}