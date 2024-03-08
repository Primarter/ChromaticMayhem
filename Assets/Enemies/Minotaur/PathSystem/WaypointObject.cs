using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Minotaur
{
    public class WaypointObject : MonoBehaviour
    {
        [SerializeField]
        public bool isLair = false;

        void OnDrawGizmos()
        {
            Gizmos.color = isLair ? Color.red : Color.cyan;
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }
}
