using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bell
{

public class AttractMinotaur : MonoBehaviour
{
    public AudioClip bellSound;
    GameObject minotaur;

    MinotaurSpawnPoint[] closeMinotaurSpawnPoints;

    void Awake()
    {
        minotaur = GameObject.FindWithTag("Minotaur");
        closeMinotaurSpawnPoints = transform.parent.GetComponentsInChildren<MinotaurSpawnPoint>();
    }

    public void RingBell()
    {
        if (minotaur != null && !minotaur.activeInHierarchy) // if not active, place minotaur at random spawnPoint and activate it
        {
            Vector3 p = closeMinotaurSpawnPoints[Random.Range(0, closeMinotaurSpawnPoints.Length)].transform.position;
            minotaur.SetActive(true);
            minotaur.GetComponent<Minotaur.LocomotionSystem>().navMeshAgent.Warp(p);
        }

        LoudSoundManager.playLoudSound(transform.position);
        SoundManager.PlaySound(transform.position, bellSound, volume:2f, maxDistance: 40, spatialBlend: 0.85f, pitch: Random.Range(0.9f, 1.1f));
    }
}

}