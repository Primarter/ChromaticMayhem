using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{

public class StaggerSystem : MonoBehaviour
{
    BustAgent agent;

    public Enemies.OnHitEventSystem.HitInfo lastHit;

    void Awake()
    {
        agent = GetComponent<BustAgent>();
        if (agent == null)
                Debug.LogError("StaggerSystem is lacking BustAgent");
    }

    public void OnHitCallback(Enemies.OnHitEventSystem.HitInfo hit)
    {
        if (agent.currentState != State.Attacking && agent.isActiveAndEnabled)
        {
            lastHit = hit;
            agent.stateMachine.ChangeState(State.Staggered);
        }
    }
}

}