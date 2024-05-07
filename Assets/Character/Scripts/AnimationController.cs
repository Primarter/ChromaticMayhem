using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knossos.Character
{

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    public Config config;

    public UnityEvent<int> onHitActive;
    public UnityEvent<int> onHitInactive;
    public UnityEvent<int> onHitConnect;
    public UnityEvent<int> onHitStopEnd;
    public UnityEvent<int> onDamageAnimStart;
    public UnityEvent<int> onDamageAnimEnd;
    public UnityEvent onSpecialAvailable;

    Animator animator;
    Vector3 smoothInput = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    Camera mainCam;

    Coroutine hitStopCoroutine = null;

    private void Awake()
    {
        mainCam = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetFloat("DodgeSpeed", config.DodgeAnimationSpeedMultiplier);
    }

    private void Update()
    {
        Vector3 movement = new(InputManager.inputs.horizontal, 0f, InputManager.inputs.vertical);
        smoothInput = Vector3.SmoothDamp(smoothInput, movement, ref velocity, 5 * Time.deltaTime);
        movement = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0) * smoothInput;
        movement = Vector3.ClampMagnitude(movement, 1);

        animator.SetFloat("Speed", movement.magnitude);

        #if UNITY_EDITOR
            animator.SetFloat("DodgeSpeed", config.DodgeAnimationSpeedMultiplier);
        #endif
    }

    // Animator control functions

    public void StopAnimation()
    {
        animator.speed = 0;
    }

    public void ResumeAnimation()
    {
        animator.speed = 1;
    }

    public void TriggerDodge()
    {
        animator.SetTrigger("Dodge");
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void TriggerSpecial()
    {
        animator.SetTrigger("Special");
    }

    public void ResetAttack()
    {
        animator.ResetTrigger("Attack");
    }

    public void ResetSpecial()
    {
        animator.ResetTrigger("Special");
    }

    public void ResetDodge()
    {
        animator.ResetTrigger("Dodge");
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        animator.SetFloat("MovementSpeed", moveSpeed);
    }

    // Animation Events control functions

    public void OnHitActiveEvent(int hit)
    {
        // print("HitActive");
        onHitActive.Invoke(hit);
    }

    public void OnHitConnectEvent(int hit)
    {
        // print("HitConnect");
        onHitConnect.Invoke(hit);
    }

    public void OnHitInactiveEvent(int hit)
    {
        // print("HitInactive");
        onHitInactive.Invoke(hit);
    }

    // Hitstop control

    public void TriggerHitStop(int hitIdx)
    {
        if (hitIdx >= 0 && hitIdx < config.moves.Length)
        {
            StopAnimation();

            // if (config.moves[hitIdx].hitStop < 0)
            //     return;
            if (hitStopCoroutine != null)
                StopCoroutine(hitStopCoroutine);
            hitStopCoroutine = StartCoroutine(EndHitStopAfter(config.moves[hitIdx].hitStop, hitIdx));
        }
        else
            Debug.LogError("Invalid hitIdx in TriggerHitStop");
    }

    IEnumerator EndHitStopAfter(int frames, int hitIdx)
    {
        int startFrame = Time.frameCount;

        while (Time.frameCount < startFrame + frames)
            yield return null;

        ResumeAnimation();
        onHitStopEnd.Invoke(hitIdx);

        hitStopCoroutine = null;
    }
}

}