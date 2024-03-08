using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos.FSM;

namespace Knossos.Minotaur
{
    public class StateSleep : FSM.State
    {
        MinotaurAgent agent;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.isStopped = true;
        }

        public override void Exit(int nextState)
        {
        }

        public override void FixedUpdate()
        {
            if (agent.soundSensorSystem.heardSuspiciousSound)
            {
                agent.stateMachine.ChangeState(State.Alert);
            }
        }

        public override void Update()
        {
        }
    }
}