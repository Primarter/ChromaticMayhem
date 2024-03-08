using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Knossos.Bust
{
    public class StateIdle : FSM.State
    {
        BustAgent agent;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.speed = agent.config.defaultSpeed;
            agent.locomotionSystem.navMeshAgent.isStopped = true;
        }

        public override void Exit(int nextState)
        {
            agent.locomotionSystem.navMeshAgent.isStopped = false;
        }

        public override void FixedUpdate()
        {
            if (agent.targetingSystem.hasTarget)
            {
                agent.stateMachine.ChangeState(State.Pursue);
            }
        }

        public override void Update()
        {
        }
    }
}