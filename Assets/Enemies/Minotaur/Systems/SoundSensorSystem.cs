using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Minotaur
{
    public class SoundSensorSystem : MonoBehaviour
    {
        public bool heardSuspiciousSound = false;
        public Vector3 suspiciousSoundPosition;

        public float heardSoundTime = 1f;
        public float heardSoundTimer;

        void OnEnable()
        {
            LoudSoundManager.OnLoudSound += heardSound;
        }

        void OnDisable()
        {
            LoudSoundManager.OnLoudSound -= heardSound;
        }

        void Start()
        {
            heardSuspiciousSound = false;
        }

        void FixedUpdate()
        {
            heardSoundTimer -= Time.fixedDeltaTime;
            if (heardSoundTimer <= 0f)
            {
                heardSuspiciousSound = false;
            }
        }

        public void heardSound(Vector3 position)
        {
            heardSoundTimer = heardSoundTime;

            heardSuspiciousSound = true;
            suspiciousSoundPosition = position;
        }
    }
}
