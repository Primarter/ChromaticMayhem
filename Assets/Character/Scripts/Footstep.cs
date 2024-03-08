using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

public class Footstep : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Transform leftFoot;
    [SerializeField] Transform rightFoot;

    [SerializeField] AudioClip[] footstepSounds;

    bool lastLeftHit = false;
    bool lastRightHit = false;

    const float footHeightBias = 0.2f;
    const float raycastDistance = 0.2f;
    // TODO: add foot speed affecting volume



    void Update()
    {
        Vector3 offset = new(0f, footHeightBias, 0f);

        bool leftHit = Physics.Raycast(leftFoot.position + offset, Vector3.down, footHeightBias + raycastDistance, groundLayer);
        bool rightHit = Physics.Raycast(rightFoot.position + offset, Vector3.down, footHeightBias + raycastDistance, groundLayer);

        if (leftHit && lastLeftHit == false)
        {
            SoundManager.PlaySound(leftFoot.position, ArrayHelper.PickRandom(footstepSounds), pitch: Random.Range(0.5f, 0.8f), minDistance: 5f, maxDistance: 50f);
        }

        if (rightHit && lastRightHit == false)
        {
            SoundManager.PlaySound(rightFoot.position, ArrayHelper.PickRandom(footstepSounds), pitch: Random.Range(0.5f, 0.8f), minDistance: 5f, maxDistance: 50f);
        }

        lastLeftHit = leftHit;
        lastRightHit = rightHit;
    }
}

}