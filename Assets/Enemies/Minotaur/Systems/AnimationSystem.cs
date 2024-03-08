using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knossos.Minotaur
{

public class AnimationSystem : MonoBehaviour
{
    public Animator animator;
    public UnityEvent OnAttackActive;
    public UnityEvent OnAttackInactive;

    [HideInInspector] public MinotaurAgent minotaurAgent;

    [HideInInspector] public bool isAttacking = false;

    private void Awake()
    {
        if (animator == null)
        {
            this.enabled = false;
            return;
        }

        minotaurAgent = GetComponent<MinotaurAgent>();
        if (minotaurAgent == null)
            this.enabled = false;
    }

    private void Update()
    {
        if (!isAttacking)
        {
            var agent = minotaurAgent.locomotionSystem.navMeshAgent;
            float v = agent.speed > 0f ? agent.velocity.magnitude / agent.speed : 0f;
            animator.SetFloat("Velocity", v);
            animator.ResetTrigger("Attack");
        }
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void OnAttackActiveEvent()
    {
        OnAttackActive.Invoke();
    }

    public void OnAttackInactiveEvent()
    {
        OnAttackInactive.Invoke();
    }
}

}