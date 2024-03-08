using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Knossos.Bust
{
    public class LocomotionSystem : MonoBehaviour
    {
        BustAgent agent;
        public NavMeshAgent navMeshAgent;

        void Awake()
        {
            agent = GetComponent<BustAgent>();
            if (agent == null)
                Debug.LogError("GetComponent failed");
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Start()
        {
            navMeshAgent.speed = agent.config.defaultSpeed;
        }

        void Update()
        {
            if (!navMeshAgent.enabled) return;
            if (!navMeshAgent.isStopped) return;

            // Vector3 dir = new Vector3(transform.forward.x, 0f, transform.forward.z);
            // navMeshAgent.Move(dir * navMeshAgent.speed * Time.deltaTime);
        }

        private void OnDrawGizmos() {
            if (navMeshAgent) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(navMeshAgent.destination, 1f);
            }
        }

        private void OnDisable()
        {
            // navMeshAgent.velocity = Vector3.zero;
            // if (isActiveAndEnabled)
            //     navMeshAgent.isStopped = true;
        }

        private void OnEnable()
        {
            // if (isActiveAndEnabled)
            //     navMeshAgent.isStopped = false;
        }
    }
}