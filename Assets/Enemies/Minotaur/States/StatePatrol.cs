using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos.FSM;

namespace Knossos.Minotaur
{
    public class StatePatrol : FSM.State
    {
        MinotaurAgent agent;

        // Waypoint targetWaypoint;
        Vector3 targetWaypoint;
        // bool hasTarget = false;
        // float timeOnTarget = 0f;

        public float patrolTime = 60f;
        [SerializeField] float patrolTimeRemaining;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.speed = 3f;
            agent.locomotionSystem.navMeshAgent.isStopped = false;

            // targetWaypoint = null;
            patrolTimeRemaining = patrolTime;
        }

        public override void Exit(int nextState)
        {
        }

        public override void Update()
        {
            if (agent.staggerSystem.stagger)
                agent.stateMachine.ChangeState(State.Staggered);
        }

        public override void FixedUpdate()
        {
            float distanceMinotaurPlayer = Vector3.Distance(agent.visionSystem.player.transform.position, agent.transform.position);
            if (distanceMinotaurPlayer > 30f || agent.playerMinotaurVisibilitySystem.timeSinceVisible > 3f)
            {
                agent.stateMachine.ChangeState(State.Sleep);
                agent.gameObject.SetActive(false);
                return;
            }
            // patrolTimeRemaining -= Time.fixedDeltaTime;
            // if (patrolTimeRemaining <= 0f)
            // {
            //     agent.stateMachine.ChangeState(State.GoHome);
            // }

            if (agent.visionSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.Follow);
            }

            if (agent.soundSensorSystem.heardSuspiciousSound)
            {
                agent.stateMachine.ChangeState(State.Alert);
            }

            updateDestination();
        }

        void updateDestination()
        {
            if (agent.locomotionSystem.navMeshAgent.remainingDistance < 1.5f)
            {
                setTarget(findNewTarget());
            }
        }

        void setTarget(Vector3 target)
        {
            // hasTarget = true;
            targetWaypoint = target;
            agent.locomotionSystem.navMeshAgent.destination = targetWaypoint;
        }

        Vector3 findNewTarget()
        {
            float minRange = 8f;
            float maxRange = 40f;

            Vector3 point = Random.onUnitSphere * Random.Range(minRange, maxRange);
            NavMeshHit hit;
            NavMesh.SamplePosition(agent.transform.position + point, out hit, 20f, 1);

            return hit.position;
        }

        // void updateDestination()
        // {
        //     if (targetWaypoint == null)
        //     {
        //         Waypoint waypoint = agent.pathSystem.getClosestWaypoint();
        //         if (waypoint == null)
        //             return;
        //         targetWaypoint = waypoint;
        //         agent.locomotionSystem.navMeshAgent.destination = targetWaypoint.obj.transform.position;
        //     }
        //     else
        //     {
        //         Vector3 targetWaypoint2D = new Vector3(targetWaypoint.obj.transform.position.x, 0f, targetWaypoint.obj.transform.position.z);
        //         Vector3 agentPosition2D = new Vector3(agent.transform.position.x, 0f, agent.transform.position.z);

        //         // TODO: use navmeshAgent.remaining distance instead of doing math myself
        //         if (Vector3.Distance(targetWaypoint2D, agentPosition2D) < 1f)
        //         {
        //             setTarget(findNewTarget());
        //         }
        //     }
        // }

        // void setTarget(Waypoint target)
        // {
        //     targetWaypoint = target;
        //     agent.locomotionSystem.navMeshAgent.destination = target.obj.transform.position;
        // }

        // Waypoint findNewTarget()
        // {
        //     Waypoint randomWaypoint = targetWaypoint.neighbours[Random.Range(0, targetWaypoint.neighbours.Length)];
        //     return randomWaypoint;
        // }
    }
}