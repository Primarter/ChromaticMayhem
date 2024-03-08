using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace Knossos.Enemies
{

public class Cluster : MonoBehaviour
{
    public UnityEvent startEncounterEvent;
    public UnityEvent endEncounterEvent;

    private Character.Health player;
    private EnemyAgent[] enemies;
    protected bool startedEncounter = false;

    private void Awake()
    {
        enemies = GetComponentsInChildren<EnemyAgent>();
        if (enemies.Length == 0)
            this.gameObject.SetActive(false);
        player = GameObject.FindWithTag("Player").GetComponent<Character.Health>();
    }

    protected virtual void Update()
    {
        if (startedEncounter)
        {
            foreach (var bust in enemies)
            {
                if (bust && bust.gameObject.activeSelf)
                    return;
            }
            endEncounterEvent.Invoke();
            this.enabled = false;
        }
    }

    public void StartEncounter()
    {
        if (!startedEncounter)
        {
            if (enemies.All(enemy => enemy.gameObject.activeSelf == false)) // if all enemies are dead, don't start encounter
            {
                print("Not starting encounter because all enemies of this cluster are dead!");
                return;
            }

            startEncounterEvent.Invoke();
            startedEncounter = true;
            ActivateAgents();
            player.currentEncounter = this;
        }
    }

    public void StopEncounter()
    {
        if (startedEncounter)
        {
            endEncounterEvent.Invoke();
            startedEncounter = false;
            DisableAgents();
            player.currentEncounter = null;
        }
    }

    protected virtual void DisableAgents()
    {
        foreach (var agent in enemies)
        {
            if (agent && agent.isActiveAndEnabled)
                agent.Disable();
        }
    }

    protected virtual void ActivateAgents()
    {
        foreach (var agent in enemies)
        {
            if (agent)
                agent.Enable();
        }
    }
}

}