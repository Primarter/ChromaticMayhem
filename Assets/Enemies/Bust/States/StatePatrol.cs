using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Knossos.Bust
{
    public class StatePatrol : FSM.State
    {
        BustAgent agent;

        Coroutine patrolCoroutine;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.speed = agent.config.defaultSpeed;
            agent.locomotionSystem.navMeshAgent.isStopped = false;
            patrolCoroutine = agent.StartCoroutine(Patrol());
        }

        public override void Exit(int nextState)
        {
            agent.StopCoroutine(patrolCoroutine);
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

        IEnumerator Patrol()
        {
            while (true)
            {
                agent.locomotionSystem.navMeshAgent.SetDestination(findNewDestination());
                yield return new WaitForSeconds(10f);
            }
        }

        Vector3 findNewDestination()
        {
            float minRange = 2f;
            float maxRange = 10f;
            Vector3 point = Random.onUnitSphere * Random.Range(minRange, maxRange);

            NavMeshHit hit;
            NavMesh.SamplePosition(agent.transform.position + point, out hit, 30f, 1);

            return hit.position;
        }
    }
}