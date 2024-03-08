using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StatePursue : FSM.State
    {
        BustAgent agent;
        bool isCoolingDown = false;
        Coroutine cooldown;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.speed = agent.config.defaultSpeed;
            if ((State)previousState == State.Endlag)
                cooldown = agent.StartCoroutine(Cooldown());

            if ((State)previousState == State.Patrol || (State)previousState == State.Idle)
            {
                agent.detectionSystem?.PropagateDetection();
            }
        }

        public override void Exit(int nextState)
        {
            if (cooldown != null)
                agent.StopCoroutine(cooldown);
            isCoolingDown = false;
        }

        public override void FixedUpdate()
        {
            if (agent.targetingSystem.hasTarget)
            {
                agent.locomotionSystem.navMeshAgent.destination = agent.targetingSystem.target.position;
                if (agent.targetingSystem.isInRange)
                {
                    agent.locomotionSystem.navMeshAgent.isStopped = true;
                    if (!isCoolingDown)
                        agent.stateMachine.ChangeState(State.ChargeAttack);
                    else
                    {
                        Vector3 lookPos = agent.locomotionSystem.navMeshAgent.destination - agent.transform.position;
                        lookPos.y = 0;
                        Quaternion rotation = Quaternion.LookRotation(lookPos);
                        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, 10 * Time.fixedDeltaTime);
                    }
                }
                else
                    agent.locomotionSystem.navMeshAgent.isStopped = false;
            }
            else
                agent.stateMachine.ChangeState(State.Patrol);
        }

        public override void Update()
        {
        }

        IEnumerator Cooldown()
        {
            isCoolingDown = true;
            yield return new WaitForSeconds(agent.config.attackCooldown);
            isCoolingDown = false;
        }
    }
}