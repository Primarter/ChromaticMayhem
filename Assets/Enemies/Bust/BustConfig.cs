using UnityEngine;

[CreateAssetMenu()]
public class BustConfig : ScriptableObject
{
    [Header("Stats")]
    public float attackSpeed = 25f;
    public float defaultSpeed = 2f;

    [Header("Timings")]
    public float attackStartup = 0.3f;
    public float attackDuration = 0.5f;
    public float attackEndlag = 1f;
    public float attackCooldown = 2f;
    public float staggerDuration = 1f;

    [Header("Ranges")]
    public float detectionRange = 12f;
    public float maxDetectionRange = 30f;
    public float attackRange = 8f;

    [Header("StateMachine")]
    public Knossos.Bust.State initialState;
}