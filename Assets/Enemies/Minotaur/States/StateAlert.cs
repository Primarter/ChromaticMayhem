using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos.FSM;

namespace Knossos.Minotaur
{
    public class StateAlert : FSM.State
    {
        MinotaurAgent agent;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.soundSensorSystem.heardSuspiciousSound = false;

            agent.locomotionSystem.navMeshAgent.isStopped = false;
            agent.locomotionSystem.navMeshAgent.speed = 5.5f;
            agent.locomotionSystem.navMeshAgent.destination = agent.soundSensorSystem.suspiciousSoundPosition;
        }

        public override void Exit(int nextState)
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            if (agent.staggerSystem.stagger)
                agent.stateMachine.ChangeState(State.Staggered);
            if (agent.visionSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.Follow);
            }

            if (agent.locomotionSystem.navMeshAgent.remainingDistance < 1f)
            {
                agent.stateMachine.ChangeState(State.Patrol);
            }
        }
    }
}