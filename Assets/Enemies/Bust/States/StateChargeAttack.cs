using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StateChargeAttack : FSM.State
    {
        BustAgent agent;

        Coroutine attackTimerCoroutine;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.attackSystem.chargingVFX.Play();
            agent.locomotionSystem.navMeshAgent.isStopped = true;

            agent.locomotionSystem.navMeshAgent.destination += (agent.locomotionSystem.navMeshAgent.destination - agent.transform.position).normalized * 10;

            attackTimerCoroutine = agent.StartCoroutine(attackTimer());
        }

        public override void Exit(int nextState)
        {
            agent.attackSystem.chargingVFX.Stop();
            agent.locomotionSystem.navMeshAgent.isStopped = false;
            agent.StopCoroutine(attackTimerCoroutine);
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            Vector3 lookPos = agent.locomotionSystem.navMeshAgent.destination - agent.transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, 10 * Time.deltaTime);
        }

        IEnumerator attackTimer()
        {
            yield return new WaitForSeconds(agent.config.attackStartup);
            agent.stateMachine.ChangeState(State.Attacking);
        }

    }
}