using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace Knossos.Minotaur
{

public class AttackSystem : MonoBehaviour
{
    MinotaurAgent agent;
    [SerializeField] ColliderManager attackColliderManager;
    [SerializeField] int damage = 50;

    bool _hitting = false;
    public bool hitting
    {
        get => _hitting;
        set
        {
            _hitting = value;
        }
    }

    void Awake()
    {
        agent = GetComponent<MinotaurAgent>();
    }

    private void FixedUpdate()
    {
        if (hitting)
        {
            foreach (var gameObject in attackColliderManager.GetColliding())
            {
                if (gameObject.tag == "Player")
                {
                    Vector3 playerPos = gameObject.transform.position;
                    Character.Health health = gameObject.GetComponent<Character.Health>();
                    bool hasHit = health.TakeDamage(damage, out bool died);
                    if (died)
                    {
                        agent.visionSystem.hasTarget = false;
                        agent.stateMachine.ChangeState(State.Patrol);
                    }
                    if (hasHit)
                    {
                        hitting = false;
                        break;
                    }
                }
            }
        }
    }
}

}
