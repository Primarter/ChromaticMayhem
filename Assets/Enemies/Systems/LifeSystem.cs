using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Enemies
{

[RequireComponent(typeof(OnHitEventSystem))]
public class LifeSystem : MonoBehaviour
{
    [SerializeField]
    int startHealth = 3;

    int health;

    private void Start() {
        health = startHealth;
    }

    public void TakeDamage(OnHitEventSystem.HitInfo hitInfo)
    {
        health -= hitInfo.damage;
        if (health <= 0)
        {
            this.gameObject.SetActive(false); // disable instead of destroy because cluster hold an array of reference
            // Destroy(gameObject);
        }
    }
}

}