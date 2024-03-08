using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Enemies
{
    public class TriggerZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (transform.parent == null || transform.parent.tag != "Cluster" || other.tag != "Player")
                return;
            transform.parent.GetComponent<Cluster>()?.StartEncounter();
        }
    }

}