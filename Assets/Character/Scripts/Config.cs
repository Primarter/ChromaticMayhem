using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

[System.Serializable]
public struct MoveInfo
{
    public int damage;
    public int hitStop;
    public float knockBackStrength;
    public float knockBackDuration;
    public float staggerDuration;
}

[CreateAssetMenu()]
public class Config : ScriptableObject
{
    [Header("Player stats")]
    public float speed = 10f;
    public float attackMoveSpeedMultiplier = .01f;
    public float attackRotationSpeedMultiplier = .2f;

    [Header("ControllerConfig")]
    public float rotationSpeed = 10f;

    [Header("Dash config")]
    public AnimationCurve DashSpeedCurve = new();
    public float dashStrength = 20f;
    [Tooltip("Dash animation duration is 1.05s")]
    public float dashDuration = .5f;
    public float dashCooldown = .2f;
    public float lastDashCooldown = .7f;
    public float quickDashTiming = .2f;
    public int maxQuickDash = 2;

    [Header("CombatStats")]
    public MoveInfo[] moves = {
        new MoveInfo {damage=1, hitStop=2, knockBackStrength=0, knockBackDuration=0, staggerDuration=1f},
        new MoveInfo {damage=1, hitStop=2, knockBackStrength=0, knockBackDuration=0, staggerDuration=1f},
        new MoveInfo {damage=1, hitStop=4, knockBackStrength=2, knockBackDuration=0.75f, staggerDuration=1.5f},
    };

    public int[] damageAnimationDurations = {15, 20, 25};

    public float specialCooldown = 5f;

    [Header("Read Only Info")]
    public float DodgeAnimationSpeedMultiplier = 1f;

    private void OnValidate()
    {
        DodgeAnimationSpeedMultiplier = 1.05f / dashDuration;
    }
}

}