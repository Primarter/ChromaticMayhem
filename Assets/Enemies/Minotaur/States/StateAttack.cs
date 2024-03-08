using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos.FSM;

namespace Knossos.Minotaur
{
    public class StateAttack : FSM.State
    {
        MinotaurAgent agent;
        float timer;

        public override void Init()
        {
            agent = obj.GetComponent<MinotaurAgent>();
        }

        public override void Enter(int previousState)
        {
            agent.locomotionSystem.navMeshAgent.isStopped = true;
            agent.animationSystem.isAttacking = true;
            agent.locomotionSystem.navMeshAgent.velocity = Vector3.zero;
            timer = 2.4f;
            agent.animationSystem.TriggerAttack();
        }

        public override void Exit(int nextState)
        {
            agent.animationSystem.isAttacking = false;
            agent.attackSystem.hitting = false;
        }

        public override void FixedUpdate()
        {
            timer -= Time.fixedDeltaTime;
            if (timer > 0) return;

            agent.visionSystem.TargetPlayer();
            agent.stateMachine.ChangeState(State.Follow);
        }

        public override void Update()
        {
            if (agent.staggerSystem.stagger)
            {
                agent.animationSystem.animator.CrossFade("Walk", .2f, 0);
                agent.stateMachine.ChangeState(State.Staggered);
            }
        }
    }
}