using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StateAttacking : FSM.State
    {
        BustAgent agent;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.isStopped = false;
            agent.locomotionSystem.navMeshAgent.speed = agent.config.attackSpeed;
            agent.capsuleCollider.isTrigger = true;
            agent.StartCoroutine(AttackTimer());
        }

        public override void Exit(int nextState)
        {
            agent.locomotionSystem.navMeshAgent.destination = agent.targetingSystem.target.position;
            agent.capsuleCollider.isTrigger = false;
            agent.StopAllCoroutines();
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
        }

        IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(agent.config.attackDuration);
            agent.stateMachine.ChangeState(State.Endlag);
        }
    }
}
