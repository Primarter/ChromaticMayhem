using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class TargetingSystem : MonoBehaviour
    {
        BustAgent agent;
        public Transform target;
        [SerializeField] LayerMask visionLayers;

        private bool _hasTarget = false;
        public bool hasTarget
        {
            get => _hasTarget;
            set
            {
                _hasTarget = value;
            }
        }

        public bool isInRange { get => distanceToTarget <= agent.config.attackRange; }

        float distanceToTarget;
        bool usesTriggerZones = false;

        void Awake()
        {
            agent = GetComponent<BustAgent>();
            if (agent == null)
                Debug.LogError("TargetingSystem is lacking BustAgent");
            if (transform.parent != null && transform.parent.tag == "Cluster")
                usesTriggerZones = transform.parent.GetComponentsInChildren<Enemies.TriggerZone>().Length > 0;
        }

        void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            distanceToTarget = Vector3.Distance(transform.position, target.position);
        }

        void FixedUpdate()
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (!hasTarget && !usesTriggerZones && distanceToTarget < agent.config.detectionRange)
                hasTarget = true;
            if (distanceToTarget > agent.config.maxDetectionRange)
                hasTarget = false;
        }

        bool CheckLineOfSight()
        {
            RaycastHit hit;
            Vector3 targetPos = target.position + new Vector3(0f, 1.0f, 0f);
            bool res = Physics.Raycast(transform.position, targetPos - transform.position, out hit, agent.config.detectionRange, visionLayers);
            if (res)
                Debug.Log($"HIT {hit.collider.name} {hit.transform.tag} {hit.transform.gameObject.layer}");
            return res
                && hit.transform.tag == "Player";
        }

        void OnDrawGizmosSelected()
        {
            var ag = GetComponent<BustAgent>();
            if (ag.config != null)
                Gizmos.DrawWireSphere(transform.position, ag.config.detectionRange);
        }
    }
}