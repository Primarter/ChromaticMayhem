using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StateEndlag : FSM.State
    {
        BustAgent agent;
        Coroutine cooldown;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.isStopped = true;

            cooldown = agent.StartCoroutine(CooldownTimer());
        }

        public override void Exit(int nextState)
        {
            agent.locomotionSystem.navMeshAgent.isStopped = false;

            agent.StopCoroutine(cooldown);
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            agent.locomotionSystem.navMeshAgent.destination = agent.targetingSystem.target.position;
            Vector3 lookPos = agent.locomotionSystem.navMeshAgent.destination - agent.transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, 10 * Time.deltaTime);
        }

        IEnumerator CooldownTimer()
        {
            yield return new WaitForSeconds(agent.config.attackEndlag);
            agent.stateMachine.ChangeState(State.Pursue);
        }
    }
}