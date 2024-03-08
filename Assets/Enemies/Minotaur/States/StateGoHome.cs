using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Knossos.FSM;

namespace Knossos.Minotaur
{
    public class StateGoHome : FSM.State
    {
        MinotaurAgent agent;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.isStopped = false;

            Waypoint[] lairWaypoints = agent.pathSystem.waypoints.Where(w => w.obj.GetComponent<WaypointObject>().isLair == true).ToArray();

            if (lairWaypoints.Length > 0)
            {
                agent.locomotionSystem.navMeshAgent.destination = lairWaypoints[0].obj.transform.position;
            }

            // TODO: pathfind nodes to nearest lair using waypoints to create path
        }

        public override void Exit(int nextState)
        {
        }

        public override void FixedUpdate()
        {
            if (agent.visionSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.Follow);
            }

            if (agent.soundSensorSystem.heardSuspiciousSound)
            {
                agent.stateMachine.ChangeState(State.Alert);
            }

            if (agent.locomotionSystem.navMeshAgent.remainingDistance < 1f)
            {
                agent.stateMachine.ChangeState(State.Sleep);
            }
        }

        public override void Update()
        {
        }
    }
}