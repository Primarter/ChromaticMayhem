using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

public class DodgeController : MonoBehaviour
{
    // Dash Control

    Controller mainController;
    AnimationController animationController;
    Config config;

    [HideInInspector] public bool dashing = false;
    [HideInInspector] public bool canDash = true;

    int dodgesLeft = 1;

    Coroutine coroutine;

    private void Awake()
    {
        mainController = GetComponent<Controller>();
        animationController = GetComponent<AnimationController>();
        if (mainController == null || animationController == null)
        {
            this.enabled = false;
            return;
        }
        config = mainController.config;
    }

    private void Start()
    {
        dodgesLeft = config.maxQuickDash;
    }

    public void UpdateDodge()
    {
        if (canDash && dodgesLeft > 0 && InputManager.CheckBuffer(BufferedInput.Dodge, false))
        {
            animationController.TriggerDodge();
            animationController.ResetAttack();
            animationController.ResetSpecial();
        }
    }

    private IEnumerator DashCoroutine()
    {
        Health health = GetComponent<Health>();
        if (health != null)
            health.invincible = true;

        dashing = true;
        // dodgesLeft -= 1;

        if (mainController.movement.magnitude == 0f)
        {
            mainController.movement = transform.forward;
        }

        float startTime = Time.time;
        while (Time.time < startTime + config.dashDuration)
        {
            float curveValue = config.DashSpeedCurve.Evaluate((Time.time - startTime) / config.dashDuration);
            mainController.speed = (curveValue * config.dashStrength + 1) * config.speed;
            yield return null;
        }

        mainController.speed = config.speed;
        dashing = false;
        if (health != null)
            health.invincible = false;

        canDash = false;
        yield return new WaitForSeconds(config.dashCooldown);
        canDash = true;

        // yield return new WaitForSeconds(config.quickDashTiming);
        // canDash = false;

        // yield return new WaitForSeconds(config.lastDashCooldown);
        // canDash = true;
        // dodgesLeft = config.maxQuickDash;
    }

    public void TriggerDash()
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(DashCoroutine());
    }
}

}