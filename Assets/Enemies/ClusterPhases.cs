using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Enemies
{

public class ClusterPhases : Cluster
{
    [SerializeField] Phase[] phases;
    private int currentPhase = 0;

    [System.Serializable]
    private struct Phase
    {
        public EnemyAgent[] enemies;
    }

    private void Start()
    {
        if (phases.Length == 0)
        {
            enabled = false;
            return;
        }
        foreach (var phase in phases)
        {
            foreach (var enemy in phase.enemies)
            {
                enemy.Disable();
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (startedEncounter && currentPhase < phases.Length)
        {
            foreach (var enemy in phases[currentPhase].enemies)
            {
                if (enemy && enemy.isActiveAndEnabled)
                    return;
            }
            currentPhase += 1;
            if (currentPhase < phases.Length)
            {
                foreach (var enemy in phases[currentPhase].enemies)
                {
                    enemy.Enable();
                }
            }
        }
    }

    protected override void DisableAgents()
    {
        if (phases.Length > 0)
        {
            foreach (var agent in phases[currentPhase].enemies)
            {
                if(agent && agent.isActiveAndEnabled)
                    agent.Disable();
            }
        }
    }

    protected override void ActivateAgents()
    {
        if (phases.Length > 0)
        {
            foreach (var agent in phases[currentPhase].enemies)
            {
                if (agent)
                    agent.Enable();
            }
        }
    }

}

}