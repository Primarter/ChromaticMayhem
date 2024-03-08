using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{
    public class DetectionSystem : MonoBehaviour
    {
        BustAgent agent;

        bool propagated = false;

        void Awake()
        {
            agent = GetComponent<BustAgent>();
            if (agent == null)
                Debug.LogError("DetectionSystem is lacking BustAgent");
        }

        // private void Update()
        // {
        //     if (agent.targetingSystem.hasTarget == false)
        //     {
        //         propagated = false;
        //     }
        // }

        public void PropagateDetection()
        {
            if (transform.parent == null || transform.parent.tag != "Cluster" || propagated)
                return;
            propagated = true;
            transform.parent.GetComponent<Enemies.Cluster>().StartEncounter();
        }

        public void AlertPlayer()
        {
            GameObject.FindWithTag("Player")?.GetComponent<Character.VisibilitySystem>()?.AlertPlayer();
        }

        public void LosePlayer()
        {
            GameObject.FindWithTag("Player")?.GetComponent<Character.VisibilitySystem>()?.LosePlayer();
        }

        private void OnDisable()
        {
            LosePlayer();
        }
    }
}