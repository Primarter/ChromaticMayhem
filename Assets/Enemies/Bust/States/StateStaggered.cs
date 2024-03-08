using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class StateStagered : FSM.State
    {
        BustAgent agent;
        Quaternion originalRotation;
        int previousState;
        Coroutine staggered;
        Coroutine knockback;

        public override void Init()
        {
            agent = obj.GetComponent<BustAgent>();
        }

        public override void Enter(int previousState)
        {
            this.previousState = previousState;

            agent.locomotionSystem.navMeshAgent.updateRotation = false;
            agent.locomotionSystem.navMeshAgent.destination = agent.transform.position + agent.staggerSystem.lastHit.hitDirection.normalized;
            agent.locomotionSystem.navMeshAgent.speed = agent.config.defaultSpeed * agent.staggerSystem.lastHit.knockBackStrength;

            knockback = agent.StartCoroutine(KnockBackTimer());
            staggered = agent.StartCoroutine(StaggeredTimer());
        }

        public override void Exit(int nextState)
        {
            // agent.transform.rotation = originalRotation;
            agent.locomotionSystem.navMeshAgent.destination = agent.transform.position;
            agent.locomotionSystem.navMeshAgent.updateRotation = true;
            agent.locomotionSystem.navMeshAgent.isStopped = false;
            if (staggered != null)
                agent.StopCoroutine(staggered);
            if (knockback != null)
                agent.StopCoroutine(knockback);
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            // agent.transform.rotation = originalRotation;
        }

        IEnumerator KnockBackTimer()
        {
            var start = Time.time;

            while (Time.time < start + agent.staggerSystem.lastHit.knockBackDuration)
            {
                agent.locomotionSystem.navMeshAgent.destination = agent.locomotionSystem.navMeshAgent.transform.position + agent.staggerSystem.lastHit.hitDirection.normalized;
                float progress = Easing.easeOutExpo(1f - ((Time.time - start) / agent.staggerSystem.lastHit.knockBackDuration));
                agent.locomotionSystem.navMeshAgent.speed = agent.config.defaultSpeed * agent.staggerSystem.lastHit.knockBackStrength * progress;
                yield return null;
            }
            agent.locomotionSystem.navMeshAgent.speed = 0;
            agent.locomotionSystem.navMeshAgent.isStopped = true;
        }

        IEnumerator StaggeredTimer()
        {
            yield return new WaitForSeconds(agent.staggerSystem.lastHit.staggerDuration);
            agent.stateMachine.ChangeState(State.Pursue);
        }
    }
}