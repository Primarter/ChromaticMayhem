using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knossos.Character
{

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    public int hitIdx = 0;
    [SerializeField] AnimationController animationController;
    [SerializeField] GameObject damageParticle;
    [Tooltip("This is not linked to MonoBehaviour OnEnable")]
    [SerializeField] UnityEvent onEnableHitbox;
    [Tooltip("This is not linked to MonoBehaviour OnDisable")]
    [SerializeField] UnityEvent onDisableHitbox;

    Transform player;
    List<Enemies.OnHitEventSystem> enemies = new();
    List<Enemies.OnHitEventSystem> hitEnemies = new();
    bool hitting = false;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        enemies.RemoveAll(en => en == null || !en.isActiveAndEnabled);
        if (!hitting)
            return;
        foreach (Enemies.OnHitEventSystem enemy in enemies)
        {
            if (!hitEnemies.Contains(enemy))
            {
                Hit(enemy);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var en = other.GetComponent<Enemies.OnHitEventSystem>();
        if (en != null)
            enemies.Add(en);
    }

    private void OnTriggerExit(Collider other)
    {
        var en = other.GetComponent<Enemies.OnHitEventSystem>();
        if (en != null)
            enemies.Remove(en);
    }

    private void Hit(Enemies.OnHitEventSystem enemy)
    {
        Vector3 dir = enemy.transform.position - player.position;
        dir.y = 0;
        enemy.TakeDamage(animationController.config.moves[hitIdx], dir);
        hitEnemies.Add(enemy);
        animationController.OnHitConnectEvent(hitIdx);
    }

    public void EnableHitbox()
    {
        onEnableHitbox.Invoke();
        hitting = true;
        foreach (Enemies.OnHitEventSystem enemy in enemies)
        {
            Hit(enemy);
        }
    }

    public void DisableHitbox()
    {
        onDisableHitbox.Invoke();
        hitting = false;
        hitEnemies.Clear();
    }
}

}