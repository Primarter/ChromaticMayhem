using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

public class AttackController : MonoBehaviour
{
    Controller mainController;
    AnimationController animationController;
    DodgeController dodgeController;
    Config config;

    [HideInInspector] public float attMoveSpeedMult = 1f;
    [HideInInspector] public float attRotSpeedMult = 1f;

    private bool canSpecial = true;

    private void Awake()
    {
        mainController = GetComponent<Controller>();
        animationController = GetComponent<AnimationController>();
        dodgeController = GetComponent<DodgeController>();
        if (mainController == null || animationController == null || dodgeController == null) {
            this.enabled = false;
            return;
        }
        config = mainController.config;
    }

    public void UpdateAttack()
    {
        if (InputManager.CheckBuffer(BufferedInput.Attack, false))
        {
            dodgeController.canDash = false;
            EnableSlowMotion();
            animationController.ResetDodge();
            animationController.ResetSpecial();
            animationController.TriggerAttack();
        }
        if (InputManager.CheckBuffer(BufferedInput.Special, false) && canSpecial)
        {
            dodgeController.canDash = false;
            EnableSlowMotion();
            animationController.ResetDodge();
            animationController.ResetAttack();
            animationController.TriggerSpecial();
        }
    }

    public void EnableSlowMotion()
    {
        attMoveSpeedMult = config.attackMoveSpeedMultiplier;
        attRotSpeedMult = config.attackRotationSpeedMultiplier;
    }

    public void DisableSlowMotion()
    {
        attMoveSpeedMult = 1f;
        attRotSpeedMult = 1f;
    }

    public void StopAttacking()
    {
        dodgeController.canDash = true;
        DisableSlowMotion();
    }

    public void StartSpecial()
    {
        // GetComponent<SoundController>().PlaySpecialAttackSound();
        StartCoroutine(SpecialCooldown());
    }

    private IEnumerator SpecialCooldown()
    {
        canSpecial = false;
        yield return new WaitForSeconds(config.specialCooldown);
        canSpecial = true;
        animationController.onSpecialAvailable.Invoke();
    }
}

}