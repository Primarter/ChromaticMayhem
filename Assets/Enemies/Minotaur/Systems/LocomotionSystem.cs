using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Knossos.Minotaur
{
    public class LocomotionSystem : MonoBehaviour
    {
        // MinotaurAgent agent;
        public NavMeshAgent navMeshAgent;

        void Awake()
        {
            // agent = GetComponent<MinotaurAgent>();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            navMeshAgent.speed = 3f;
        }

        void Update()
        {
        }
    }
}
