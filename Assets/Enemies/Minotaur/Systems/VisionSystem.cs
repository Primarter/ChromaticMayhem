using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Minotaur
{
    public class VisionSystem : MonoBehaviour
    {
        [HideInInspector] public GameObject player;

        public bool hasTarget = false;
        public Vector3 targetPosition;
        public float timeSinceLastSeePlayer;

        [SerializeField] public float FOV = 150f; // field of view degrees
        [SerializeField] public float viewRange = 20f;
        [SerializeField] public float minDetectionRange = 5f;
        [SerializeField] LayerMask obstructionLayer;

        void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        void Start()
        {
            hasTarget = false;
            timeSinceLastSeePlayer = 0f;
        }

        void FixedUpdate()
        {
            bool canSeePlayer = CanSeePlayer();

            if (player.GetComponent<Character.Hiding>().isHiding)
            {
                if (!(hasTarget || canSeePlayer))
                {
                    // lose target
                    hasTarget = false;
                }
            }
            else // if not hiding
            {
                if (canSeePlayer)
                {
                    hasTarget = true;
                    targetPosition = player.transform.position;
                }
                else
                {
                    hasTarget = false;
                }
            }

            timeSinceLastSeePlayer += Time.fixedDeltaTime;
            if (hasTarget)
                timeSinceLastSeePlayer = 0f;
        }

        void Update()
        {
        }

        public bool CanSeePlayer()
        {
            Vector3 playerPosition = player.transform.position;
            bool isInMinDetectionRange = Vector3.Distance(transform.position, playerPosition) < minDetectionRange;

            return isInMinDetectionRange || CanSeePosition(playerPosition);
        }

        public bool CanSeePosition(Vector3 p)
        {
            Vector3 towardPosition = p - transform.position;
            Vector3 forward = transform.forward;

            Vector2 towardPosition2D = new Vector2(towardPosition.x, towardPosition.z).normalized;
            Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z).normalized;

            float angle = Vector2.Angle(towardPosition2D, forward2D);
            // print((angle, FOV/2f));

            bool isInViewRange = Vector3.Distance(transform.position, p) < viewRange;
            bool isInFOV = angle < (FOV / 2f);
            bool isInDirectVision = !Physics.Raycast(transform.position, towardPosition, towardPosition.magnitude, obstructionLayer);

            return (
                isInViewRange &&
                isInFOV &&
                isInDirectVision
            );
        }

        public void TargetPlayer()
        {
            hasTarget = true;
            targetPosition = player.transform.position;
        }
    }
}